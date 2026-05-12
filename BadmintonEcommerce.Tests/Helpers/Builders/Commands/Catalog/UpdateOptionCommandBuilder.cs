using BadmintonEcommerce.Application.Features.Product.UpdateOption;
using BadmintonEcommerce.Contracts.API.Presentation.Product.Requests;

namespace BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;

public class UpdateOptionCommandBuilder
{
    private Guid _productId;
    private List<CreateOptionRequest> _addedOptions;
    private List<CreateVariantRequest> _addedVariants;
    private List<Guid> _deletedOptions;
    private List<Guid> _deletedVariants;

    public UpdateOptionCommandBuilder WithProductId(Guid productId)
    {
        this._productId = productId;
        return this;
    }

    public UpdateOptionCommandBuilder WithAddedOptions(List<CreateOptionRequest> addedOptions)
    {
        this._addedOptions = addedOptions;
        return this;
    }

    public UpdateOptionCommandBuilder WithAddedVariants(List<CreateVariantRequest> addedVariants)
    {
        this._addedVariants = addedVariants;
        return this;
    }

    public UpdateOptionCommandBuilder WithDeletedOptions(List<Guid> deletedOptions)
    {
        this._deletedOptions = deletedOptions;
        return this;
    }

    public UpdateOptionCommandBuilder WithDeletedVariants(List<Guid> deletedVariants)
    {
        this._deletedVariants = deletedVariants;
        return this;
    }

    public UpdateOptionCommand Build() => new UpdateOptionCommand()
    {
        ProductId = this._productId,
        AddedOptions = this._addedOptions,
        AddedVariants = this._addedVariants,
        DeletedOptions = this._deletedOptions,
        DeletedVariants = this._deletedVariants
    };

    public UpdateOptionCommand Valid() => new UpdateOptionCommand()
    {
        ProductId = Guid.NewGuid(),
        AddedOptions = new List<CreateOptionRequest>()
        {
            new CreateOptionRequest()
            {
                Code = "color",
                Name = "Color",
                Values = ["Red", "Blue"]
            }
        },
        AddedVariants = new List<CreateVariantRequest>()
        {
            new CreateVariantRequest()
            {
                Price = 100,
                Stock = 10,
                Values =
                [
                    new OptionValueRequest()
                    {
                        Code = "color",
                        Value = "Red"
                    }
                ]
            }
        },
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