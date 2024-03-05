namespace dictionaries;

struct Product : IComparable<Product>
{
    public int ProductId;
    public string VendorName;

    public override string ToString() => $"ProductId: {ProductId}, VendorName: {VendorName}";

    public int CompareTo(Product other)
    {
        if (ProductId < other.ProductId)
        {
            return -1;
        }
        if (ProductId > other.ProductId)
        {
            return 1;
        }
        return VendorName.CompareTo(other.VendorName);
    }
}

class Program
{
    public Random random = new Random();

    static void PrintPrices(Dictionary<Product, decimal> prices)
    {
        foreach (var price in prices)
        {
            Console.WriteLine(price);
        }
    }

    static void Main(string[] args)
    {
        var program = new Program();
        program.FillDictionaries();   
    }

    void FillDictionaries()
    {
        var prices = new Dictionary<Product, decimal>();
        FillPrices(prices);
        var prices2 = new SortedDictionary<Product, decimal>();
        FillPrices(prices2);
    }

    string RandomName()
    {
        return new string(Enumerable.Range(0, 8).Select(i => (char)('A' + random.Next(0, 26))).ToArray());
    }

    void FillPrices(IDictionary<Product, decimal> prices)
    {
        var memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: {0}", memory);
        for (int i = 0; i < 100000; i++)
        {
            prices.Add(new Product { ProductId = i, VendorName = RandomName() }, i);
        }
        memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: {0}", memory);
        prices.Clear();
        GC.Collect();
        memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: {0}", memory);
    }

    static void CompareDictionaries()
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
