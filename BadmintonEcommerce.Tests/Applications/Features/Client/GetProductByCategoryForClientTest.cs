using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Client.GetProductsByCategory;
using BadmintonEcommerce.Contracts.API.Presentation;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Responses;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using BadmintonEcommerce.Tests.Helpers.Builders.Queries;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Client;

public class GetProductByCategoryForClientTest
{
    private readonly Mock<IProductCategoryRepository>
        _categoryRepositoryMock;

    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly GetProductsByCategoryQueryHandler
        _handler;

    public GetProductByCategoryForClientTest()
    {
        _categoryRepositoryMock =
            new Mock<IProductCategoryRepository>();

        _productRepositoryMock =
            new Mock<IProductRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetProductsByCategoryQueryHandler(
                _categoryRepositoryMock.Object,
                _productRepositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnAllProducts()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var query = new GetProductsByCategoryQueryBuilder()
            .WithPageNumber(1)
            .WithPageSize(10)
            .WithCategoryId(categoryId)
            .Build();

        var products = GetProductsByCategoryQueryBuilder
            .CreateProducts(5);

        var mappedProducts = GetProductsByCategoryQueryBuilder.
            CreateMappedResponses(5);

        _categoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Images,Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<
                    Domain.Entities.Catalog.Product>>()))
            .Returns(mappedProducts);

        // Act
        Result<PagedList<List<ProductResponse>>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.TotalCount.Should().Be(5);

        result.Value.TotalPages.Should().Be(1);

        result.Value.Data.Should()
            .BeEquivalentTo(mappedProducts);

        _productRepositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "Images,Category,Variants",
                null,
                null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryExists_ShouldReturnParentAndChildProducts()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        Guid childCategoryId = Guid.NewGuid();

        var query = new GetProductsByCategoryQueryBuilder()
            .WithPageNumber(1)
            .WithPageSize(10)
            .WithCategoryId(categoryId)
            .Build();

        var category =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = categoryId
            };

        var childCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = childCategoryId
            };

        category.ChildCategories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                childCategory
            };

        var categories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                category
            };

        var childProducts = GetProductsByCategoryQueryBuilder.CreateProducts(2);

        var parentProducts = GetProductsByCategoryQueryBuilder.CreateProducts(3);

        var allProducts =
            childProducts.Concat(parentProducts).ToList();

        var mappedResponses =
            GetProductsByCategoryQueryBuilder.CreateMappedResponses(5);

        _categoryRepositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        _categoryRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                null,
                "ChildCategories",
                null,
                null))
            .ReturnsAsync(categories);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.Product,
                    bool>>>(),
                null,
                "Images,Category,Variants",
                null,
                null))
            .ReturnsAsync(parentProducts);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<
                    Domain.Entities.Catalog.Product>>()))
            .Returns(mappedResponses);

        // Act
        Result<PagedList<List<ProductResponse>>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.TotalCount.Should().BeGreaterThan(0);

        result.Value.Data.Should()
            .BeEquivalentTo(mappedResponses);

        _categoryRepositoryMock.Verify(
            x => x.GetById(categoryId),
            Times.Once);

        _categoryRepositoryMock.Verify(
            x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                null,
                "ChildCategories",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<ProductResponse>>(
                It.IsAny<List<
                    Domain.Entities.Catalog.Product>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidPageValues_ShouldUseDefaultPagination()
    {
        // Arrange
        var query = new GetProductsByCategoryQueryBuilder()
            .WithPageNumber(0)
            .WithPageSize(0)
            .WithCategoryId(Guid.NewGuid())
            .Build();

        var products = GetProductsByCategoryQueryBuilder.CreateProducts(15);

        var mappedResponses =
            GetProductsByCategoryQueryBuilder.CreateMappedResponses(9);

        _categoryRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Images,Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<
                    Domain.Entities.Catalog.Product>>()))
            .Returns(mappedResponses);

        // Act
        Result<PagedList<List<ProductResponse>>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.TotalCount.Should().Be(15);

        result.Value.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldApplyPaginationCorrectly()
    {
        // Arrange
        var query = new GetProductsByCategoryQueryBuilder()
            .WithPageNumber(2)
            .WithPageSize(5)
            .WithCategoryId(Guid.NewGuid())
            .Build();

        var products = GetProductsByCategoryQueryBuilder.CreateProducts(12);

        var mappedResponses = GetProductsByCategoryQueryBuilder
            .CreateMappedResponses(5);

        _categoryRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        _productRepositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "Images,Category,Variants",
                null,
                null))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductResponse>>(
                It.IsAny<List<
                    Domain.Entities.Catalog.Product>>()))
            .Returns(mappedResponses);

        // Act
        Result<PagedList<List<ProductResponse>>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.TotalCount.Should().Be(12);

        result.Value.TotalPages.Should().Be(3);

        result.Value.Data.Count.Should().Be(5);
    }
}