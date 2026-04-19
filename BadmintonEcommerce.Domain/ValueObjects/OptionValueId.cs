using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Domain.ValueObjects;

public class OptionValueId : ValueObject
{
    public Guid Value { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}