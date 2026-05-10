using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Client.GetCategories;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Client;

public class GetCategoriesTest
{
    private readonly Mock<IProductCategoryRepository>
        _repositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly GetCategoriesQueryHandler
        _handler;

    public GetCategoriesTest()
    {
        _repositoryMock =
            new Mock<IProductCategoryRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetCategoriesQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_NoCategories_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetCategoriesQuery();

        _repositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "ChildCategories,ParentCategory",
                null,
                null))
            .ReturnsAsync(
                new List<
                    Domain.Entities.Catalog.ProductCategory>());

        // Act
        Result<List<CategoryResponse>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().BeEmpty();

        _repositoryMock.Verify(
            x => x.Get(
                null,
                null,
                "ChildCategories,ParentCategory",
                null,
                null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_RootCategoriesOnly_ShouldReturnRootCategories()
    {
        // Arrange
        var query = new GetCategoriesQuery();

        var categories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                new ProductCategoryBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Racquets")
                    .WithParentCategoryId(null)
                    .Build(),
                new ProductCategoryBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Shoes")
                    .WithParentCategoryId(null)
                    .Build()
            };

        _repositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "ChildCategories,ParentCategory",
                null,
                null))
            .ReturnsAsync(categories);

        // Act
        Result<List<CategoryResponse>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Count.Should().Be(2);

        result.Value.Should()
            .Contain(x => x.Name == "Shoes");

        result.Value.Should()
            .Contain(x => x.Name == "Racquets");

        result.Value.All(x =>
                x.ChildCategories.Count == 0)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ParentChildCategories_ShouldBuildHierarchy()
    {
        // Arrange
        var query = new GetCategoriesQuery();

        Guid parentId = Guid.NewGuid();

        var categories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                new ProductCategoryBuilder()
                    .WithId(parentId)
                    .WithName("Badminton")
                    .WithParentCategoryId(null)
                    .Build(),
                new ProductCategoryBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Racquest")
                    .WithParentCategoryId(parentId)
                    .Build()
            };

        _repositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "ChildCategories,ParentCategory",
                null,
                null))
            .ReturnsAsync(categories);

        // Act
        Result<List<CategoryResponse>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Count.Should().Be(1);

        CategoryResponse root =
            result.Value.First();

        root.Name.Should().Be("Badminton");

        root.ChildCategories.Count.Should().Be(1);

        root.ChildCategories.First().Name
            .Should().Be("Racquest");
    }

    [Fact]
    public async Task Handle_MultipleLevels_ShouldBuildNestedTree()
    {
        // Arrange
        var query = new GetCategoriesQuery();

        Guid rootId = Guid.NewGuid();

        Guid childId = Guid.NewGuid();

        var categories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                new ProductCategoryBuilder()
                    .WithId(rootId)
                    .WithName("Badminton")
                    .WithParentCategoryId(null)
                    .Build(),
                new ProductCategoryBuilder() 
                    .WithId(childId)
                    .WithName("Racquets")
                    .WithParentCategoryId(rootId)
                    .Build(),
                new ProductCategoryBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Astrox")
                    .WithParentCategoryId(childId)
                    .Build()
            };

        _repositoryMock
            .Setup(x => x.Get(
                null,
                null,
                "ChildCategories,ParentCategory",
                null,
                null))
            .ReturnsAsync(categories);

        // Act
        Result<List<CategoryResponse>> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Count.Should().Be(1);

        var root = result.Value.First();

        root.Name.Should().Be("Badminton");

        root.ChildCategories.Count.Should().Be(1);

        var child = root.ChildCategories.First();

        child.Name.Should().Be("Racquets");

        child.ChildCategories.Count.Should().Be(1);

        child.ChildCategories.First().Name
            .Should().Be("Astrox");
    }

    [Fact]
    public void CategoryRecursion_ShouldReturnCorrectHierarchy()
    {
        // Arrange
        Guid parentId = Guid.NewGuid();

        var categories =
            new List<
                Domain.Entities.Catalog.ProductCategory>
            {
                new()
                {
                    Id = parentId,
                    CategoryName = "Root",
                    ParentCategoryId = null
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "Child",
                    ParentCategoryId = parentId
                }
            };

        // Act
        List<CategoryResponse> result =
            _handler.CategoryRecursion(
                categories,
                null);

        // Assert
        result.Count.Should().Be(1);

        result.First().Name.Should().Be("Root");

        result.First().ChildCategories
            .Count.Should().Be(1);

        result.First().ChildCategories
            .First().Name.Should().Be("Child");
    }
}