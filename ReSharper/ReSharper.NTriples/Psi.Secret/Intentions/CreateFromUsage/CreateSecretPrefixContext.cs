// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreateSecretPrefixContext.cs
// </summary>
// ***********************************************************************

using System.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.CreateDeclaration;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    public class CreateSecretPrefixContext : CreateContextBase
    {
        private readonly ITreeNode myAnchor;
        private readonly IDocument myDocument;
        private readonly CreateSecretPrefixTarget myTarget;

        public CreateSecretPrefixContext(CreateSecretPrefixTarget target)
        {
            this.myTarget = target;
            this.myAnchor = target.Anchor;
            Debug.Assert(this.myAnchor != null, "myAnchor != null");
            var psiSourceFile = this.myAnchor.GetSourceFile();
            Debug.Assert(psiSourceFile != null, "psiSourceFile != null");
            this.myDocument = psiSourceFile.Document;
        }

        public ITreeNode Anchor
        {
            get
            {
                return this.myAnchor;
            }
        }

        public ISentence Declaration
        {
            get
            {
                return this.myTarget.Declaration;
            }
        }

        public IDocument Document
        {
            get
            {
                return this.myDocument;
            }
        }

        public ICreationTarget Target
        {
            get
            {
                return this.myTarget;
            }
        }
    }
}
