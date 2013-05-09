// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordsBetterFilter.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;

namespace ReSharper.NTriples.Completion
{
    internal class KeywordsBetterFilter : ILookupItemsPreference
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
            return items.Where(x => x is NTriplesKeywordLookupItem || x is TemplateLookupItem);
        }
    }
}
