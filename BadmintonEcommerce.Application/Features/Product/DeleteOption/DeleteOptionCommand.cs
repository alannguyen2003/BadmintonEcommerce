using System.Diagnostics.CodeAnalysis;
using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Product.DeleteOption;

[ExcludeFromCodeCoverage]
public sealed record DeleteOptionCommand(Guid ProductOptionId) : ICommand;