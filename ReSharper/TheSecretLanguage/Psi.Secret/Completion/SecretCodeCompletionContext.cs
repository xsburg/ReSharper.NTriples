// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeCompletionContext.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    public class SecretCodeCompletionContext : SpecificCodeCompletionContext
    {
        public SecretCodeCompletionContext(
            CodeCompletionContext context, TextLookupRanges completionRanges, SecretReparsedCompletionContext reparsedContext)
            : base(context)
        {
            this.ReparsedContext = reparsedContext;
            this.Ranges = completionRanges;
        }

        public override string ContextId
        {
            get
            {
                return "PsiSpecificContext";
            }
        }

        public TextLookupRanges Ranges { get; private set; }

        public SecretReparsedCompletionContext ReparsedContext { get; private set; }
    }
}
