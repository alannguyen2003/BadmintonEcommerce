using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.Get;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using FluentAssertions;
using Moq;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class GetProductsTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductVariantRepository> _productVariantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetProductsQueryHandler _handler;

    public GetProductsTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productVariantRepositoryMock = new Mock<IProductVariantRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetProductsQueryHandler(
            _productRepositoryMock.Object,
            _productVariantRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Yonex Astrox 100ZZ",
                Images = new List<ProductImage>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Url = "primary-image.jpg",
                        IsPrimary = true
                    }
                }
            }
        };

        var mappedResponses = new List<ProductResponse>
        {
            new()
            {
                ProductName = "Yonex Astrox 100ZZ"
            }
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(mappedResponses);

        // Act
        var result = await _handler.Handle(
            new GetProductsQuery(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().HaveCount(1);

        result.Value[0].ProductName
            .Should()
            .Be("Yonex Astrox 100ZZ");

        _productRepositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProductHasPrimaryImage_ShouldMapPrimaryImage()
    {
        // Arrange
        var imageId = Guid.NewGuid();

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Victor Thruster",
                Images = new List<ProductImage>()
                {
                    new()
                    {
                        Id = imageId,
                        Url = "victor-primary.jpg",
                        IsPrimary = true
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Url = "victor-secondary.jpg",
                        IsPrimary = false
                    }
                }
            }
        };

        var responses = new List<ProductResponse>
        {
            new()
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(responses);

        // Act
        var result = await _handler.Handle(
            new GetProductsQuery(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value[0].PrimaryImage.Should().NotBeNull();

        result.Value[0].PrimaryImage!.Id
            .Should()
            .Be(imageId);

        result.Value[0].PrimaryImage.Url
            .Should()
            .Be("victor-primary.jpg");
    }

    [Fact]
    public async Task Handle_ProductWithoutPrimaryImage_ShouldSetPrimaryImageNull()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Lining Aeronaut",
                Images = new List<ProductImage>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Url = "image.jpg",
                        IsPrimary = false
                    }
                }
            }
        };

        var responses = new List<ProductResponse>
        {
            new()
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(responses);

        // Act
        var result = await _handler.Handle(
            new GetProductsQuery(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value[0].PrimaryImage
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task Handle_NoProducts_ShouldReturnEmptyList()
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
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(responses);

        // Act
        var result = await _handler.Handle(
            new GetProductsQuery(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_MultipleProducts_ShouldMapAllPrimaryImagesCorrectly()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                Name = "Product 1",
                Images = new List<ProductImage>()
                {
                    new()
                    {
                        Url = "product1.jpg",
                        IsPrimary = true
                    }
                }
            },
            new()
            {
                Name = "Product 2",
                Images = new List<ProductImage>()
                {
                    new()
                    {
                        Url = "product2.jpg",
                        IsPrimary = true
                    }
                }
            }
        };

        var responses = new List<ProductResponse>
        {
            new(),
            new()
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Variants,Images,Category",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(responses);

        // Act
        var result = await _handler.Handle(
            new GetProductsQuery(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().HaveCount(2);

        result.Value[0].PrimaryImage!.Url
            .Should()
            .Be("product1.jpg");

        result.Value[1].PrimaryImage!.Url
            .Should()
            .Be("product2.jpg");
    }
}