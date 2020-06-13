// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Eventing.Abstractions
{
    using System;

    /// <summary>
    /// A wrapper object for ephemeral disposables.
    /// </summary>
    internal class ActionDisposable : IDisposable
    {
        private readonly Action onDispose;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisposable"/> class.
        /// </summary>
        /// <param name="onDispose">Action used for dispose.</param>
        public ActionDisposable(Action onDispose)
        {
            this.onDispose = onDispose;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <param name="disposing">Indicate if the object is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.onDispose?.Invoke();
                }

                this.disposedValue = true;
            }
        }
    }
}
