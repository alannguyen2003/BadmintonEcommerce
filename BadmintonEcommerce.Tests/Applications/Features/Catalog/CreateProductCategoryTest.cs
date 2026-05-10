using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.ProductCategory.Create;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class CreateProductCategoryTest
{
    private readonly Mock<IProductCategoryRepository>
        _repositoryMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly CreateProductCategoryCommandHandler
        _handler;

    public CreateProductCategoryTest()
    {
        _repositoryMock =
            new Mock<IProductCategoryRepository>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _handler =
            new CreateProductCategoryCommandHandler(
                _repositoryMock.Object,
                _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_RootCategory_ShouldCreateSuccessfully()
    {
        // Arrange
        var command = new CreateProductCategoryCommandBuilder().Valid();

        DateTime utcNow = DateTime.UtcNow;

        Domain.Entities.Catalog.ProductCategory?
            insertedCategory = null;

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        _repositoryMock
            .Setup(x => x.Insert(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()))
            .Callback<Domain.Entities.Catalog.ProductCategory>(
                category => insertedCategory = category);

        // Act
        Result<Guid> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        insertedCategory.Should().NotBeNull();

        insertedCategory!.CategoryName
            .Should().Be(command.CategoryName);

        insertedCategory.Level
            .Should().Be(1);

        insertedCategory.ParentCategoryId
            .Should().BeNull();

        insertedCategory.CreatedOnUtc
            .Should().Be(utcNow);

        _repositoryMock.Verify(
            x => x.Insert(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ParentCategoryExists_ShouldCreateChildCategory()
    {
        // Arrange
        Guid parentId = Guid.NewGuid();

        var command = new CreateProductCategoryCommandBuilder().Valid();
        
        command.ParentCategoryId = parentId;

        var parentCategory = new ProductCategoryBuilder()
            .WithId(parentId)
            .WithName("Badminton")
            .WithLevel(2)
            .Build();

        DateTime utcNow = DateTime.UtcNow;

        Domain.Entities.Catalog.ProductCategory?
            insertedCategory = null;

        _repositoryMock
            .Setup(x => x.GetById(parentId))
            .Returns(parentCategory);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        _repositoryMock
            .Setup(x => x.Insert(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()))
            .Callback<Domain.Entities.Catalog.ProductCategory>(
                category => insertedCategory = category);

        // Act
        Result<Guid> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        insertedCategory.Should().NotBeNull();

        insertedCategory!.ParentCategoryId
            .Should().Be(parentId);

        insertedCategory.Level
            .Should().Be(3);

        insertedCategory.CreatedOnUtc
            .Should().Be(utcNow);

        _repositoryMock.Verify(
            x => x.GetById(parentId),
            Times.Once);

        _repositoryMock.Verify(
            x => x.Insert(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ParentCategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid parentId = Guid.NewGuid();
        
        var command = new CreateProductCategoryCommandBuilder().Valid();
        
        command.ParentCategoryId = parentId;

        _repositoryMock
            .Setup(x => x.GetById(parentId))
            .Returns((Domain.Entities.Catalog.ProductCategory?)null);

        // Act
        Result<Guid> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductCategoryError.NotFound(parentId));

        _repositoryMock.Verify(
            x => x.Insert(
                It.IsAny<Domain.Entities.Catalog.ProductCategory>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    public void CheckIfTheLevelIsMoreThan3_ShouldReturnExpectedResult(
        int currentLevel,
        bool expected)
    {
        // Act
        bool result =
            _handler.CheckIfTheLevelIsMoreThan3(currentLevel);

        // Assert
        result.Should().Be(expected);
    }
}