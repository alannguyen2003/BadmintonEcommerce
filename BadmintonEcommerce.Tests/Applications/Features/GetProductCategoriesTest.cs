using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.ProductCategory.Get;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features;

public class GetProductCategoriesTest
{
    private readonly Mock<IMapper> _mapperMock;

    private readonly Mock<IProductCategoryRepository> _repositoryMock;

    private readonly GetProductCategoriesQueryHandler _handler;

    public GetProductCategoriesTest()
    {
        _mapperMock = new Mock<IMapper>();

        _repositoryMock = new Mock<IProductCategoryRepository>();

        _handler = new GetProductCategoriesQueryHandler(
            _mapperMock.Object,
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CategoriesExist_ShouldReturnMappedResponses()
    {
        // Arrange
        var query = new GetProductCategoriesQuery();

        var categories = new List<Domain.Entities.Catalog.ProductCategory>
        {
            new ProductCategoryBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Shoes")
                .Build(),
            new ProductCategoryBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Racquets")
                .Build()
        };

        var mappedResponses = new List<ProductCategoryResponse>
        {
            new ProductCategoryBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Shoes")
                .ResponseBuild(),
            new ProductCategoryBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Racquets")
                .ResponseBuild()
        };

        _repositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                It.IsAny<Func<IQueryable<
                        Domain.Entities.Catalog.ProductCategory>,
                    IOrderedQueryable<
                        Domain.Entities.Catalog.ProductCategory>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(categories);

        _mapperMock
            .Setup(x => x.Map<List<ProductCategoryResponse>>(
                It.IsAny<List<Domain.Entities.Catalog.ProductCategory>>()))
            .Returns(mappedResponses);

        // Act
        Result<List<ProductCategoryResponse>> result =
            await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().HaveCount(2);

        result.Value.Should().BeEquivalentTo(mappedResponses);

        _repositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "ParentCategory",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<ProductCategoryResponse>>(
                It.Is<List<Domain.Entities.Catalog.ProductCategory>>(
                    list => list.Count == 2)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NoCategories_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetProductCategoriesQuery();

        var categories =
            new List<Domain.Entities.Catalog.ProductCategory>();

        var mappedResponses = new List<ProductCategoryResponse>();

        _repositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                It.IsAny<Func<IQueryable<
                        Domain.Entities.Catalog.ProductCategory>,
                    IOrderedQueryable<
                        Domain.Entities.Catalog.ProductCategory>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(categories);

        _mapperMock
            .Setup(x => x.Map<List<ProductCategoryResponse>>(
                It.IsAny<List<Domain.Entities.Catalog.ProductCategory>>()))
            .Returns(mappedResponses);

        // Act
        Result<List<ProductCategoryResponse>> result =
            await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeEmpty();

        _repositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "ParentCategory",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<ProductCategoryResponse>>(
                It.Is<List<Domain.Entities.Catalog.ProductCategory>>(
                    list => !list.Any())),
            Times.Once);
    }
}