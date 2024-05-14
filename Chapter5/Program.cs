using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Chapter5;

public static class Solutions
{
    // 1 Write a generic function that takes a string and parses it as a value of an enum. It
    // should be usable as follows:

    // OptHelper.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
    // OptHelper.Parse<DayOfWeek>("Freeday") // => None





    // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
    // return the first element in the IEnumerable that matches the predicate, or None
    // if no matching element is found. Write its signature in arrow notation:

    // bool isOdd(int i) => i % 2 == 1;
    // new List<int>().Lookup(isOdd) // => None
    // new List<int> { 1 }.Lookup(isOdd) // => Some(1)

    // Lookup : IEnumerable<T> -> (T -> bool) -> Option<T>
    // solution: see OptHelper.Lookup




    // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
    // format. Ensure that you include the following:
    // - A smart constructor
    // - Implicit conversion to string, so that it can easily be used with the typical API
    // for sending emails

    public class Email
    {
        static readonly Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        private string Value { get; }

        private Email(string value) => Value = value;

        public static Option<Email> Create(string s)
           => regex.IsMatch(s)
              ? F.Some(new Email(s))
              : F.None;

        public static implicit operator string(Email e)
           => e.Value;
    }
}

public static partial class OptHelper
{
    public static Option<T> Some<T>(T value) => new Option<T>(value);
    public static NoneType None => default;

    public static Option<int> Parse(string s)
        => int.TryParse(s, out int result) ?
            Some(result) :
            None;

    public static Option<string> Lookup(this NameValueCollection collection, string key)
        => collection[key];

    public static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
        => dict.TryGetValue(key, out T value) ? Some(value) : None;


    public static Option<T> Lookup<T>(this IEnumerable<T> collection, Func<T, bool> _filter)
    {
        foreach (var item in collection)
        {
            if (_filter(item))
                return Some(item);
        };

        return None;
    }
}


public static class EnumHelper
{
    public static Option<T> Parse<T>(this string s) where T : struct
       => Enum.TryParse(s, out T t) ? F.Some(t) : F.None;
}