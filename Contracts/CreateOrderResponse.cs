namespace Grains;

[GenerateSerializer]
public struct CreateOrderResponse
{
    [Id(0)]
    public Guid Result { get; set; }
}