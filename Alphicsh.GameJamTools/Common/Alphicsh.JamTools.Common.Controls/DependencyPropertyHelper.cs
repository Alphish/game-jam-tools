using System;
using System.Linq.Expressions;
using System.Windows;

namespace Alphicsh.JamTools.Common.Controls
{
    public static class DependencyPropertyHelper
    {
        // -------------------
        // Simple registration
        // -------------------

        public static DependencyProperty Register<TOwner, TValue>(Expression<Func<TOwner, TValue>> propertyExpression)
            where TOwner : DependencyObject
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner));
        }

        public static DependencyProperty Register<TOwner, TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            PropertyMetadata metadata
            )
            where TOwner : DependencyObject
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner), metadata);
        }

        public static DependencyProperty Register<TOwner, TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue
            )
            where TOwner : DependencyObject
        {
            return Register(propertyExpression, new PropertyMetadata(defaultValue));
        }

        public static DependencyProperty Register<TOwner, TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue,
            PropertyChangedCallback onPropertyChanged
            )
            where TOwner : DependencyObject
        {
            return Register(propertyExpression, new PropertyMetadata(defaultValue, onPropertyChanged));
        }

        // ---------------------
        // Readonly registration
        // ---------------------

        public static DependencyPropertyKey RegisterReadOnly<TOwner, TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue
            )
            where TOwner : DependencyObject
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.RegisterReadOnly(propertyName, typeof(TValue), typeof(TOwner), new PropertyMetadata(defaultValue));
        }

        // --------------
        // Helper methods
        // --------------

        private static string ExpressionNameof<TOwner, TValue>(Expression<Func<TOwner, TValue>> propertyExpression)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            return memberExpression.Member.Name;
        }
    }
}
