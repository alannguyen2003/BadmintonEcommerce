using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.UpdateOption;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class UpdateOptionTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductOptionRepository>
        _productOptionRepositoryMock;

    private readonly Mock<IProductOptionValueRepository>
        _productOptionValueRepositoryMock;

    private readonly Mock<IProductVariantRepository>
        _productVariantRepositoryMock;

    private readonly Mock<IInventoryItemRepository>
        _inventoryItemRepositoryMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly UpdateOptionCommandHandler
        _handler;

    public UpdateOptionTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productOptionRepositoryMock =
            new Mock<IProductOptionRepository>();

        _productOptionValueRepositoryMock =
            new Mock<IProductOptionValueRepository>();

        _productVariantRepositoryMock =
            new Mock<IProductVariantRepository>();

        _inventoryItemRepositoryMock =
            new Mock<IInventoryItemRepository>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _handler =
            new UpdateOptionCommandHandler(
                _productRepositoryMock.Object,
                _productOptionRepositoryMock.Object,
                _productOptionValueRepositoryMock.Object,
                _productVariantRepositoryMock.Object,
                _inventoryItemRepositoryMock.Object,
                _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldThrowException()
    {
        // Arrange
        var command = CreateValidCommand();

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync(new List<Product>());

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Handle_DeleteVariants_ShouldCallDeleteRepository()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = CreateProduct();

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);
        
        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _productVariantRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Guid>()),
            Times.Exactly(command.DeletedVariants.Count));
    }

    [Fact]
    public async Task Handle_DeleteOptions_ShouldCallDeleteRepository()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = CreateProduct();
        
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _productOptionRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Guid>()),
            Times.Exactly(command.DeletedOptions.Count));
    }

    [Fact]
    public async Task Handle_AddedOptions_ShouldAddOptionsToProduct()
    {
        // Arrange
        var command = CreateValidCommand();

        Product? updatedProduct = null;

        var product = CreateProduct();
        
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        _productRepositoryMock
            .Setup(x => x.Update(It.IsAny<Product>()))
            .Callback<Product>(x => updatedProduct = x);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        updatedProduct.Should().NotBeNull();

        updatedProduct!.Options
            .Count.Should().Be(1);

        updatedProduct.Options
            .First().OptionValues
            .Count.Should().Be(2);
    }

    [Fact]
    public async Task Handle_AddedVariants_ShouldAddVariantsToProduct()
    {
        // Arrange
        var command = CreateValidCommand();

        Product? updatedProduct = null;

        var product = CreateProduct();
        
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        _productRepositoryMock
            .Setup(x => x.Update(It.IsAny<Product>()))
            .Callback<Product>(x => updatedProduct = x);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        updatedProduct.Should().NotBeNull();

        updatedProduct!.Variants
            .Count.Should().Be(1);

        updatedProduct.Variants
            .First().Combinations
            .Count.Should().Be(2);
    }

    [Fact]
    public async Task Handle_AddedVariants_ShouldInsertInventoryItems()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = CreateProduct();

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _inventoryItemRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryItem>()),
            Times.Exactly(command.AddedVariants.Count));
    }

    [Fact]
    public async Task Handle_InvalidVariantCombination_ShouldThrowException()
    {
        // Arrange
        var command = CreateValidCommand();

        command.AddedVariants =
        [
            new CreateVariantRequest
            {
                Price = 100,
                Stock = 10,
                Values =
                [
                    new OptionValueRequest()
                    {
                        Code = "color",
                        Value = "Green"
                    }
                ]
            }
        ];

        var product = CreateProduct();
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);
        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateProductSuccessfully()
    {
        // Arrange
        var command = CreateValidCommand();

        var product = CreateProduct();
        
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    System.Linq.Expressions.Expression<
                        Func<Product, bool>>>(),
                null,
                "Options,Variants",
                null,
                null))
            .ReturnsAsync([product]);

        // Act
        Result result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    private static Product CreateProduct()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = "Yonex Astrox",
            Slug = "yonex-astrox",
            Options = [],
            Variants = []
        };
    }

    private static UpdateOptionCommand
        CreateValidCommand()
    {
        return new UpdateOptionCommand
        {
            ProductId = Guid.NewGuid(),

            AddedOptions =
            [
                new CreateOptionRequest
                {
                    Code = "COLOR",
                    Name = "Color",
                    Values = ["Red", "Blue"]
                }
            ],

            AddedVariants =
            [
                new CreateVariantRequest
                {
                    Price = 100,
                    Stock = 10,
                    Values =
                    [
                        new OptionValueRequest()
                        {
                            Code = "color",
                            Value = "Red"
                        },
                        new OptionValueRequest()
                        {
                            Code = "color",
                            Value = "Blue"
                        }
                    ]
                }
            ],

            DeletedOptions =
            [
                Guid.NewGuid()
            ],

            DeletedVariants =
            [
                Guid.NewGuid()
            ]
        };
    }
}