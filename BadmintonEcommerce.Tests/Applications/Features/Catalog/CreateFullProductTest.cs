using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Application.Features.Product.CreateProduct;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Mapper.Abstractions;
using CloudinaryDotNet.Actions;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class CreateFullProductTest
{
    private readonly Mock<IProductCategoryRepository>
        _productCategoryRepositoryMock;

    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductVariantRepository>
        _productVariantRepositoryMock;

    private readonly Mock<IProductImageRepository>
        _productImageRepositoryMock;

    private readonly Mock<IProductOptionRepository>
        _productOptionRepositoryMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly Mock<IInventoryItemRepository>
        _inventoryItemRepositoryMock;

    private readonly Mock<IDateTimeProvider>
        _dateTimeProviderMock;

    private readonly Mock<IFileService>
        _fileServiceMock;

    private readonly CreateFullProductCommandHandler
        _handler;

    public CreateFullProductTest()
    {
        _productCategoryRepositoryMock =
            new Mock<IProductCategoryRepository>();

        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productVariantRepositoryMock =
            new Mock<IProductVariantRepository>();

        _productImageRepositoryMock =
            new Mock<IProductImageRepository>();

        _productOptionRepositoryMock =
            new Mock<IProductOptionRepository>();

        _mapperMock =
            new Mock<IMapper>();

        _inventoryItemRepositoryMock =
            new Mock<IInventoryItemRepository>();

        _dateTimeProviderMock =
            new Mock<IDateTimeProvider>();

        _fileServiceMock =
            new Mock<IFileService>();

        _handler =
            new CreateFullProductCommandHandler(
                _productCategoryRepositoryMock.Object,
                _productRepositoryMock.Object,
                _productVariantRepositoryMock.Object,
                _productImageRepositoryMock.Object,
                _productOptionRepositoryMock.Object,
                _mapperMock.Object,
                _inventoryItemRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _fileServiceMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = CreateValidCommand();

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.ProductCategoryId))
            .Returns((ProductCategory?)null);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Insert(It.IsAny<Product>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateOptions_ShouldReturnFailure()
    {
        // Arrange
        var command = CreateValidCommand();

        command.OptionRequests =
        [
            new CreateOptionRequest
            {
                Code = "COLOR",
                Name = "Color",
                Values = ["Red"]
            },
            new CreateOptionRequest
            {
                Code = "COLOR",
                Name = "Color 2",
                Values = ["Blue"]
            }
        ];

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.ProductCategoryId))
            .Returns(new ProductCategory());

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Insert(It.IsAny<Product>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProductSuccessfully()
    {
        // Arrange
        var command = CreateValidCommand();

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.ProductCategoryId))
            .Returns(new ProductCategory
            {
                Id = command.ProductCategoryId,
                CategoryName = "Rackets"
            });

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _fileServiceMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "image",
                SecureUrl = new Uri(
                    "https://cloudinary.com/test.jpg")
            });

        Product? insertedProduct = null;

        _productRepositoryMock
            .Setup(x => x.Insert(It.IsAny<Product>()))
            .Callback<Product>(x => insertedProduct = x);

        // Act
        Result<Guid> result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        insertedProduct.Should().NotBeNull();

        insertedProduct!.Name
            .Should().Be(command.ProductName);

        insertedProduct.Brand
            .Should().Be(command.Brand);

        insertedProduct.Options
            .Count.Should().Be(2);

        insertedProduct.Variants
            .Count.Should().Be(2);

        insertedProduct.Images
            .Count.Should().Be(1);

        insertedProduct.Images
            .First().IsPrimary.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Insert(It.IsAny<Product>()),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _inventoryItemRepositoryMock.Verify(
            x => x.Insert(It.IsAny<InventoryItem>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_InvalidVariantCombination_ShouldThrowException()
    {
        // Arrange
        var command = CreateValidCommand();

        command.VariantRequests =
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
                        Value = "Green",
                        Name = "Color"
                    }
                ]
            }
        ];

        _productCategoryRepositoryMock
            .Setup(x => x.GetById(command.ProductCategoryId))
            .Returns(new ProductCategory());

        _fileServiceMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<FileUploadStream>()))
            .ReturnsAsync(new ImageUploadResult()
            {
                DisplayName = "image",
                SecureUrl = new Uri(
                    "https://cloudinary.com/test.jpg")
            });

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
    public void CheckIfAnyOptionDuplicate_DuplicateCodes_ShouldReturnTrue()
    {
        // Arrange
        var options =
            new List<CreateOptionRequest>
            {
                new()
                {
                    Code = "color",
                    Name = "Color"
                },
                new()
                {
                    Code = "color",
                    Name = "Color"
                }
            };

        // Act
        bool result =
            _handler.CheckIfAnyOptionDuplicate(
                options);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CheckIfAnyOptionDuplicate_UniqueCodes_ShouldReturnFalse()
    {
        // Arrange
        var options =
            new List<CreateOptionRequest>
            {
                new()
                {
                    Code = "color",
                    Name = "Color"
                },
                new()
                {
                    Code = "size",
                    Name = "Size"
                }
            };

        // Act
        bool result =
            _handler.CheckIfAnyOptionDuplicate(
                options);

        // Assert
        result.Should().BeFalse();
    }

    private static CreateFullProductCommand
        CreateValidCommand()
    {
        return new CreateFullProductCommand
        {
            ProductName = "Yonex Astrox 99",
            ProductDescription = "Professional racket",
            Brand = "Yonex",
            ProductCategoryId = Guid.NewGuid(),
            Status = true,

            Files =
            [
                new FileUploadStreamData
                {
                    FileName = "test.jpg",
                    ContentType = "image/jpeg",
                    Stream = new MemoryStream([1, 2, 3])
                }
            ],

            OptionRequests =
            [
                new CreateOptionRequest
                {
                    Code = "COLOR",
                    Name = "Color",
                    Values = ["Red", "Blue"]
                },

                new CreateOptionRequest
                {
                    Code = "SIZE",
                    Name = "Size",
                    Values = ["4U", "3U"]
                }
            ],

            VariantRequests =
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
                            Name = "Color",
                            Value = "Red"
                        },
                        new OptionValueRequest()
                        {
                            Code = "size",
                            Name = "Size",
                            Value = "4U"
                        }
                    ]
                },

                new CreateVariantRequest
                {
                    Price = 120,
                    Stock = 5,
                    Values =
                    [
                        new OptionValueRequest()
                        {
                            Code = "color",
                            Name = "Color",
                            Value = "Blue"
                        },
                        new OptionValueRequest()
                        {
                            Code = "size",
                            Name = "Size",
                            Value = "3U"
                        }
                    ]
                }
            ]
        };
    }
}