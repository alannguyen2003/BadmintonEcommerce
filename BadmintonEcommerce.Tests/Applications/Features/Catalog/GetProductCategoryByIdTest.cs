using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.ProductCategory.GetById;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class GetProductCategoryByIdTest
{
    private readonly Mock<IProductCategoryRepository>
        _repositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly GetProductCategoryByIdQueryHandler
        _handler;

    public GetProductCategoryByIdTest()
    {
        _repositoryMock =
            new Mock<IProductCategoryRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _handler =
            new GetProductCategoryByIdQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var query = new GetProductCategoryByIdQuery(categoryId);

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        // Act
        Result<ProductCategoryByIdResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductCategoryError.NotFound(categoryId));

        _repositoryMock.Verify(
            x => x.GetById(categoryId),
            Times.Once);

        _repositoryMock.Verify(
            x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                It.IsAny<Func<IQueryable<
                        Domain.Entities.Catalog.ProductCategory>,
                    IOrderedQueryable<
                        Domain.Entities.Catalog.ProductCategory>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()),
            Times.Never);

        _mapperMock.Verify(
            x => x.Map<ProductCategoryByIdResponse>(
                It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryExists_ShouldReturnMappedResponse()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var query = new GetProductCategoryByIdQuery(categoryId);
        
        var category = new ProductCategoryBuilder()
            .WithId(categoryId)
            .WithName("Racquets")
            .WithLevel(1)
            .Build();

        var categories =
            new List<Domain.Entities.Catalog.ProductCategory>
            {
                category
            };

        var mappedResponse =
            new ProductCategoryByIdResponse
            {
                CategoryName = "Shoes"
            };

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

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
            .Setup(x => x.Map<ProductCategoryByIdResponse>(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()))
            .Returns(mappedResponse);

        // Act
        Result<ProductCategoryByIdResponse> result =
            await _handler.Handle(
                query,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should()
            .BeEquivalentTo(mappedResponse);

        _repositoryMock.Verify(
            x => x.GetById(categoryId),
            Times.Once);

        _repositoryMock.Verify(
            x => x.Get(
                It.IsAny<Expression<Func<
                    Domain.Entities.Catalog.ProductCategory,
                    bool>>>(),
                null,
                "ChildCategories,Products",
                null,
                null),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<ProductCategoryByIdResponse>(
                It.Is<Domain.Entities.Catalog.ProductCategory>(
                    c => c.Id == categoryId)),
            Times.Once);
    }
}