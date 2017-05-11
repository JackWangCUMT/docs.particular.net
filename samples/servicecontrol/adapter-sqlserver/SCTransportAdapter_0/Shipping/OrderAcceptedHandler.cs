using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    ChaosGenerator chaos;

    public OrderAcceptedHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.Info($"Shipping order {message.OrderId} for {message.Value}");
        await chaos.Invoke();
        await context.Publish(new OrderShipped
        {
            OrderId = message.OrderId,
            Value = message.Value
        });
    }
}