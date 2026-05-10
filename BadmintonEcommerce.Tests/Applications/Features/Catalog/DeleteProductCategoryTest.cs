using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.ProductCategory.Delete;
using BadmintonEcommerce.Domain.Errors;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class DeleteProductCategoryTest
{
    private readonly Mock<IProductCategoryRepository> _repositoryMock;

    private readonly DeleteProductCategoryCommandHandler _handler;

    public DeleteProductCategoryTest()
    {
        _repositoryMock = new Mock<IProductCategoryRepository>();

        _handler = new DeleteProductCategoryCommandHandler(
                _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var command = new DeleteProductCategoryCommand(categoryId);

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductCategoryError.NotFound(categoryId));

        _repositoryMock.Verify(
            x => x.Delete(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryExists_ShouldDeleteSuccessfully()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var category =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = categoryId,
                CategoryName = "Badminton Shoes",
                Level = 1
            };

        var command = new DeleteProductCategoryCommand(categoryId);

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(category);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.GetById(categoryId),
            Times.Once);

        _repositoryMock.Verify(
            x => x.Delete(category),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}