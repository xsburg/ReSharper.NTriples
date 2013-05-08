// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretUriIdentifierSymbol.cs
// </summary>
// ***********************************************************************

using System.IO;
using JetBrains.ReSharper.Psi;

namespace ReSharper.NTriples.Cache
{
    public class SecretUriIdentifierSymbol : SecretSymbolBase
    {
        public SecretUriIdentifierSymbol()
        {
        }

        public SecretUriIdentifierSymbol(string @namespace, string localName, IdentifierKind kind, bool important, int offset, IPsiSourceFile psiSourceFile)
            : base(@namespace + localName, offset, psiSourceFile)
        {
            this.LocalName = localName;
            this.Kind = kind;
            this.Important = important;
            this.Namespace = @namespace;
        }

        public IdentifierKind Kind { get; private set; }
        public bool Important { get; private set; }
        public string LocalName { get; private set; }
        public string Namespace { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.Namespace = reader.ReadString();
            this.LocalName = reader.ReadString();
            this.Important = reader.ReadBoolean();
            this.Kind = (IdentifierKind)reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(this.Namespace);
            writer.Write(this.LocalName);
            writer.Write(this.Important);
            writer.Write((int)this.Kind);
        }
    }
}
