using BadmintonEcommerce.Application.Features.Product.UploadProductImage;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class UploadProductImageValidatorTest
{
    private readonly UploadProductImageCommandValidator _validator;

    public UploadProductImageValidatorTest()
    {
        _validator = new UploadProductImageCommandValidator();
    }

    private UploadProductImageCommand CreateValidCommand()
    {
        return new UploadProductImageCommand()
        {
            ProductId = Guid.NewGuid(),
            Files = new List<FileUploadStreamData>()
            {
                new()
                {
                    FileName = "image.jpg",
                    ContentType = "image/jpeg",
                    Stream = new MemoryStream(new byte[] { 1, 2, 3 })
                }
            }
        };
    }

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

    [Fact]
    public void Validate_EmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void Validate_NullFiles_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Files = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Files);
    }

    [Fact]
    public void Validate_EmptyFiles_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Files = new List<FileUploadStreamData>();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Files);
    }

    [Fact]
    public void Validate_NullFileItem_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Files = new List<FileUploadStreamData>()
        {
            null!
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Files[0]");
    }

    [Fact]
    public void Validate_EmptyFileName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.Files = new List<FileUploadStreamData>()
        {
            new()
            {
                FileName = string.Empty,
                ContentType = "image/jpeg",
                Stream = new MemoryStream()
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Files[0]");
    }

    [Fact]
    public void Validate_EmptyContentType_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.Files = new List<FileUploadStreamData>()
        {
            new()
            {
                FileName = "image.jpg",
                ContentType = string.Empty,
                Stream = new MemoryStream()
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Files[0]");
    }

    [Fact]
    public void Validate_NullStream_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        command.Files = new List<FileUploadStreamData>()
        {
            new()
            {
                FileName = "image.jpg",
                ContentType = "image/jpeg",
                Stream = null!
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Files[0]");
    }
}