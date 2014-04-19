using System;
using System.Collections.Generic;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingComponent<T> : IMappingComponent
    {
        internal MappingComponent()
            : this(Guid.NewGuid())
        {
        }

        internal MappingComponent(Guid id)
        {
            ID = id;
            MappingOption = new MappingOption();
        }

        public Guid ID { get; private set; }

        public Type KeyType { get; internal set; }

        public T Value { get; set; }

        object IMappingComponent.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (T)value;
            }
        }

        public Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }

        public IMappingOption MappingOption { get; internal set; }

        IList<IBindingArgument> _arguments;
        public IList<IBindingArgument> Arguments
        {
            get
            {
                if (_arguments == null)
                    _arguments = new List<IBindingArgument>();
                return _arguments;
            }
        }
    }
}
