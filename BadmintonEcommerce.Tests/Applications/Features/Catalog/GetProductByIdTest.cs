using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.GetById;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class GetProductByIdTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IVariantCombinationRepository>
        _variantCombinationRepositoryMock;

    private readonly Mock<IProductOptionRepository>
        _productOptionRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly GetProductByIdQueryHandler
        _handler;

    public GetProductByIdTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _variantCombinationRepositoryMock =
            new Mock<IVariantCombinationRepository>();

        _productOptionRepositoryMock =
            new Mock<IProductOptionRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetProductByIdQueryHandler(
                _productRepositoryMock.Object,
                _variantCombinationRepositoryMock.Object,
                _productOptionRepositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        var query = new GetProductByIdQuery(productId);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<Product, bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<Product>());

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductError.NotFound(productId));
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldReturnProductDetailResponse()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid variantId = Guid.NewGuid();

        Guid optionId = Guid.NewGuid();

        Guid optionValueId = Guid.NewGuid();

        var query = new GetProductByIdQuery(productId);

        var product = new Product
        {
            Id = productId,
            Name = "Yonex Shoes",
            Description = "Professional badminton shoes",
            Brand = "Yonex",
            Status = true,
            CategoryId = Guid.NewGuid(),

            Images = new List<ProductImage>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Url = "https://image.com/shoes.jpg"
                }
            },

            Variants = new List<ProductVariant>
            {
                new()
                {
                    Id = variantId,
                    SKU = "SKU-001",
                    Price = 100
                }
            },

            Options = new List<ProductOption>
            {
                new()
                {
                    Id = optionId,
                    OptionName = "Size"
                }
            }
        };

        var variantCombinations =
            new List<VariantCombination>
            {
                new()
                {
                    VariantId = variantId,
                    OptionValueId = optionValueId
                }
            };

        var option = new ProductOption
        {
            Id = optionId,

            OptionValues =
            [
                new ProductOptionValue
                {
                    Id = optionValueId,
                    Value = "42"
                }
            ]
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<Product, bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<Product>
            {
                product
            });

        _variantCombinationRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<VariantCombination, bool>>>(),
                null,
                "",
                null,
                null))
            .ReturnsAsync(variantCombinations);

        _productOptionRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<ProductOption, bool>>>(),
                null,
                "OptionValues",
                null,
                null))
            .ReturnsAsync(new List<ProductOption>
            {
                option
            });

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ProductDetailResponse response = result.Value;

        response.Should().NotBeNull();

        response.Id.Should().Be(productId);

        response.Name.Should().Be("Yonex Shoes");

        response.Description.Should()
            .Be("Professional badminton shoes");

        response.Brand.Should().Be("Yonex");

        response.Images.Should().HaveCount(1);

        response.Images.First().ImageUrl
            .Should().Be("https://image.com/shoes.jpg");

        response.Variants.Should().HaveCount(1);

        response.Variants.First().SKU
            .Should().Be("SKU-001");

        response.Variants.First().Price
            .Should().Be(100);

        response.Variants.First().OptionValues
            .Should().Contain(optionValueId);

        response.Options.Should().HaveCount(1);

        response.Options.First().Name
            .Should().Be("Size");

        response.Options.First().Values
            .Should().HaveCount(1);

        response.Options.First().Values.First().Value
            .Should().Be("42");
    }

    [Fact]
    public async Task Handle_ProductWithoutVariants_ShouldReturnEmptyVariants()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        var query = new GetProductByIdQuery(productId);

        var product = new Product
        {
            Id = productId,
            Name = "Yonex Shirt",

            Images = new List<ProductImage>(),

            Variants = new List<ProductVariant>(),

            Options = new List<ProductOption>()
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<Product, bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<Product>
            {
                product
            });

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Variants.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ProductWithoutOptions_ShouldReturnEmptyOptions()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        var query = new GetProductByIdQuery(productId);

        var product = new Product
        {
            Id = productId,
            Name = "Victor Bag",

            Images = new List<ProductImage>(),

            Variants = new List<ProductVariant>(),

            Options = new List<ProductOption>()
        };

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<Product, bool>>>(),
                null,
                "Images,Category,Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<Product>
            {
                product
            });

        // Act
        Result<ProductDetailResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Options.Should().BeEmpty();
    }
}