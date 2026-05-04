using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Inventory;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Inventory.Import;

public class ImportTransactionCommandHandler(
    IInventoryItemRepository inventoryItemRepository, 
    IMapper mapper,
    IInventoryTransactionRepository inventoryTransactionRepository,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<ImportTransactionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ImportTransactionCommand command, CancellationToken cancellationToken)
    {
        //Check if inventory exists
        InventoryItem? inventory = inventoryItemRepository.GetById(command.InventoryItemId);
        if (inventory == null) 
            return Result.Failure<Guid>(InventoryItemError.NotFound(command.InventoryItemId));
        //Check if export and quantity to export is more than current quantity of inventory
        if (command.Type == InventoryTransactionType.Export && 
            inventory.Quantity < command.Quantity)
            return Result.Failure<Guid>(InventoryItemError.OutOfQuantity(command.InventoryItemId));
        //Import base on type
        switch (command.Type)
        {
            case InventoryTransactionType.Import:
                inventory.Quantity += command.Quantity;
                break;
            case InventoryTransactionType.Export:
                inventory.Quantity -= command.Quantity;
                break;
        }
        InventoryTransaction transaction = new InventoryTransaction()
        {
            InventoryItemId = command.InventoryItemId,
            Quantity = command.Quantity,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            Type = command.Type
        };
        inventoryTransactionRepository.Insert(transaction);
        
        await inventoryItemRepository.SaveChangesAsync();
        return Result.Success(transaction.Id);
    }
}