namespace dictionaries;

record struct Product(int ProductId, string VendorName) : IComparable<Product>
{
    public int CompareTo(Product other) {
        var cmp = ProductId.CompareTo(other.ProductId);
        if (cmp != 0) return cmp;
        return string.Compare(VendorName, other.VendorName, StringComparison.Ordinal);
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
        CompareDictionaries();
        var program = new Program();
        program.FillDictionaries();   
    }

    void FillDictionaries()
    {
        var prices = new Dictionary<Product, decimal>();
        FillPrices(prices);
        prices = null;
        GC.Collect();
        var prices2 = new SortedDictionary<Product, decimal>();
        FillPrices(prices2);
    }

    string RandomName()
    {
        return new string(Enumerable.Range(0, 8).Select(i => (char)('A' + random.Next(0, 26))).ToArray());
    }

    void FillPrices(IDictionary<Product, decimal> prices)
    {
        var now = DateTime.Now;
        var startingMemory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: {0}", startingMemory);
        for (int i = 0; i < 100000; i++)
        {
            prices.Add(new Product { ProductId = random.Next(1, 1001), VendorName = RandomName() }, i);
        }
        var memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: +{0}", memory - startingMemory);
        prices.Clear();
        GC.Collect();
        memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: +{0}", memory - startingMemory);
        var elapsed = DateTime.Now - now;
        Console.WriteLine("elapsed: {0}", elapsed);
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
