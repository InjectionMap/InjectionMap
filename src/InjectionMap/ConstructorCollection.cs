using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InjectionMap
{
    /// <summary>
    /// Class that is used to define and select constructors when creating a map or resolving
    /// </summary>
    public class ConstructorCollection : IEnumerable<ConstructorDefinition>
    {
        readonly List<ConstructorDefinition> _items = new List<ConstructorDefinition>();

        public ConstructorCollection()
        {
        }

        public ConstructorDefinition this[int id]
        {
            get
            {
                return _items[id];
            }
        }

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

    public interface IConstructorDefinition
    {
        //ConstructorInfo ConstructorInfo { get; }
    }

    public class ConstructorDefinition : IEnumerable<ConstructorArgument>, IConstructorDefinition
    {
        readonly List<ConstructorArgument> _items = new List<ConstructorArgument>();

        public ConstructorDefinition()
        {
        }

        public ConstructorArgument this[int id]
        {
            get
            {
                return _items[id];
            }
        }

        public ConstructorArgument this[string name]
        {
            get
            {
                return _items.FirstOrDefault(itm => itm.Name == name);
            }
        }

        internal void Add(ConstructorArgument argument)
        {
            _items.Add(argument);
        }

        internal ConstructorInfo ConstructorInfo { get; set; }

        public IEnumerator<ConstructorArgument> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    public class ConstructorArgument : IArgument
    {
        public string Name { get; internal set; }

        object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (value != null && value.GetType() != Type)
                    throw new ArgumentException(string.Format("Value passed to the constructor argument does not match the type defined in the constructor. Value is of type {0} but expected was Type {1}", value.GetType(), Type));
            }
        }

        public Type Type { get; internal set; }
    }
}
