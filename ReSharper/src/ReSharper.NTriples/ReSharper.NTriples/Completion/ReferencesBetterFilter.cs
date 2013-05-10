// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ReferencesBetterFilter.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Lookup;

namespace ReSharper.NTriples.Completion
{
    internal class ReferencesBetterFilter : ILookupItemsPreference
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        public IEnumerable<ILookupItem> FilterItems(ICollection<ILookupItem> items)
        {
            return items.Where(i => i is DeclaredElementLookupItem);
        }
    }
}
