using System;

interface IInternalDeliveryService
{
    void DeliverOrder(string orderId);
    string GetDeliveryStatus(string orderId);
    double CalculateDeliveryCost(double weight);
}

class InternalDeliveryService : IInternalDeliveryService
{
    public void DeliverOrder(string orderId) => Console.WriteLine($"Внутренняя служба доставки: заказ {orderId} доставлен.");
    public string GetDeliveryStatus(string orderId) => $"Внутренняя служба доставки: статус доставки заказа {orderId}.";
    public double CalculateDeliveryCost(double weight) => weight * 5;
}

class ExternalLogisticsServiceA
{
    public void ShipItem(int itemId) => Console.WriteLine($"Сторонняя служба A: отправка товара {itemId}.");
    public string TrackShipment(int shipmentId) => $"Сторонняя служба A: статус отправки {shipmentId}.";
    public double CalculateShippingCost(double weight) => weight * 4;
}

class LogisticsAdapterA : IInternalDeliveryService
{
    private ExternalLogisticsServiceA _externalService;
    public LogisticsAdapterA(ExternalLogisticsServiceA externalService) => _externalService = externalService;
    public void DeliverOrder(string orderId) => _externalService.ShipItem(int.Parse(orderId));
    public string GetDeliveryStatus(string orderId) => _externalService.TrackShipment(int.Parse(orderId));
    public double CalculateDeliveryCost(double weight) => _externalService.CalculateShippingCost(weight);
}

class ExternalLogisticsServiceB
{
    public void SendPackage(string packageInfo) => Console.WriteLine($"Сторонняя служба B: отправка посылки {packageInfo}.");
    public string CheckPackageStatus(string trackingCode) => $"Сторонняя служба B: статус посылки {trackingCode}.";
    public double CalculatePackageCost(double weight) => weight * 6;
}

class LogisticsAdapterB : IInternalDeliveryService
{
    private ExternalLogisticsServiceB _externalService;
    public LogisticsAdapterB(ExternalLogisticsServiceB externalService) => _externalService = externalService;
    public void DeliverOrder(string orderId) => _externalService.SendPackage(orderId);
    public string GetDeliveryStatus(string orderId) => _externalService.CheckPackageStatus(orderId);
    public double CalculateDeliveryCost(double weight) => _externalService.CalculatePackageCost(weight);
}

class DeliveryServiceFactory
{
    public static IInternalDeliveryService GetDeliveryService(string serviceType) => serviceType switch
    {
        "Internal" => new InternalDeliveryService(),
        "ExternalA" => new LogisticsAdapterA(new ExternalLogisticsServiceA()),
        "ExternalB" => new LogisticsAdapterB(new ExternalLogisticsServiceB()),
        _ => throw new ArgumentException("Неизвестный тип службы доставки")
    };
}

class Program
{
    static void Main()
    {
        IInternalDeliveryService internalService = DeliveryServiceFactory.GetDeliveryService("Internal");
        internalService.DeliverOrder("123");
        Console.WriteLine(internalService.GetDeliveryStatus("123"));
        Console.WriteLine($"Стоимость доставки: {internalService.CalculateDeliveryCost(10)}");

        IInternalDeliveryService externalServiceA = DeliveryServiceFactory.GetDeliveryService("ExternalA");
        externalServiceA.DeliverOrder("456");
        Console.WriteLine(externalServiceA.GetDeliveryStatus("456"));
        Console.WriteLine($"Стоимость доставки: {externalServiceA.CalculateDeliveryCost(10)}");

        IInternalDeliveryService externalServiceB = DeliveryServiceFactory.GetDeliveryService("ExternalB");
        externalServiceB.DeliverOrder("789");
        Console.WriteLine(externalServiceB.GetDeliveryStatus("789"));
        Console.WriteLine($"Стоимость доставки: {externalServiceB.CalculateDeliveryCost(10)}");
    }
}
