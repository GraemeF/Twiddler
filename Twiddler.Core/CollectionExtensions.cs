using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Twiddler.Core
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> SafeNewItems<T>(this NotifyCollectionChangedEventArgs args)
        {
            return SafeItems<T>(args, x => x.NewItems);
        }

        public static IEnumerable<T> SafeOldItems<T>(this NotifyCollectionChangedEventArgs args)
        {
            return SafeItems<T>(args, x => x.OldItems);
        }

        private static IEnumerable<T> SafeItems<T>(NotifyCollectionChangedEventArgs args,
                                                   Func<NotifyCollectionChangedEventArgs, IList> items)
        {
            return (items(args) ?? new T[] {}).Cast<T>();
        }
    }
}