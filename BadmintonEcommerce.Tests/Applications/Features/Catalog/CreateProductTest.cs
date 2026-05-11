using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.Create;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class CreateProductTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductCategoryRepository>
        _productCategoryRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly CreateProductCommandHandler
        _handler;

    public CreateProductTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productCategoryRepositoryMock =
            new Mock<IProductCategoryRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _handler =
            new CreateProductCommandHandler(
                _productRepositoryMock.Object,
                _productCategoryRepositoryMock.Object,
                _mapperMock.Object,
                _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var command = new CreateProductCommand
        {
            ProductName = "Yonex Shoes",
            Description = "Professional badminton shoes",
            Brand = "Yonex",
            CategoryId = categoryId
        };

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns((ProductCategory?)null);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductCategoryError.NotFound(categoryId));

        _productRepositoryMock.Verify(
            x => x.Insert(It.IsAny<Product>()),
            Times.Never);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProductSuccessfully()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        DateTime utcNow = DateTime.UtcNow;

        var command = new CreateProductCommand
        {
            ProductName = "Yonex Shoes",
            Description = "Professional badminton shoes",
            Brand = "Yonex",
            CategoryId = categoryId
        };

        var category = new ProductCategory
        {
            Id = categoryId,
            CategoryName = "Shoes"
        };

        var mappedProduct = new ProductBuilder()
            .WithId(Guid.NewGuid())
            .WithName(command.ProductName)
            .WithDescription(command.Description)
            .WithBrand(command.Brand)
            .WithCategoryId(categoryId)
            .Build();

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _mapperMock
            .Setup(x => x.Map<Product>(command))
            .Returns(mappedProduct);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().Be(mappedProduct.Id);

        mappedProduct.CreatedOnUtc
            .Should().Be(utcNow);

        _mapperMock.Verify(
            x => x.Map<Product>(command),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Insert(
                It.Is<Product>(p =>
                    p.Name == command.ProductName &&
                    p.Description == command.Description &&
                    p.Brand == command.Brand &&
                    p.CategoryId == command.CategoryId &&
                    p.CreatedOnUtc == utcNow)),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSetCreatedOnUtc()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        DateTime utcNow = new DateTime(
            2026,
            5,
            11,
            10,
            0,
            0,
            DateTimeKind.Utc);

        var command = new CreateProductCommand
        {
            ProductName = "Victor Racket",
            Description = "Professional racket",
            Brand = "Victor",
            CategoryId = categoryId
        };

        var category = new ProductCategory
        {
            Id = categoryId
        };

        var mappedProduct = new Product
        {
            Id = Guid.NewGuid()
        };

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _mapperMock
            .Setup(x => x.Map<Product>(command))
            .Returns(mappedProduct);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        mappedProduct.CreatedOnUtc
            .Should().Be(utcNow);
    }
}