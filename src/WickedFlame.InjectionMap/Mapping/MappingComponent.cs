using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public Expression<Func<T>> ValueCallback { get; set; }

        Expression<Func<object>> IMappingComponent.ValueCallback
        {
            get
            {
                if (ValueCallback == null)
                    return null;

                return () => ValueCallback.Compile().Invoke();
            }
            set
            {
                ValueCallback = () => (T)value.Compile().Invoke();
            }
        }

        public Action<T> OnResolvedCallback { get; internal set; }

        Action<object> IMappingComponent.OnResolvedCallback
        {
            get
            {
                if (OnResolvedCallback == null)
                    return null;

                return p => OnResolvedCallback((T)p);
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

        public bool IsSubstitute { get; internal set; }
    }
}
