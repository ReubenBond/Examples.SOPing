namespace Grains;

public interface IUserWalletGrain : IGrainWithIntegerKey
{
    ValueTask<CreateOrderResponse> CreateOrder(CreateOrderCommand command);
    ValueTask Ping();
}