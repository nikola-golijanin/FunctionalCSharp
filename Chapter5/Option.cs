using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace Chapter5;
   using static F;

   public static partial class F
   {
      // wrap the given value into a Some
      public static Option<T> Some<T>([NotNull] T? value) // NotNull: `value` is guaranteed to never be null if the method returns without throwing an exception
         => new(value ?? throw new ArgumentNullException(nameof(value)));

      // the None value
      public static NoneType None => default;
   }

   // a NoneType can be implicitely converted to an Option<T> for any type T
   public struct NoneType { }

   public struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
   {
      readonly T? value;
      readonly bool isSome;
      bool isNone => !isSome;

      internal Option(T t) => (isSome, value) = (true, t);

      public static implicit operator Option<T>(NoneType _) => default;

      public static implicit operator Option<T>(T t)
         => t is null ? None : new Option<T>(t);

      public R Match<R>(Func<R> None, Func<T, R> Some)
         => isSome ? Some(value!) : None();

      public IEnumerable<T> AsEnumerable()
      {
         if (isSome) yield return value!;
      }

      public static bool operator true(Option<T> @this) => @this.isSome;
      public static bool operator false(Option<T> @this) => @this.isNone;
      public static Option<T> operator |(Option<T> l, Option<T> r) => l.isSome ? l : r;

      // equality operators

      public bool Equals(Option<T> other)
         => this.isSome == other.isSome
         && (this.isNone || this.value!.Equals(other.value));

      public bool Equals(NoneType _) => isNone;

      public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
      public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

      public override bool Equals(object? other)
         => other is Option<T> option && this.Equals(option);

      public override int GetHashCode()
         => isNone ? 0 : value!.GetHashCode();

      public override string ToString() => isSome ? $"Some({value})" : "None";
   }

   public static class OptionExt
   {
      public static Option<R> Apply<T, R>
         (this Option<Func<T, R>> @this, Option<T> arg)
         => @this.Match(
            () => None,
            (func) => arg.Match(
               () => None,
               (val) => Some(func(val))));



      public static Option<R> Bind<T, R>
         (this Option<T> optT, Func<T, Option<R>> f)
          => optT.Match(
             () => None,
             (t) => f(t));


      public static Option<R> Map<T, R>
         (this NoneType _, Func<T, R> f)
         => None;

      public static Option<R> Map<T, R>
         (this Option<T> optT, Func<T, R> f)
         => optT.Match(
            () => None,
            (t) => Some(f(t)));


   }
