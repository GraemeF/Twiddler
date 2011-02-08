namespace Twiddler.Core
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    #endregion

    public static class CollectionChangedExtensions
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
            return (items(args) ?? new T[] { }).Cast<T>();
        }
    }
}