namespace Twiddler.Tests
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using NSubstitute;

    using ReactiveUI;

    #endregion

    public static class ReactiveObjectMockHelpers
    {
        public static void PropertyChanges<T, TValue>(this T substitute, 
                                                      Expression<Func<T, TValue>> expression, 
                                                      TValue newValue)
            where T : class, IReactiveNotifyPropertyChanged
        {
            string propertyName = GetPropertyName(expression);
            T sender = substitute;

            PublishChange(sender, sender.Changing, propertyName, newValue);

            UpdatePropertyValue(expression, substitute, newValue);
            RaisePropertyChanged(substitute, propertyName);

            PublishChange(sender, sender.Changed, propertyName, newValue);
        }

        public static T WithReactiveProperties<T>(this T substitute) where T : class, IReactiveNotifyPropertyChanged
        {
            substitute.Changing.Returns(new Subject<IObservedChange<object, object>>());
            substitute.Changed.Returns(new Subject<IObservedChange<object, object>>());

            return substitute;
        }

        private static string GetPropertyName<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        private static void PublishChange<T, TValue>(T sender, 
                                                     IObservable<IObservedChange<object, object>> observable, 
                                                     string propertyName, 
                                                     TValue newValue)
        {
            ((Subject<IObservedChange<object, object>>)observable).
                OnNext(new ObservedChange<object, object>
                           {
                               PropertyName = propertyName, 
                               Sender = sender, 
                               Value = newValue
                           });
        }

        private static void RaisePropertyChanged<T>(T substitute, string propertyName)
            where T : class, INotifyPropertyChanged
        {
            substitute.PropertyChanged +=
                Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs(propertyName));
        }

        private static void UpdatePropertyValue<T, TValue>(Expression<Func<T, TValue>> property, 
                                                           T substitute, 
                                                           TValue newValue)
            where T : class
        {
            property.Compile()(substitute).Returns(newValue);
        }
    }
}