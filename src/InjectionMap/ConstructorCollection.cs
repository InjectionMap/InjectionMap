using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InjectionMap
{
    /// <summary>
    /// Class that is used to define and select constructors when creating a map or resolving.
    /// </summary>
    public class ConstructorCollection : IEnumerable<ConstructorDefinition>
    {
        private readonly List<ConstructorDefinition> _items = new List<ConstructorDefinition>();

        public ConstructorCollection()
        {
        }

        /// <summary>
        /// Gets a constructordefinition by index
        /// </summary>
        /// <param name="id">The index of the constructor</param>
        /// <returns>The constructordefinition of the constructor</returns>
        public ConstructorDefinition this[int id]
        {
            get
            {
                return _items[id];
            }
        }

        /// <summary>
        /// Adds a constructordefinition to the collection
        /// </summary>
        /// <param name="definition">The constructordefinition</param>
        internal void Add(ConstructorDefinition definition)
        {
            _items.Add(definition);
        }

        public IEnumerator<ConstructorDefinition> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents a constructor with all arguments/parameters of a object
    /// </summary>
    public interface IConstructorDefinition
    {
        // TODO: hide this? don't show to public?
        /// <summary>
        /// The Constructorinfo
        /// </summary>
        ConstructorInfo ConstructorInfo { get; }
    }

    /// <summary>
    /// Represents a constructor with all arguments/parameters of a object
    /// </summary>
    public class ConstructorDefinition : IEnumerable<Argument>, IConstructorDefinition
    {
        private readonly List<Argument> _items = new List<Argument>();

        public ConstructorDefinition()
        {
        }

        /// <summary>
        /// Gets the arguments by index
        /// </summary>
        /// <param name="id">The index of the argument</param>
        /// <returns>Constructorargument</returns>
        public Argument this[int id]
        {
            get
            {
                return _items[id];
            }
        }

        /// <summary>
        /// Gets the arguments by index
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>Constructorargument</returns>
        public Argument this[string name]
        {
            get
            {
                return _items.FirstOrDefault(itm => itm.Name == name);
            }
        }

        /// <summary>
        /// Adds a contstructorargument to the collection
        /// </summary>
        /// <param name="argument">The argument for the collection</param>
        internal void Add(Argument argument)
        {
            _items.Add(argument);
        }

        // TODO: hide this? don't show to public?
        /// <summary>
        /// The Constructorinfo
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; set; }

        public IEnumerator<Argument> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents a argument/parameter of a constructor
    /// </summary>
    public class Argument : IArgument
    {
        public Argument()
        {
        }

        public Argument(object value)
        {
            Value = value;
            Type = value.GetType();
        }

        public Argument(string name, object value)
        {
            Name = name;
            Value = value;
            Type = value.GetType();
        }

        public Argument(Type type)
        {
            Type = type;
        }

        public Argument(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; internal set; }

        object _value;

        /// <summary>
        /// The value that is passed to the constructor
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (value != null && Type != null && value.GetType() != Type)
                    throw new ArgumentException(string.Format("Value passed to the constructor argument does not match the type defined in the constructor. Value is of type {0} but expected was Type {1}", value.GetType(), Type));
            }
        }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        public Type Type { get; internal set; }
    }
}
