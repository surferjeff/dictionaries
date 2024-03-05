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

    static void PrintPrices(IDictionary<Product, decimal> prices)
    {
        foreach (var price in prices)
        {
            Console.Write(price.Key.ProductId);
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("============ Dictionary ============");
        CompareDictionaries(() => new Dictionary<Product, decimal>());
        Console.WriteLine("============ SortedDictionary ============");
        CompareDictionaries(() => new SortedDictionary<Product, decimal>());
        var program = new Program();
        Console.WriteLine("============ FillDictionaries ============");
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
        var startingMemory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: {0}", startingMemory);
        var now = DateTime.Now;
        for (int i = 0; i < 100000; i++)
        {
            prices.Add(new Product { ProductId = i, VendorName = RandomName() }, i);
        }
        var memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: +{0}", memory - startingMemory);
        prices.Clear();
        var elapsed = DateTime.Now - now;
        GC.Collect();
        memory = GC.GetTotalMemory(true) / 1024;
        Console.WriteLine("memory: +{0}", memory - startingMemory);
        Console.WriteLine("elapsed: {0}", elapsed);
    }

    static void Fill12(IDictionary<Product, decimal> prices)
    {
        prices.Add(new Product { ProductId = 1, VendorName = "Contoso" }, 100m);
        prices.Add(new Product { ProductId = 2, VendorName = "Fabrikam" }, 150m);
    }

    static void Fill21(IDictionary<Product, decimal> prices)
    {
        prices.Add(new Product { ProductId = 2, VendorName = "Fabrikam" }, 150m);
        prices.Add(new Product { ProductId = 1, VendorName = "Contoso" }, 100m);
    }

    static void ComparePrices(IDictionary<Product, decimal> a, IDictionary<Product, decimal> b)
    {
        Console.Write("Comparing ");
        PrintPrices(a);
        Console.Write(" to ");
        PrintPrices(b);
        Console.WriteLine();

        Console.WriteLine(".Equals() {0}", a.Equals(b));
        Console.WriteLine(".SequenceEquals() {0}", a.SequenceEqual(b));
        Console.WriteLine(".SequenceEquals(sorted) {0}",
            a.OrderBy(p => p.Key.ProductId).SequenceEqual(b.OrderBy(p => p.Key.ProductId)));
    }

    static void CompareDictionaries(Func<IDictionary<Product, decimal> > newDictionary) {
        var a = newDictionary();
        Fill12(a);
        var b = newDictionary();
        Fill21(b);
        var c = newDictionary();
        Fill12(c);
        ComparePrices(a, b);
        ComparePrices(a, b);
        ComparePrices(a, c);
    }
}
