// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretRuleSymbol.cs
// </summary>
// ***********************************************************************

using System.IO;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    public class SecretUriIdentifierSymbol : SecretSymbolBase
    {
        public SecretUriIdentifierSymbol()
        {
        }

        public SecretUriIdentifierSymbol(string @namespace, string localName, UriIdentifierKind kind, int offset, IPsiSourceFile psiSourceFile)
            : base(@namespace + localName, offset, psiSourceFile)
        {
            LocalName = localName;
            Kind = kind;
            Namespace = @namespace;
        }

        public string Namespace { get; private set; }

        public string LocalName { get; private set; }

        public UriIdentifierKind Kind { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.Namespace = reader.ReadString();
            this.LocalName = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(this.Namespace);
            writer.Write(this.LocalName);
        }
    }
}
