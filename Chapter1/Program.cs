namespace Chapter1;

public static class ExtensionMethods
{
    public static (string Base, string Quote) SplitAt(this string s, int at)
        => (s.Substring(0, at), s.Substring(at));

    public static (string Base, string Quote) AsPair(this string pair)
        => pair.SplitAt(3);

    public static (IEnumerable<TSource> ConditionTrueCollection, IEnumerable<TSource> ConditionFalseCollection) Partition<TSource>(
        this IEnumerable<TSource> ints, Func<TSource, bool> filter)
    {
        var enumerable = ints.ToList();
        return (enumerable.Where(filter), enumerable.Where(i => !filter(i)));
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        //Two ways to split Currency string
        var pair1 = "EURUSD".SplitAt(3);
        Console.WriteLine(pair1.Base);
        Console.WriteLine(pair1.Quote);

        var pair2 = "EURUSD".AsPair();
        Console.WriteLine(pair2.Base);
        Console.WriteLine(pair2.Quote);

        //create two collections of this range, one from numbers that completes this condition, other one from numbers that does not complete condition
        //partition method used for that
        var partition = Enumerable.Range(0, 10).Partition(i => i % 3 == 0);
        foreach (var num in partition.ConditionTrueCollection)
        {
            Console.WriteLine(num);
        }

        foreach (var num in partition.ConditionFalseCollection)
        {
            Console.WriteLine(num);
        }
        
        
        //VAT Calculator example
        var address = new Address("de");
        var product = new Product("AAA", 12.5m, true);
        var order = new Order(product, 2);

        Console.WriteLine(VATCalculator.Vat(address, order));
    }
}