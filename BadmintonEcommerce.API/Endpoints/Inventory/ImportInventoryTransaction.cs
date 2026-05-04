using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Inventory.Import;
using BadmintonEcommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Inventory;

public class ImportInventoryTransaction : IEndpoint
{
    public class ImportInventoryTransactionRequest
    {
        public Guid InventoryItemId { get; set; }
        public InventoryTransactionType Type { get; set; }
        public int Quantity { get; set; }
    }
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/transaction", async (
            [FromBody] ImportInventoryTransactionRequest request,
            [FromServices] ICommandHandler<ImportTransactionCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(new ImportTransactionCommand()
            {
                InventoryItemId = request.InventoryItemId,
                Type = request.Type,
                Quantity = request.Quantity
            }, cancellationToken);
            return result.Match(Results.Ok, CustomResult.Problem);
        }).WithTags(Tags.Inventory);
    }
}