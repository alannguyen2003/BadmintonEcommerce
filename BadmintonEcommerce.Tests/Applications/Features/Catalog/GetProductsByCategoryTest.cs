using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.GetByCategory;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Queries;
using FluentAssertions;
using Moq;
using SharedKernel.Errors;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class GetProductsByCategoryTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetProductsByCategoryQueryHandler _handler;

    public GetProductsByCategoryTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetProductsByCategoryQueryHandler(
            _productRepositoryMock.Object,
            _productCategoryRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns((ProductCategory?)null);

        var query = new GetProductsByCategoryIdQueryBuilder()
            .WithProductCategoryId(categoryId)
            .Build();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBe(Error.None);

        _productRepositoryMock.Verify(
            x => x.Get(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                null,
                "Category,Variants",
                null,
                null),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryExists_ShouldReturnMappedProducts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var category = new ProductCategory()
        {
            Id = categoryId,
            CategoryName = "Rackets"
        };

        var products = new List<Product>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Yonex Astrox 100ZZ",
                CategoryId = categoryId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Victor Thruster",
                CategoryId = categoryId
            }
        };

        var mappedResponses = new List<ProductResponse>()
        {
            new()
            {
                ProductName = "Yonex Astrox 100ZZ"
            },
            new()
            {
                ProductName = "Victor Thruster"
            }
        };

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                null,
                "Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(mappedResponses);

        var query = new GetProductsByCategoryIdQueryBuilder()
            .WithProductCategoryId(categoryId)
            .Build();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().HaveCount(2);

        result.Value[0].ProductName
            .Should()
            .Be("Yonex Astrox 100ZZ");

        result.Value[1].ProductName
            .Should()
            .Be("Victor Thruster");

        _productCategoryRepositoryMock.Verify(
            x => x.GetById(categoryId),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Get(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                null,
                "Category,Variants",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryExistsButNoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var category = new ProductCategory()
        {
            Id = categoryId,
            CategoryName = "Shoes"
        };

        var products = new List<Product>();

        var mappedResponses = new List<ProductResponse>();

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                null,
                "Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(mappedResponses);

        var query = new GetProductsByCategoryIdQueryBuilder()
            .WithProductCategoryId(categoryId)
            .Build();

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();

        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallMapperWithCorrectProducts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var category = new ProductCategory()
        {
            Id = categoryId
        };

        var products = new List<Product>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Lining Aeronaut"
            }
        };

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                null,
                "Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns(new List<ProductResponse>());

        var query = new GetProductsByCategoryIdQueryBuilder()
            .WithProductCategoryId(categoryId)
            .Build();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<List<ProductResponse>>(
                It.Is<List<Product>>(p =>
                    p.Count == 1 &&
                    p[0].Name == "Lining Aeronaut")),
            Times.Once);
    }
}