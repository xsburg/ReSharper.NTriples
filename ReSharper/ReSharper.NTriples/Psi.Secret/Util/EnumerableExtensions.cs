// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   EnumerableExtensions.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace ReSharper.NTriples.Util
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that applies an operation to each item in
        ///     this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The collection to act on.</param>
        /// <param name="action">The action.</param>
        public static void Apply<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        ///     An <see cref="IEnumerable" /> extension method that applies an operation to each item in
        ///     collection.
        /// </summary>
        /// <param name="source">The collection to act on.</param>
        /// <param name="action">The action.</param>
        public static void Apply(this IEnumerable source, Action<object> action)
        {
            foreach (object item in source)
            {
                action(item);
            }
        }
    }
}
