namespace Twiddler.Tests.Screens
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using Moq;

    using ReactiveUI;

    #endregion

    public static class ReactiveObjectMockHelpers
    {
        public static void PropertyChanges<T, TValue>(this Mock<T> fake, 
                                                      Expression<Func<T, TValue>> expression, 
                                                      TValue newValue)
            where T : class, IReactiveNotifyPropertyChanged
        {
            string propertyName = GetPropertyName(expression);
            T sender = fake.Object;

            PublishChange(sender, sender.Changing, propertyName, newValue);

            UpdatePropertyValue(expression, fake, newValue);
            RaisePropertyChanged(fake, propertyName);

            PublishChange(sender, sender.Changed, propertyName, newValue);
        }

        public static void SetupReactiveObject<T>(this Mock<T> fake) where T : class, IReactiveNotifyPropertyChanged
        {
            SetupChangeSubject(fake, x => x.Changing);
            SetupChangeSubject(fake, x => x.Changed);
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

        private static void RaisePropertyChanged<T>(Mock<T> fake, string propertyName)
            where T : class, INotifyPropertyChanged
        {
            fake.Raise(x => x.PropertyChanged += null, 
                       new PropertyChangedEventArgs(propertyName));
        }

        private static void SetupChangeSubject<T>(Mock<T> fake, 
                                                  Expression<Func<T, IObservable<IObservedChange<object, object>>>> expression)
            where T : class
        {
            fake.
                Setup(expression).
                Returns(new Subject<IObservedChange<object, object>>());
        }

        private static void UpdatePropertyValue<T, TValue>(Expression<Func<T, TValue>> property, 
                                                           Mock<T> fake, 
                                                           TValue newValue)
            where T : class
        {
            fake.
                Setup(property).
                Returns(newValue);
        }
    }
}