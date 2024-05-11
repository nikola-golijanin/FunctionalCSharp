using NUnit.Framework;

namespace Chapter2;

public static class Solutions
{
    // 1. Write a function that negates a given predicate: whenvever the given predicate
    // evaluates to `true`, the resulting function evaluates to `false`, and vice versa.
    public static Func<T, bool> Negate<T>(this Func<T, bool> pred)
       => t => !pred(t);

    // 2. Write a method that uses quicksort to sort a `List<int>` (return a new list,
    // rather than sorting it in place).
    public static List<int> QuickSort(this List<int> list)
    {
        if (list.Count == 0) return new List<int>();

        var pivot = list[0];
        var rest = list.Skip(1);

        var small = from item in rest where item <= pivot select item;
        var large = from item in rest where pivot < item select item;

        return small.ToList().QuickSort()
           .Append(pivot)
           .Concat(large.ToList().QuickSort())
           .ToList();
    }


    // 3. Generalize your implementation to take a `List<T>`, and additionally a 
    // `Comparison<T>` delegate.
    public static List<T> QuickSort<T>(this List<T> list, Comparison<T> compare)
    {
        if (list.Count == 0) return new List<T>();

        var pivot = list[0];
        var rest = list.Skip(1);

        var small = from item in rest where compare(item, pivot) <= 0 select item;
        var large = from item in rest where 0 < compare(item, pivot) select item;

        return small.ToList().QuickSort(compare)
           .Concat(new List<T> { pivot })
           .Concat(large.ToList().QuickSort(compare))
           .ToList();
    }


    // 4. In this chapter, you've seen a `Using` function that takes an `IDisposable`
    // and a function of type `Func<TDisp, R>`. Write an overload of `Using` that
    // takes a `Func<IDisposable>` as first
    // parameter, instead of the `IDisposable`. (This can be used to fix warnings
    // given by some code analysis tools about instantiating an `IDisposable` and
    // not disposing it.)
    public static R Using<TDisp, R>(Func<TDisp> createDisposable, Func<TDisp, R> func) where TDisp : IDisposable
    {
        using (var disp = createDisposable()) return func(disp);
    }

    [Test]
    public static void TestQuickSort()
    {
        var list = new List<int> { -100, 63, 30, 45, 1, 1000, -23, -67, 1, 2, 56, 75, 975, 432, -600, 193, 85, 12 };
        var expected = new List<int> { -600, -100, -67, -23, 1, 1, 2, 12, 30, 45, 56, 63, 75, 85, 193, 432, 975, 1000 };
        var actual = list.QuickSort();
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public static void TestQuickSortWithCompare()
    {
        var list = new List<int> { -100, 63, 30, 45, 1, 1000, -23, -67, 1, 2, 56, 75, 975, 432, -600, 193, 85, 12 };
        var expected = new List<int> { -600, -100, -67, -23, 1, 1, 2, 12, 30, 45, 56, 63, 75, 85, 193, 432, 975, 1000 };
        static int Compare(int r, int l)
        {
            if (r < l) return -1;
            if (r > l) return 1;
            return 0;
        }

        var actual = list.QuickSort(Compare);
        Assert.AreEqual(expected, actual);
    }
}