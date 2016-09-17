﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace OneBlog.Core.Providers
{
    /// <summary>
    /// A collection class for BlogFileSystemProviders.
    /// </summary>
    public class BlogFileSystemProviderCollection : ProviderCollection
    {
        #region Indexers

        /// <summary>
        ///     Gets a provider by its name.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        public new BlogFileSystemProvider this[string name]
        {
            get
            {
                return (BlogFileSystemProvider)base[name];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a provider to the collection.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (!(provider is BlogFileSystemProvider))
            {
                throw new ArgumentException("Invalid provider type", "provider");
            }

            base.Add(provider);
        }

        #endregion
    }
}
