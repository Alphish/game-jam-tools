using System;
using System.Linq.Expressions;
using System.Windows;

namespace Alphicsh.JamTools.Common.Controls
{
    public class DependencyPropertyHelper<TOwner>
        where TOwner : DependencyObject
    {
        // -------------------
        // Simple registration
        // -------------------

        public DependencyProperty Register<TValue>(Expression<Func<TOwner, TValue>> propertyExpression)
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner));
        }

        public DependencyProperty Register<TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            PropertyMetadata metadata
            )
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner), metadata);
        }

        public DependencyProperty Register<TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue
            )
        {
            return Register(propertyExpression, new PropertyMetadata(defaultValue));
        }

        public DependencyProperty Register<TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue,
            PropertyChangedCallback onPropertyChanged
            )
        {
            return Register(propertyExpression, new PropertyMetadata(defaultValue, onPropertyChanged));
        }

        // ---------------------
        // Readonly registration
        // ---------------------

        public DependencyPropertyKey RegisterReadOnly<TValue>(
            Expression<Func<TOwner, TValue>> propertyExpression,
            TValue defaultValue
            )
        {
            var propertyName = ExpressionNameof(propertyExpression);
            return DependencyProperty.RegisterReadOnly(propertyName, typeof(TValue), typeof(TOwner), new PropertyMetadata(defaultValue));
        }

        // --------------
        // Helper methods
        // --------------

        private string ExpressionNameof<TValue>(Expression<Func<TOwner, TValue>> propertyExpression)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            return memberExpression.Member.Name;
        }
    }
}
