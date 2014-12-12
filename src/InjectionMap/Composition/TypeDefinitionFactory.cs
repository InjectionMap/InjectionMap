using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using InjectionMap.Tracing;
using InjectionMap.Extensions;

namespace InjectionMap.Composition
{
    internal delegate void PropertySetterDelegate(object instance, object value);

    internal class TypeDefinitionFactory
    {
        internal Lazy<ILoggerFactory> LoggerFactory { get; private set; }

        internal ILogger Logger
        {
            get
            {
                return LoggerFactory.Value.CreateLogger();
            }
        }

        public TypeDefinitionFactory()
        {
            LoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory());
        }

        public PropertyInfo ExtractProperty<T>(Expression<Func<T, object>> propertyExpression)
        {
            propertyExpression.EnsureArgumentNotNull("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                // try get the member from the operand of the unaryexpression
                var unary = propertyExpression.Body as UnaryExpression;
                if (unary != null)
                    memberExpression = unary.Operand as MemberExpression;

                if (memberExpression == null)
                {
                    var binary = propertyExpression.Body as BinaryExpression;
                    if (binary != null)
                        memberExpression = binary.Left as MemberExpression;
                }

                if (memberExpression == null)
                {
                    Logger.Write(string.Format("InjectionMap - Cannot extract Property from expression. Expression is not a MemberAccess Expression: {0}", propertyExpression), "TypeDefinitionFactory", "Mapping");
                    throw new ArgumentException(string.Format("Cannot extract Property from expression. Expression is not a MemberAccess Expression: {0}", propertyExpression), "propertyExpression");
                }
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                Logger.Write(string.Format("InjectionMap - Cannot extract Property from expression. Expression is not a PropertyInfo: {0}", propertyExpression), "TypeDefinitionFactory", "Mapping");
            }

            return propertyInfo;
        }

        public PropertySetterDelegate GetPropertySetter(PropertyInfo propertyInfo)
        {
            var propertySetMethod = propertyInfo.GetSetMethod();
            if (propertySetMethod == null)
            {
                Logger.Write(string.Format("InjectionMap - Property has no setter {0}", propertyInfo.Name), "TypeDefinitionFactory", "Mapping");
                return null;
            }

            var instance = Expression.Parameter(typeof(object), "i");
            var argument = Expression.Parameter(typeof(object), "a");

            var instanceParam = Expression.Convert(instance, propertyInfo.DeclaringType);
            var valueParam = Expression.Convert(argument, propertyInfo.PropertyType);

            var setterCall = Expression.Call(instanceParam, propertyInfo.GetSetMethod(), valueParam);

            return Expression.Lambda<PropertySetterDelegate>(setterCall, instance, argument).Compile();
        }
    }
}
