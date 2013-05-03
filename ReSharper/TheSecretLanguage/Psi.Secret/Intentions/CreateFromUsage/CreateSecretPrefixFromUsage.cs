// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreateSecretPrefixFromUsage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Intentions.CreateFromUsage;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.Util;
using JetBrains.Util.Lazy;

namespace JetBrains.ReSharper.Psi.Secret.Intentions.CreateFromUsage
{
    [QuickFix]
    internal class CreateSecretPrefixFromUsage
        : CreateFromUsageActionBase<ICreateSecretPrefixIntention, SecretPrefixReference>, IQuickFix
    {
        public CreateSecretPrefixFromUsage(SecretUnresolvedReferenceHighlighting<SecretPrefixReference> error)
            : base(error.Reference)
        {
        }

        public void CreateBulbItems(BulbMenu menu, Severity severity)
        {
            menu.ArrangeQuickFixes(this.Items.Select(_ => Pair.Of(_, severity)));
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return ((this.Reference != null) && (this.Reference.IsValid()));
        }

        protected override IEnumerable<IBulbAction> CreateBulbItems()
        {
            Debug.Assert(this.Reference != null, "Reference != null");
            yield return
                new CreatePsiRuleItem(Lazy.Of(this.GetContext), string.Format("Create prefix '{0}'", this.Reference.GetName()));
        }

        protected override ICreationTarget GetTarget()
        {
            return new CreateSecretPrefixTarget(this.Reference);
        }

        private CreateSecretPrefixContext GetContext()
        {
            return new CreateSecretPrefixContext(this.GetTarget() as CreateSecretPrefixTarget);
        }
    }
}
