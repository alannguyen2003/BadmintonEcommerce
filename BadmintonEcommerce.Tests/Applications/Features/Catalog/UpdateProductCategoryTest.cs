using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.ProductCategory.Update;
using BadmintonEcommerce.Domain.Errors;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class UpdateProductCategoryTest
{
    private readonly Mock<IProductCategoryRepository>
        _repositoryMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly UpdateProductCategoryCommandHandler
        _handler;

    public UpdateProductCategoryTest()
    {
        _repositoryMock =
            new Mock<IProductCategoryRepository>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _handler =
            new UpdateProductCategoryCommandHandler(
                _repositoryMock.Object,
                Mock.Of<BadmintonEcommerce.Mapper.Abstractions.IMapper>(),
                _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var command = new UpdateProductCategoryCommand
        {
            Id = categoryId,
            CategoryName = "Updated Category"
        };

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
            x => x.Update(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ParentCategoryLevelIs3_ShouldReturnFailure()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        Guid parentCategoryId = Guid.NewGuid();

        var existingCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = categoryId,
                CategoryName = "Shoes",
                Level = 1
            };

        var parentCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = parentCategoryId,
                CategoryName = "Parent",
                Level = 3
            };

        var command = new UpdateProductCategoryCommand
        {
            Id = categoryId,
            CategoryName = "Updated",
            ParentCategoryId = parentCategoryId
        };

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(existingCategory);

        _repositoryMock
            .Setup(x => x.GetById(parentCategoryId))
            .Returns(parentCategory);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductCategoryError.CannotCreateMoreThan3Levels());

        _repositoryMock.Verify(
            x => x.Update(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidParentCategory_ShouldUpdateSuccessfully()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        Guid parentCategoryId = Guid.NewGuid();

        var existingCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = categoryId,
                CategoryName = "Shoes",
                Level = 1
            };

        var parentCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = parentCategoryId,
                CategoryName = "Parent",
                Level = 2
            };

        DateTime utcNow = DateTime.UtcNow;

        var command = new UpdateProductCategoryCommand
        {
            Id = categoryId,
            CategoryName = "Updated Category",
            ParentCategoryId = parentCategoryId
        };

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(existingCategory);

        _repositoryMock
            .Setup(x => x.GetById(parentCategoryId))
            .Returns(parentCategory);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        existingCategory.CategoryName
            .Should().Be(command.CategoryName);

        existingCategory.ParentCategoryId
            .Should().Be(parentCategoryId);

        existingCategory.Level
            .Should().Be(3);

        existingCategory.LastModifiedOnUtc
            .Should().Be(utcNow);

        _repositoryMock.Verify(
            x => x.Update(existingCategory),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NoParentCategory_ShouldUpdateAsRootLevel()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        var existingCategory =
            new Domain.Entities.Catalog.ProductCategory
            {
                Id = categoryId,
                CategoryName = "Shoes",
                Level = 2
            };

        DateTime utcNow = DateTime.UtcNow;

        var command = new UpdateProductCategoryCommand
        {
            Id = categoryId,
            CategoryName = "Updated Root Category",
            ParentCategoryId = null
        };

        _repositoryMock
            .Setup(x => x.GetById(categoryId))
            .Returns(existingCategory);

        _repositoryMock
            .Setup(x => x.GetById((object?)null))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        existingCategory.CategoryName
            .Should().Be(command.CategoryName);

        existingCategory.ParentCategoryId
            .Should().BeNull();

        existingCategory.Level
            .Should().Be(1);

        existingCategory.LastModifiedOnUtc
            .Should().Be(utcNow);

        _repositoryMock.Verify(
            x => x.Update(existingCategory),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}