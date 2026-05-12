using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.CreateOption;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class CreateProductOptionValueTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductOptionRepository> _productOptionRepositoryMock;
    private readonly Mock<IProductOptionValueRepository> _productOptionValueRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly CreateProductOptionValueCommandHandler _handler;

    public CreateProductOptionValueTest()
    {
        _mapperMock = new Mock<IMapper>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _productOptionRepositoryMock = new Mock<IProductOptionRepository>();
        _productOptionValueRepositoryMock = new Mock<IProductOptionValueRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _handler = new CreateProductOptionValueCommandHandler(
            _mapperMock.Object,
            _productRepositoryMock.Object,
            _productOptionRepositoryMock.Object,
            _productOptionValueRepositoryMock.Object,
            _dateTimeProviderMock.Object);
    }

    private static CreateProductOptionValueCommand CreateCommand()
    {
        return new CreateProductOptionValueCommand()
        {
            ProductId = Guid.NewGuid(),
            OptionName = "Color",
            OptionValues = ["Red", "Blue"]
        };
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = CreateCommand();

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns((Domain.Entities.Catalog.Product)null!);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductError.NotFound(command.ProductId));

        _productOptionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<ProductOption>()),
            Times.Never);

        _productOptionRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldInsertProductOption()
    {
        // Arrange
        var command = CreateCommand();

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId,
            Name = "Yonex Astrox 99"
        };

        var utcNow = DateTime.UtcNow;

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        ProductOption insertedOption = null!;

        _productOptionRepositoryMock
            .Setup(x => x.Insert(It.IsAny<ProductOption>()))
            .Callback<ProductOption>(x => insertedOption = x);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _productOptionRepositoryMock.Verify(
            x => x.Insert(It.IsAny<ProductOption>()),
            Times.Once);

        _productOptionRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        insertedOption.Should().NotBeNull();

        insertedOption.ProductId.Should().Be(command.ProductId);

        insertedOption.OptionName.Should().Be(command.OptionName);

        insertedOption.OptionValues.Should().HaveCount(2);

        insertedOption.OptionValues
            .Select(x => x.Value)
            .Should()
            .BeEquivalentTo(command.OptionValues);

        insertedOption.CreatedOnUtc.Should().Be(utcNow);
    }

    [Fact]
    public async Task Handle_ShouldCreateOptionValuesCorrectly()
    {
        // Arrange
        var command = new CreateProductOptionValueCommand()
        {
            ProductId = Guid.NewGuid(),
            OptionName = "Size",
            OptionValues = ["S", "M", "L"]
        };

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId
        };

        var utcNow = DateTime.UtcNow;

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        ProductOption insertedOption = null!;

        _productOptionRepositoryMock
            .Setup(x => x.Insert(It.IsAny<ProductOption>()))
            .Callback<ProductOption>(x => insertedOption = x);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        insertedOption.OptionValues.Should().HaveCount(3);

        insertedOption.OptionValues.ToList()[0].Value.Should().Be("S");
        insertedOption.OptionValues.ToList()[1].Value.Should().Be("M");
        insertedOption.OptionValues.ToList()[2].Value.Should().Be("L");

        insertedOption.OptionValues
            .All(x => x.CreatedOnUtc == utcNow)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Handle_EmptyOptionValues_ShouldStillCreateOption()
    {
        // Arrange
        var command = new CreateProductOptionValueCommand()
        {
            ProductId = Guid.NewGuid(),
            OptionName = "Material",
            OptionValues = []
        };

        var product = new Domain.Entities.Catalog.Product()
        {
            Id = command.ProductId
        };
        
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        ProductOption insertedOption = null!;

        _productOptionRepositoryMock
            .Setup(x => x.Insert(It.IsAny<ProductOption>()))
            .Callback<ProductOption>(x => insertedOption = x);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        insertedOption.Should().NotBeNull();

        insertedOption.OptionValues.Should().BeEmpty();
    }
}