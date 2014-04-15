using System;

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

        public Type Key { get; internal set; }

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

        public Type ValueType { get; internal set; }

        public IMappingOption MappingOption { get; internal set; }
    }
}
