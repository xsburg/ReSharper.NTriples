// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCodeCompletionContext.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;

namespace ReSharper.NTriples.Completion
{
    public class NTriplesCodeCompletionContext : SpecificCodeCompletionContext
    {
        public NTriplesCodeCompletionContext(
            CodeCompletionContext context, TextLookupRanges completionRanges, NTriplesReparsedCompletionContext reparsedContext)
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

        public NTriplesReparsedCompletionContext ReparsedContext { get; private set; }
    }
}
