using Orleans.Runtime;

namespace Grains;

[GenerateSerializer]
public record struct MyState
{
    [Id(0)]
    public int Count { get; set; }
}

public class UserWalletGrain : Grain, IUserWalletGrain
{
    private readonly IPersistentState<MyState> _state;

    public UserWalletGrain([PersistentState("state", storageName: "OrleansMemoryProvider")] IPersistentState<MyState> state)
    {
        _state = state;
    }

    public async ValueTask<CreateOrderResponse> CreateOrder(CreateOrderCommand command)
    {
        var state = _state.State;
        _state.State = state with { Count = state.Count + 1 };
        await _state.WriteStateAsync();
        return new CreateOrderResponse { Result = Guid.NewGuid() };
    }

    public ValueTask Ping() => default;
}