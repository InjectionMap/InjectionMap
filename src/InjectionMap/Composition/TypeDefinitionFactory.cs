using InjectionMap.Extensions;
using InjectionMap.Tracing;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace InjectionMap.Composition
{
    public delegate void PropertySetterDelegate(object instance, object value);

    /// <summary>
    /// Helper class for properties
    /// </summary>
    public class TypeDefinitionFactory
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

        /// <summary>
        /// Extracts the propertyinfo from a expression
        /// </summary>
        /// <typeparam name="T">The type containing the property</typeparam>
        /// <param name="propertyExpression">The expression defining the property</param>
        /// <returns>The propertyinfo of the property</returns>
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
                    Logger.Write(string.Format("InjectionMap - Cannot extract Property from expression. Expression is not a MemberAccess Expression: {0}", propertyExpression), LogLevel.Error, "TypeDefinitionFactory", "Mapping");
                    throw new ArgumentException(string.Format("Cannot extract Property from expression. Expression is not a MemberAccess Expression: {0}", propertyExpression), "propertyExpression");
                }
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                Logger.Write(string.Format("InjectionMap - Cannot extract Property from expression. Expression is not a PropertyInfo: {0}", propertyExpression), LogLevel.Warning, "TypeDefinitionFactory", "Mapping");
            }

            return propertyInfo;
        }

        /// <summary>
        /// Creates a delegate that can be used to set a value to  a property
        /// </summary>
        /// <param name="propertyInfo">The propertyinfo of the property</param>
        /// <returns>A delegate to set the value</returns>
        public PropertySetterDelegate GetPropertySetter(PropertyInfo propertyInfo)
        {
            var propertySetMethod = propertyInfo.GetSetMethod();
            if (propertySetMethod == null)
            {
                Logger.Write(string.Format("InjectionMap - Property has no setter {0}", propertyInfo.Name), LogLevel.Warning, "TypeDefinitionFactory", "Mapping");
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
