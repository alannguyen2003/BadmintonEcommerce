using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.Get;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using FluentAssertions;
using Moq;
using GetProductsQueryHandler = BadmintonEcommerce.Application.Features.Product.Get.GetProductsQueryHandler;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class GetProductsTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductVariantRepository>
        _productVariantRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly Application.Features.Product.Get.GetProductsQueryHandler
        _handler;

    public GetProductsTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productVariantRepositoryMock =
            new Mock<IProductVariantRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetProductsQueryHandler(
                _productRepositoryMock.Object,
                _productVariantRepositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedProducts()
    {
        // Arrange
        var products = CreateProducts();

        var responses = CreateResponses();

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()))
            .Returns(responses);

        var query = new Application.Features.Product.Get.GetProductsQuery();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().HaveCount(2);

        _mapperMock.Verify(
            x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProductHasPrimaryImage_ShouldSetPrimaryImage()
    {
        // Arrange
        var products = CreateProducts();

        var responses = CreateResponses();

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()))
            .Returns(responses);

        var query = new GetProductsQuery();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.Value[0].PrimaryImage
            .Should().NotBeNull();

        result.Value[0].PrimaryImage!.Url
            .Should().Be(
                "https://cdn.com/image1.jpg");
    }

    [Fact]
    public async Task Handle_ProductDoesNotHavePrimaryImage_ShouldSetNull()
    {
        // Arrange
        var products = CreateProductsWithoutPrimary();

        var responses = CreateResponses();

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()))
            .Returns(responses);

        var query = new GetProductsQuery();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.Value[0].PrimaryImage
            .Should().BeNull();
    }

    [Fact]
    public async Task Handle_EmptyProducts_ShouldReturnEmptyList()
    {
        // Arrange
        var products = new List<Product>();

        var responses = new List<ProductResponse>();

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()))
            .Returns(responses);

        var query = new GetProductsQuery();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
        // Arrange
        var products = CreateProducts();

        var responses = CreateResponses();

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<Product>>()))
            .Returns(responses);

        var query = new GetProductsQuery();

        // Act
        await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null),
            Times.Once);
    }

    private static List<Product> CreateProducts()
    {
        return
        [
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Yonex Astrox",
                Images =
                [
                    new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://cdn.com/image1.jpg",
                        IsPrimary = true
                    },
                    new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://cdn.com/image2.jpg",
                        IsPrimary = false
                    }
                ]
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Victor Thruster",
                Images =
                [
                    new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://cdn.com/image3.jpg",
                        IsPrimary = true
                    }
                ]
            }
        ];
    }

    private static List<Product>
        CreateProductsWithoutPrimary()
    {
        return
        [
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Yonex Astrox",
                Images =
                [
                    new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://cdn.com/image1.jpg",
                        IsPrimary = false
                    }
                ]
            }
        ];
    }

    private static List<ProductResponse>
        CreateResponses()
    {
        return
        [
            new ProductResponse
            {
                Id = Guid.NewGuid(),
                ProductName = "Yonex Astrox"
            },
            new ProductResponse
            {
                Id = Guid.NewGuid(),
                ProductName = "Victor Thruster"
            }
        ];
    }
}