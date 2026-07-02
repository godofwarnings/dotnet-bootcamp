using System;

namespace ConsoleApp1;

public abstract class Delivery
{
    protected Delivery(decimal orderAmount)
    {
        if (orderAmount < 0)
        {

            Console.WriteLine("OrderAmount value cannot be negative.");
            return;
        }

        OrderAmount = orderAmount;
    }

    protected decimal OrderAmount { get; }

    public abstract decimal CalculateDeliveryCharge();

    public virtual void DisplayBill()
    {
        decimal deliveryCharge = CalculateDeliveryCharge();
        decimal finalBill = OrderAmount + deliveryCharge;

        Console.WriteLine($"Order Amount: {OrderAmount}");
        Console.WriteLine($"Delivery Charge: {deliveryCharge}");
        Console.WriteLine($"Final Bill: {finalBill}");
    }
}

public class StandardDelivery : Delivery
{
    private const decimal DeliveryCharge = 50m;

    public StandardDelivery(decimal orderAmount) : base(orderAmount)
    {
    }

    public override decimal CalculateDeliveryCharge() => DeliveryCharge;
}

public class ExpressDelivery : Delivery
{
    private const decimal DeliveryCharge = 100m;

    public ExpressDelivery(decimal orderAmount) : base(orderAmount)
    {
    }

    public override decimal CalculateDeliveryCharge() => DeliveryCharge;
}

public class InternationalDelivery : Delivery
{
    private const decimal DeliveryRate = 0.10m;

    public InternationalDelivery(decimal orderAmount) : base(orderAmount)
    {
    }

    public override decimal CalculateDeliveryCharge() => OrderAmount * DeliveryRate;
}

public sealed class FreeDelivery : Delivery
{
    private const decimal FreeDeliveryThreshold = 2000m;
    private const decimal DefaultDeliveryCharge = 40m;

    public FreeDelivery(decimal orderAmount) : base(orderAmount)
    {
    }

    public override decimal CalculateDeliveryCharge() =>
        OrderAmount > FreeDeliveryThreshold ? 0m : DefaultDeliveryCharge;
}
