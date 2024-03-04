namespace dictionaries;

struct Product
{
    public int ProductId;
    public string VendorName;

    public override string ToString() => $"ProductId: {ProductId}, VendorName: {VendorName}";
}

class Program
{
    static void PrintPrices(Dictionary<Product, decimal> prices)
    {
        foreach (var price in prices)
        {
            Console.WriteLine(price);
        }
    }

    static void Main(string[] args)
    {
        var prices = new Dictionary<Product, decimal> {
            { new Product { ProductId = 1, VendorName = "Contoso" }, 100m },
            { new Product { ProductId = 2, VendorName = "Fabrikam" }, 150m }
        };

        var prices2 = new Dictionary<Product, decimal> {
            { new Product { ProductId = 2, VendorName = "Fabrikam" }, 150m },
            { new Product { ProductId = 1, VendorName = "Contoso" }, 100m }
        };

        PrintPrices(prices);
        PrintPrices(prices2);

        Console.WriteLine(prices.Equals(prices2)); // False
        Console.WriteLine(prices.SequenceEqual(prices2)); // False
        Console.WriteLine(prices.OrderBy(p => p.Key.ProductId).SequenceEqual(prices2.OrderBy(p => p.Key.ProductId))); // True
    }
}
