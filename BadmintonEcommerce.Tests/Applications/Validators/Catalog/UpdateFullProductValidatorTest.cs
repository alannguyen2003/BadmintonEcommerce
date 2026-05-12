using BadmintonEcommerce.Application.Features.Product.UpdateProduct;
using BadmintonEcommerce.Contracts.API.Presentation.File.Request;
using FluentValidation.TestHelper;

namespace BadmintonEcommerce.Tests.Applications.Validators.Catalog;

public class UpdateFullProductValidatorTest
{
    private readonly UpdateFullProductCommandValidator _validator;

    public UpdateFullProductValidatorTest()
    {
        _validator = new UpdateFullProductCommandValidator();
    }

    private UpdateFullProductCommand CreateValidCommand()
    {
        return new UpdateFullProductCommand()
        {
            Id = Guid.NewGuid(),
            Name = "Yonex Astrox 100ZZ",
            Description = "Professional badminton racket",
            Brand = "Yonex",
            CategoryId = Guid.NewGuid(),
            Status = true,

            DeletedImages = new List<Guid>(),
            AddedImages = new List<FileUploadStreamData>()
            {
                new()
                {
                    FileName = "image.jpg",
                    ContentType = "image/jpeg",
                    Stream = new MemoryStream(new byte[] {1, 2, 3})
                }
            },

            UpdatedOptions = new List<UpdateOption>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    OptionName = "Color"
                }
            },

            DeletedOptions = new List<Guid>(),
            DeletedOptionValues = new List<Guid>(),

            UpdatedOptionValues = new List<UpdateOptionValue>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    OptionId = Guid.NewGuid(),
                    NewValue = "Red"
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
    public void Validate_EmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Id = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_EmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Name = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_NameExceeds200Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Name = new string('A', 201);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_EmptyDescription_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Description = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_DescriptionExceeds4000Characters_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Description = new string('A', 4001);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_EmptyBrand_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Brand = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Brand);
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
        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Validate_EmptyCategoryId_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.CategoryId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Fact]
    public void Validate_NullDeletedImages_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.DeletedImages = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedImages);
    }

    [Fact]
    public void Validate_DeletedImagesContainsEmptyGuid_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.DeletedImages = new List<Guid>()
        {
            Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("DeletedImages[0]");
    }

    [Fact]
    public void Validate_DuplicateDeletedImages_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        var id = Guid.NewGuid();

        command.DeletedImages = new List<Guid>()
        {
            id,
            id
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedImages);
    }

    [Fact]
    public void Validate_NullAddedImages_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.AddedImages = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddedImages);
    }

    [Fact]
    public void Validate_AddedImagesContainsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.AddedImages = new List<FileUploadStreamData>()
        {
            null!
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("AddedImages[0]");
    }

    [Fact]
    public void Validate_NullUpdatedOptions_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.UpdatedOptions = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UpdatedOptions);
    }

    [Fact]
    public void Validate_DuplicateUpdatedOptionIds_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        var optionId = Guid.NewGuid();

        command.UpdatedOptions = new List<UpdateOption>()
        {
            new()
            {
                Id = optionId,
                OptionName = "Color"
            },
            new()
            {
                Id = optionId,
                OptionName = "Size"
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UpdatedOptions);
    }

    [Fact]
    public void Validate_NullDeletedOptions_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.DeletedOptions = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedOptions);
    }

    [Fact]
    public void Validate_DuplicateDeletedOptions_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        var id = Guid.NewGuid();

        command.DeletedOptions = new List<Guid>()
        {
            id,
            id
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedOptions);
    }

    [Fact]
    public void Validate_NullDeletedOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.DeletedOptionValues = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedOptionValues);
    }

    [Fact]
    public void Validate_DuplicateDeletedOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        var id = Guid.NewGuid();

        command.DeletedOptionValues = new List<Guid>()
        {
            id,
            id
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DeletedOptionValues);
    }

    [Fact]
    public void Validate_NullUpdatedOptionValues_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();
        command.UpdatedOptionValues = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UpdatedOptionValues);
    }

    [Fact]
    public void Validate_DuplicateUpdatedOptionValueIds_ShouldHaveValidationError()
    {
        // Arrange
        var command = CreateValidCommand();

        var valueId = Guid.NewGuid();

        command.UpdatedOptionValues = new List<UpdateOptionValue>()
        {
            new()
            {
                Id = valueId,
                OptionId = Guid.NewGuid(),
                NewValue = "Red"
            },
            new()
            {
                Id = valueId,
                OptionId = Guid.NewGuid(),
                NewValue = "Blue"
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UpdatedOptionValues);
    }
}

public class UpdateOptionValidatorTests
{
    private readonly UpdateOptionValidator _validator;

    public UpdateOptionValidatorTests()
    {
        _validator = new UpdateOptionValidator();
    }

    [Fact]
    public void Validate_ValidOption_ShouldNotHaveValidationError()
    {
        // Arrange
        var option = new UpdateOption()
        {
            Id = Guid.NewGuid(),
            OptionName = "Color"
        };

        // Act
        var result = _validator.TestValidate(option);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var option = new UpdateOption()
        {
            Id = Guid.Empty,
            OptionName = "Color"
        };

        // Act
        var result = _validator.TestValidate(option);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_EmptyOptionName_ShouldHaveValidationError()
    {
        // Arrange
        var option = new UpdateOption()
        {
            Id = Guid.NewGuid(),
            OptionName = string.Empty
        };

        // Act
        var result = _validator.TestValidate(option);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionName);
    }

    [Fact]
    public void Validate_OptionNameExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var option = new UpdateOption()
        {
            Id = Guid.NewGuid(),
            OptionName = new string('A', 101)
        };

        // Act
        var result = _validator.TestValidate(option);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionName);
    }
}

public class UpdateOptionValueValidatorTest
{
    private readonly UpdateOptionValueValidator _validator;

    public UpdateOptionValueValidatorTest()
    {
        _validator = new UpdateOptionValueValidator();
    }

    [Fact]
    public void Validate_ValidOptionValue_ShouldNotHaveValidationError()
    {
        // Arrange
        var optionValue = new UpdateOptionValue()
        {
            Id = Guid.NewGuid(),
            OptionId = Guid.NewGuid(),
            NewValue = "Red"
        };

        // Act
        var result = _validator.TestValidate(optionValue);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var optionValue = new UpdateOptionValue()
        {
            Id = Guid.Empty,
            OptionId = Guid.NewGuid(),
            NewValue = "Red"
        };

        // Act
        var result = _validator.TestValidate(optionValue);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_EmptyOptionId_ShouldHaveValidationError()
    {
        // Arrange
        var optionValue = new UpdateOptionValue()
        {
            Id = Guid.NewGuid(),
            OptionId = Guid.Empty,
            NewValue = "Red"
        };

        // Act
        var result = _validator.TestValidate(optionValue);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OptionId);
    }

    [Fact]
    public void Validate_EmptyNewValue_ShouldHaveValidationError()
    {
        // Arrange
        var optionValue = new UpdateOptionValue()
        {
            Id = Guid.NewGuid(),
            OptionId = Guid.NewGuid(),
            NewValue = string.Empty
        };

        // Act
        var result = _validator.TestValidate(optionValue);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewValue);
    }

    [Fact]
    public void Validate_NewValueExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var optionValue = new UpdateOptionValue()
        {
            Id = Guid.NewGuid(),
            OptionId = Guid.NewGuid(),
            NewValue = new string('A', 101)
        };

        // Act
        var result = _validator.TestValidate(optionValue);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewValue);
    }
}