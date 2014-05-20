using System;
using System.Linq.Expressions;

namespace InjectionMap.Mapping
{
    internal class MappingComponent : Component, IMappingComponent
    {
        internal MappingComponent()
            : this(Guid.NewGuid())
        {
        }

        internal MappingComponent(Guid id)
            : base(id)
        {
        }

        #region Properties

        /// <summary>
        /// The predicate that gets executed to provide the value for the mapping
        /// </summary>
        public Expression<Func<object>> ValueCallback { get; set; }

        public Action<object> OnResolvedCallback { get; internal set; }

        public Type ValueType { get; internal set; }

        #endregion

        #region Overrides

        protected override Type GetValueType()
        {
            return ValueType;
        }

        #endregion

        #region IDisposeable Implementation

        public override void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    ValueCallback = null;
                    OnResolvedCallback = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    internal class MappingComponent<T> : Component, IMappingComponent
    {
        internal MappingComponent()
            : this(Guid.NewGuid())
        {
        }

        internal MappingComponent(Guid id) : base(id)
        {
        }

        #region Properties

        /// <summary>
        /// The predicate that gets executed to provide the value for the mapping
        /// </summary>
        public Expression<Func<T>> ValueCallback { get; set; }

        /// <summary>
        /// The predicate that gets executed to provide the value for the mapping
        /// </summary>
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

        #endregion

        #region Overrides

        protected override Type GetValueType()
        {
            return ValueType;
        }

        #endregion

        #region IDisposeable Implementation

        public override void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    ValueCallback = null;
                    OnResolvedCallback = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
