using System;
using System.Collections.Generic;

namespace InjectionMap.Internal
{
    internal abstract class Component : IComponent
    {
        internal Component(Guid id)
        {
            ID = id;
            MappingConfiguration = new MappingConfiguration();
        }

        #region Properties

        public Guid ID { get; private set; }

        public Type KeyType { get; internal set; }

        Type IComponent.ValueType
        {
            get
            {
                return GetValueType();
            }
        }

        public IMappingConfiguration MappingConfiguration { get; internal set; }

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

        #endregion

        #region Implementation

        protected abstract Type GetValueType();

        #endregion

        #region IDisposeable Implementation

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~Component()
        {
            Dispose(false);
        }

        #endregion
    }
}
