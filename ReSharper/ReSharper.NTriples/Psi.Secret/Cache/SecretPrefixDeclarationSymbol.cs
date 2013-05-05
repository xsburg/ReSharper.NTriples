// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretPrefixDeclarationSymbol.cs
// </summary>
// ***********************************************************************

using System.IO;
using JetBrains.ReSharper.Psi;

namespace ReSharper.NTriples.Cache
{
    public class SecretPrefixDeclarationSymbol : SecretSymbolBase
    {
        public SecretPrefixDeclarationSymbol()
        {
        }

        public SecretPrefixDeclarationSymbol(string uri, string name, int offset, IPsiSourceFile psiSourceFile)
            : base(name, offset, psiSourceFile)
        {
            this.Uri = uri;
        }

        public string Uri { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.Uri = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(this.Uri);
        }
    }
}
