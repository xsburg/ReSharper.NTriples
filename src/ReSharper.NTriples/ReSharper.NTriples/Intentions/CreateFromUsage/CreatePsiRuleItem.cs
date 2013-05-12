// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreatePsiRuleItem.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Intentions.CreateFromUsage;
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    public class CreatePsiRuleItem : CreateFromUsageItemBase<CreateNTriplesPrefixContext>, IPartBulbItem
    {
        private readonly string myFormatText;

        public CreatePsiRuleItem(JetBrains.Util.Lazy.Lazy<CreateNTriplesPrefixContext> context, string format)
            : base(context)
        {
            this.myFormatText = format;
        }

        public override string Text
        {
            get
            {
                return this.myFormatText;
            }
        }

        protected override IntentionResult ExecuteIntention()
        {
            return null;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var result = this.ExecutePsiIntention();

            Assertion.Assert(result.ResultDeclaration != null, "result.ResultDeclaration != null");
            Assertion.Assert(result.ResultDeclaration.IsValid(), "result.ResultDeclaration.IsValid()");
            result.ResultDeclaration.GetPsiServices().PsiManager.UpdateCaches();
            var postExecute = this.GetContext().Target as ITargetPostExecute;
            if (result.ResultDeclaration.DeclaredElement != null && postExecute != null)
            {
                postExecute.PostExecute(result.ResultDeclaration.DeclaredElement);
            }

            return tmp => result.ExecuteTemplate();
        }

        private NTriplesIntentionResult ExecutePsiIntention()
        {
            return LanguageManager.Instance.GetService<ICreateNTriplesPrefixIntention>(
                this.GetContext().Target.GetTargetDeclaration().Language).ExecuteEx(this.GetContext());
        }
    }
}
