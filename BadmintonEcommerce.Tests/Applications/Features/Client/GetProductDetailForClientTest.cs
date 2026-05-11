using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Client.GetProductDetail;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Queries;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Client;

public class GetProductDetailForClientTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductImageRepository>
        _productImageRepositoryMock;

    private readonly Mock<IProductOptionRepository>
        _productOptionRepositoryMock;

    private readonly Mock<IProductVariantRepository>
        _productVariantRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly GetProductDetailQueryHandler
        _handler;

    public GetProductDetailForClientTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productImageRepositoryMock =
            new Mock<IProductImageRepository>();

        _productOptionRepositoryMock =
            new Mock<IProductOptionRepository>();

        _productVariantRepositoryMock =
            new Mock<IProductVariantRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetProductDetailQueryHandler(
                _productRepositoryMock.Object,
                _productImageRepositoryMock.Object,
                _productOptionRepositoryMock.Object,
                _productVariantRepositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        

        var query = new GetProductDetailQueryBuilder()
            .WithProductId(productId)
            .Build();

        _productRepositoryMock
            .Setup(x => x.GetById(productId))
            .Returns((Domain.Entities.Catalog.Product?)null);

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductError.NotFound(productId));

        _mapperMock.Verify(
            x => x.Map<ProductDetailResponse>(
                It.IsAny<Domain.Entities.Catalog.Product>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldReturnProductDetail()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid optionId = Guid.NewGuid();

        Guid optionValueId = Guid.NewGuid();

        Guid variantId = Guid.NewGuid();
        
        
        var query = new GetProductDetailQueryBuilder()
            .WithProductId(productId)
            .Build();

        var product =
            new Domain.Entities.Catalog.Product
            {
                Id = productId,
                Name = "Yonex Shoes",
                Variants =
                    new List<
                        Domain.Entities.Catalog.ProductVariant>
                    {
                        new()
                        {
                            Id = variantId
                        }
                    }
            };

        var products =
            new List<Domain.Entities.Catalog.Product>
            {
                product
            };

        var mappedResponse =
            new ProductDetailResponse
            {
                Id = productId,
                Name = "Yonex Shoes",
                Options =
                    new List<ProductOptionResponse>
                    {
                        new()
                        {
                            Id = optionId,
                            Name = "Size"
                        }
                    },
                Variants =
                    new List<ProductVariantResponse>()
            };

        var option =
            new Domain.Entities.Catalog.ProductOption
            {
                Id = optionId,
                OptionValues =
                    new List<
                        Domain.Entities.Catalog.ProductOptionValue>
                    {
                        new()
                        {
                            Id = optionValueId,
                            Value = "42"
                        }
                    }
            };

        var variant =
            new Domain.Entities.Catalog.ProductVariant
            {
                Id = variantId,
                SKU = "SKU-001",
                Price = 100,
                InventoryItem =
                    new Domain.Entities.Inventory.InventoryItem
                    {
                        Quantity = 10
                    },
                Combinations =
                    new List<
                        Domain.Entities.Catalog.VariantCombination>
                    {
                        new()
                        {
                            OptionValueId = optionValueId
                        }
                    }
            };

        _productRepositoryMock
            .Setup(x => x.GetById(productId))
            .Returns(product);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<
                        Domain.Entities.Catalog.Product,
                        bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<ProductDetailResponse>(
                product))
            .Returns(mappedResponse);

        _productOptionRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<
                        Domain.Entities.Catalog.ProductOption,
                        bool>>>(),
                null,
                "OptionValues",
                null,
                null))
            .ReturnsAsync(new List<
                Domain.Entities.Catalog.ProductOption>
            {
                option
            });

        _productVariantRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<
                        Domain.Entities.Catalog.ProductVariant,
                        bool>>>(),
                null,
                "Combinations,InventoryItem",
                null,
                null))
            .ReturnsAsync(new List<
                Domain.Entities.Catalog.ProductVariant>
            {
                variant
            });

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ProductDetailResponse response =
            result.Value;

        response.Name.Should()
            .Be("Yonex Shoes");

        response.Options.Count.Should().Be(1);

        response.Options.First()
            .Values.Count.Should().Be(1);

        response.Options.First()
            .Values.First().Value.Should().Be("42");

        response.Variants.Count.Should().Be(1);

        response.Variants.First().SKU
            .Should().Be("SKU-001");

        response.Variants.First().Price
            .Should().Be(100);

        response.Variants.First().IsAvailable
            .Should().BeTrue();

        response.Variants.First()
            .OptionValues.Should()
            .Contain(optionValueId);

        _mapperMock.Verify(
            x => x.Map<ProductDetailResponse>(
                product),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InventoryQuantityZero_ShouldSetUnavailable()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid variantId = Guid.NewGuid();
        
        var query = new GetProductDetailQueryBuilder()
            .WithProductId(productId)
            .Build();

        var product =
            new Domain.Entities.Catalog.Product
            {
                Id = productId,
                Variants =
                    new List<
                        Domain.Entities.Catalog.ProductVariant>
                    {
                        new()
                        {
                            Id = variantId
                        }
                    }
            };

        var mappedResponse =
            new ProductDetailResponse
            {
                Variants =
                    new List<ProductVariantResponse>(),
                Options =
                    new List<ProductOptionResponse>()
            };

        var variant =
            new Domain.Entities.Catalog.ProductVariant
            {
                Id = variantId,
                InventoryItem =
                    new Domain.Entities.Inventory.InventoryItem
                    {
                        Quantity = 0
                    },
                Combinations =
                    new List<Domain.Entities.Catalog.VariantCombination>()
            };

        _productRepositoryMock
            .Setup(x => x.GetById(productId))
            .Returns(product);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<
                        Domain.Entities.Catalog.Product,
                        bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<
                Domain.Entities.Catalog.Product>
            {
                product
            });

        _mapperMock
            .Setup(x => x.Map<ProductDetailResponse>(
                product))
            .Returns(mappedResponse);

        _productVariantRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<
                        Domain.Entities.Catalog.ProductVariant,
                        bool>>>(),
                null,
                "Combinations,InventoryItem",
                null,
                null))
            .ReturnsAsync(new List<
                Domain.Entities.Catalog.ProductVariant>
            {
                variant
            });

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Variants.First()
            .IsAvailable.Should().BeFalse();
    }
}