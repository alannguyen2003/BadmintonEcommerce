using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.Update;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using Moq;
using SharedKernel.Services;
using SharedKernel.Utils;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class UpdateProductTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _handler = new UpdateProductCommandHandler(
            _productRepositoryMock.Object,
            _productCategoryRepositoryMock.Object,
            _mapperMock.Object,
            _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            ProductName = "Yonex Astrox 99",
            ProductDescription = "Description",
            Brand = "Yonex",
            CategoryId = Guid.NewGuid()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns((Product)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);

        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Never);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Old Product"
        };

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            ProductName = "New Product",
            ProductDescription = "Description",
            Brand = "Yonex",
            CategoryId = Guid.NewGuid()
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.CategoryId))
            .Returns((ProductCategory)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);

        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Never);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateProductSuccessfully()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description",
            Brand = "Old Brand",
            CategoryId = Guid.NewGuid(),
            Slug = "old-name"
        };

        var category = new ProductCategory
        {
            Id = categoryId,
            CategoryName = "Rackets"
        };

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            ProductName = "Yonex Astrox 100ZZ",
            ProductDescription = "Professional racket",
            Brand = "Yonex",
            CategoryId = categoryId
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.CategoryId))
            .Returns(category);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(now);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(command.ProductName, product.Name);
        Assert.Equal(command.ProductDescription, product.Description);
        Assert.Equal(command.Brand, product.Brand);
        Assert.Equal(command.CategoryId, product.CategoryId);
        Assert.Equal(now, product.LastModifiedOnUtc);

        Assert.Equal(
            SlugGenerateProvider.GenerateSlug(command.ProductName),
            product.Slug);

        _productRepositoryMock.Verify(
            x => x.Update(product),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldGenerateNewSlug()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Old Product",
            Slug = "old-product"
        };

        var category = new ProductCategory
        {
            Id = Guid.NewGuid()
        };

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            ProductName = "Yonex NanoFlare 800 Pro",
            ProductDescription = "Description",
            Brand = "Yonex",
            CategoryId = category.Id
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.CategoryId))
            .Returns(category);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(
            "yonex-nanoflare-800-pro",
            product.Slug);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallUpdateAndSaveChanges()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid()
        };

        var category = new ProductCategory
        {
            Id = Guid.NewGuid()
        };

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            ProductName = "Product",
            ProductDescription = "Description",
            Brand = "Brand",
            CategoryId = category.Id
        };

        _productRepositoryMock
            .Setup(x => x.GetById(command.Id))
            .Returns(product);

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.CategoryId))
            .Returns(category);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}