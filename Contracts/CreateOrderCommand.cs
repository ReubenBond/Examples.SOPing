namespace Grains;

[GenerateSerializer]
public struct CreateOrderCommand
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public int One { get; set; }

    [Id(2)]
    public int Two { get; set; }

    [Id(3)]
    public List<Guid>? Values { get; set; }
}