using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Application.Features.Product.UploadProductImage;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Domain.Entities.Catalog;
using CloudinaryDotNet.Actions;
using Moq;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class UploadProductImageTest
{
    private readonly Mock<IProductImageRepository> _productImageRepositoryMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly UploadProductImageCommandHandler _handler;

    public UploadProductImageTest()
    {
        _productImageRepositoryMock = new Mock<IProductImageRepository>();
        _fileServiceMock = new Mock<IFileService>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _handler = new UploadProductImageCommandHandler(
            _productImageRepositoryMock.Object,
            _fileServiceMock.Object,
            _productRepositoryMock.Object,
            _dateTimeProviderMock.Object
        );
    }

    private UploadProductImageCommand CreateValidCommand()
    {
        return new UploadProductImageCommand()
        {
            ProductId = Guid.NewGuid(),
            Files = new List<FileUploadStreamData>()
            {
                new()
                {
                    FileName = "image1.jpg",
                    ContentType = "image/jpeg",
                    Stream = new MemoryStream(new byte[] {1, 2, 3})
                },
                new()
                {
                    FileName = "image2.jpg",
                    ContentType = "image/jpeg",
                    Stream = new MemoryStream(new byte[] {4, 5, 6})
                }
            }
        };
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = CreateValidCommand();

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns((Domain.Entities.Catalog.Product)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);

        _productImageRepositoryMock.Verify(
            x => x.Insert(It.IsAny<ProductImage>()),
            Times.Never);

        _productImageRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUploadImagesSuccessfully()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId,
            Name = "Yonex Astrox"
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _fileServiceMock
            .SetupSequence(x => x.UploadFileAsync(It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "uploaded-image-1",
                SecureUrl = new Uri("https://cdn.com/image1.jpg")
            })
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "uploaded-image-2",
                SecureUrl = new Uri("https://cdn.com/image2.jpg")
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count);

        Assert.Equal(
            "https://cdn.com/image1.jpg",
            result.Value[0].ImageUrl);

        Assert.Equal(
            "https://cdn.com/image2.jpg",
            result.Value[1].ImageUrl);

        _fileServiceMock.Verify(
            x => x.UploadFileAsync(It.IsAny<FileUploadStream>()),
            Times.Exactly(2));

        _productImageRepositoryMock.Verify(
            x => x.Insert(It.IsAny<ProductImage>()),
            Times.Exactly(2));

        _productImageRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_FirstImage_ShouldBePrimary()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId,
            Name = "Victor Thruster"
        };

        ProductImage? firstInsertedImage = null;
        ProductImage? secondInsertedImage = null;

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _fileServiceMock
            .Setup(x => x.UploadFileAsync(It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "uploaded",
                SecureUrl = new Uri("https://cdn.com/image.jpg")
            });

        var insertCallCount = 0;

        _productImageRepositoryMock
            .Setup(x => x.Insert(It.IsAny<ProductImage>()))
            .Callback<ProductImage>(image =>
            {
                insertCallCount++;

                if (insertCallCount == 1)
                    firstInsertedImage = image;

                if (insertCallCount == 2)
                    secondInsertedImage = image;
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(firstInsertedImage);
        Assert.True(firstInsertedImage!.IsPrimary);

        Assert.NotNull(secondInsertedImage);
        Assert.False(secondInsertedImage!.IsPrimary);
    }

    [Fact]
    public async Task Handle_ShouldSaveCorrectProductId()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId,
            Name = "Lining Aeronaut"
        };

        ProductImage? insertedImage = null;

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _fileServiceMock
            .Setup(x => x.UploadFileAsync(It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "uploaded",
                SecureUrl = new Uri("https://cdn.com/image.jpg")
            });

        _productImageRepositoryMock
            .Setup(x => x.Insert(It.IsAny<ProductImage>()))
            .Callback<ProductImage>(x => insertedImage = x);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(insertedImage);
        Assert.Equal(command.ProductId, insertedImage!.ProductId);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectResponseData()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId,
            Name = "Apacs Power"
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _fileServiceMock
            .Setup(x => x.UploadFileAsync(It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "metadata-test",
                SecureUrl = new Uri("https://cdn.com/test.jpg")
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        var response = result.Value.First();

        Assert.Equal("metadata-test", response.Metadata);
        Assert.Equal("https://cdn.com/test.jpg", response.ImageUrl);
        Assert.Equal(product.Name, response.ProductName);
        Assert.False(response.IsMainProfile);
    }
}