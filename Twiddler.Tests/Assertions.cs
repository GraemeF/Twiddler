namespace Twiddler.Tests
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using ReactiveUI.Testing;

    using Should.Fluent;

    #endregion

    public static class Assertions
    {
        public static void AssertThatChangeNotificationIsRaisedBy<T, K>(this T propertyOwner, 
                                                                        Expression<Func<T, K>> property, 
                                                                        Action action)
            where T : INotifyPropertyChanged
        {
            bool changed = false;
            propertyOwner.PropertyChanged +=
                (sender, args) => changed |= PropertyChangeIsForProperty(args, property);

            action();

            changed.Should().Be.True();
        }

        private static bool PropertyChangeIsForProperty<T, K>(PropertyChangedEventArgs args, 
                                                              Expression<Func<T, K>> property)
        {
            return args.PropertyName == property.GetPropertyName();
        }
    }
}