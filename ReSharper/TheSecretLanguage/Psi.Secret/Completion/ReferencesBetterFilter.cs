﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Lookup;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    internal class ReferencesBetterFilter : ILookupItemsPreference
    {
        public IEnumerable<ILookupItem> FilterItems(ICollection<ILookupItem> items)
        {
            return items.Where(i => i is DeclaredElementLookupItem);
        }

        public int Order
        {
            get
            {
                return 0;
            }
        }
    }
}