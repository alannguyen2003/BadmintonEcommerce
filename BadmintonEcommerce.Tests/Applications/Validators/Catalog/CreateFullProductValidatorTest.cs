using BadmintonEcommerce.Application.Features.Product.CreateProduct;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class CreateFullProductValidatorTest
{
    private readonly CreateFullProductCommandValidator
        _validator;

    public CreateFullProductValidatorTest()
    {
        _validator = new CreateFullProductCommandValidator();
    }

    #region ProductName

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_InvalidProductName_ShouldHaveValidationError(
        string? productName)
    {
        // Arrange
        var command = CreateValidCommand();

        command.ProductName = productName;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductName);
    }

    [Fact]
    public void Validate_ProductNameExceeds200Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.ProductName = new string('A', 201);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductName);
    }

    [Fact]
    public void Validate_ValidProductName_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductName);
    }

    #endregion

    #region ProductDescription

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_InvalidProductDescription_ShouldHaveValidationError(
        string? description)
    {
        // Arrange
        var command = CreateValidCommand();

        command.ProductDescription = description;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductDescription);
    }

    [Fact]
    public void Validate_ProductDescriptionExceeds4000Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.ProductDescription = new string('A', 4001);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductDescription);
    }

    #endregion

    #region Brand

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_InvalidBrand_ShouldHaveValidationError(
        string? brand)
    {
        // Arrange
        var command = CreateValidCommand();

        command.Brand = brand;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Brand);
    }

    [Fact]
    public void Validate_BrandExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.Brand = new string('A', 101);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Brand);
    }

    #endregion

    #region ProductCategoryId

    [Fact]
    public void Validate_EmptyProductCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.ProductCategoryId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.ProductCategoryId);
    }

    [Fact]
    public void Validate_ValidProductCategoryId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(
            x => x.ProductCategoryId);
    }

    #endregion

    #region OptionRequests

    [Fact]
    public void Validate_NullOptionRequests_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.OptionRequests = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.OptionRequests);
    }

    [Fact]
    public void Validate_InvalidOptionRequest_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.OptionRequests =
        [
            new CreateOptionRequest
            {
                Name = "",
                Code = "",
                Values = []
            }
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    #endregion

    #region VariantRequests

    [Fact]
    public void Validate_NullVariantRequests_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.VariantRequests = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.VariantRequests);
    }

    [Fact]
    public void Validate_EmptyVariantRequests_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.VariantRequests = [];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.VariantRequests);
    }

    [Fact]
    public void Validate_InvalidVariantRequest_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.VariantRequests =
        [
            new CreateVariantRequest
            {
                Price = -1,
                Stock = -1,
                Values = []
            }
        ];

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }

    #endregion

    #region Files

    [Fact]
    public void Validate_NullFiles_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.Files = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(
            x => x.Files);
    }

    #endregion

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateFullProductCommand
        CreateValidCommand()
    {
        return new CreateFullProductCommand
        {
            ProductName = "Yonex Astrox 99",
            ProductDescription = "Professional badminton racket",
            Brand = "Yonex",
            ProductCategoryId = Guid.NewGuid(),

            OptionRequests =
            [
                new CreateOptionRequest
                {
                    Name = "Color",
                    Code = "color",
                    Values = ["Red", "Blue"]
                }
            ],

            VariantRequests =
            [
                new CreateVariantRequest
                {
                    Price = 100,
                    Stock = 10,
                    Values = [
                        new OptionValueRequest()
                        {
                            Name = "color",
                            Code = "color",
                            Value = "Red"
                        }
                    ]
                }
            ],
            Files = []
        };
    }
}