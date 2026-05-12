using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Application.Features.Product.UpdateProduct;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class UpdateFullProductInformationTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductOptionRepository> _productOptionRepositoryMock;
    private readonly Mock<IProductOptionValueRepository> _productOptionValueRepositoryMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IProductImageRepository> _productImageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateFullProductCommandHandler _handler;

    public UpdateFullProductInformationTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productOptionRepositoryMock = new Mock<IProductOptionRepository>();
        _productOptionValueRepositoryMock = new Mock<IProductOptionValueRepository>();
        _fileServiceMock = new Mock<IFileService>();
        _productImageRepositoryMock = new Mock<IProductImageRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateFullProductCommandHandler(
            _productRepositoryMock.Object,
            _productOptionRepositoryMock.Object,
            _productOptionValueRepositoryMock.Object,
            _fileServiceMock.Object,
            _productImageRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        UpdateFullProductCommand command = new()
        {
            Id = Guid.NewGuid(),
            Name = "Yonex Astrox",
            Brand = "Yonex",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
            DeletedImages = new List<Guid>(),
            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = new List<Guid>(),
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns((Product)null);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductError.NotFound(command.Id));

        _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateProductSuccessfully()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid(),
            Name = "Old Product",
            Brand = "Old Brand",
            Description = "Old Description",
            Slug = "old-product"
        };

        UpdateFullProductCommand command = new()
        {
            Id = product.Id,
            Name = "New Product",
            Brand = "Yonex",
            Description = "Updated Description",
            CategoryId = Guid.NewGuid(),
            Status = true,
            DeletedImages = new List<Guid>(),
            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = new List<Guid>(),
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        product.Name.Should().Be(command.Name);
        product.Brand.Should().Be(command.Brand);
        product.Description.Should().Be(command.Description);
        product.CategoryId.Should().Be(command.CategoryId);
        product.Status.Should().Be(command.Status);
        product.Slug.Should().Contain("new-product");

        _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeletedImages_ShouldDeleteImages()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid()
        };

        List<Guid> deletedImages =
        [
            Guid.NewGuid(),
            Guid.NewGuid()
        ];

        UpdateFullProductCommand command = new()
        {
            Id = product.Id,
            Name = "Product",
            Brand = "Brand",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
            DeletedImages = deletedImages,
            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = new List<Guid>(),
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        foreach (Guid imageId in deletedImages)
        {
            _productImageRepositoryMock.Verify(
                x => x.Delete(imageId),
                Times.Once);
        }
    }

    [Fact]
    public async Task Handle_DeletedOptions_ShouldDeleteOptions()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid()
        };

        List<Guid> deletedOptions =
        [
            Guid.NewGuid(),
            Guid.NewGuid()
        ];

        UpdateFullProductCommand command = new()
        {
            Id = product.Id,
            Name = "Product",
            Brand = "Brand",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
            DeletedImages = new List<Guid>(),
            DeletedOptions = deletedOptions,
            DeletedOptionValues = new List<Guid>(),
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        foreach (Guid optionId in deletedOptions)
        {
            _productOptionRepositoryMock.Verify(
                x => x.Delete(optionId),
                Times.Once);
        }
    }

    [Fact]
    public async Task Handle_DeletedOptionValues_ShouldDeleteOptionValues()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid()
        };

        List<Guid> deletedOptionValues =
        [
            Guid.NewGuid(),
            Guid.NewGuid()
        ];

        UpdateFullProductCommand command = new()
        {
            Id = product.Id,
            Name = "Product",
            Brand = "Brand",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
            DeletedImages = new List<Guid>(),
            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = deletedOptionValues,
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        foreach (Guid optionValueId in deletedOptionValues)
        {
            _productOptionValueRepositoryMock.Verify(
                x => x.Delete(optionValueId),
                Times.Once);
        }
    }

    [Fact]
    public async Task Handle_NoDeletedData_ShouldNotCallDeleteMethods()
    {
        // Arrange
        Product product = new()
        {
            Id = Guid.NewGuid()
        };

        UpdateFullProductCommand command = new()
        {
            Id = product.Id,
            Name = "Product",
            Brand = "Brand",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
            DeletedImages = new List<Guid>(),
            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = new List<Guid>(),
            AddedImages = new List<Contracts.API.Presentation.File.Request.FileUploadStreamData>(),
            UpdatedOptions = new List<UpdateOption>(),
            UpdatedOptionValues = new List<UpdateOptionValue>()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productImageRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Guid>()),
            Times.Never);

        _productOptionRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Guid>()),
            Times.Never);

        _productOptionValueRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Guid>()),
            Times.Never);
    }
}