// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesUriIdentifierSymbol.cs
// </summary>
// ***********************************************************************

using System.IO;
using JetBrains.ReSharper.Psi;

namespace ReSharper.NTriples.Cache
{
    public class NTriplesUriIdentifierSymbol : NTriplesSymbolBase
    {
        public NTriplesUriIdentifierSymbol()
        {
        }

        public NTriplesUriIdentifierSymbol(
            string @namespace, string localName, IdentifierInfo info, int offset, IPsiSourceFile psiSourceFile)
            : base(@namespace + localName, offset, psiSourceFile)
        {
            this.LocalName = localName;
            this.Info = info;
            this.Namespace = @namespace;
        }

        public IdentifierInfo Info { get; private set; }
        public string LocalName { get; private set; }
        public string Namespace { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.Namespace = reader.ReadString();
            this.LocalName = reader.ReadString();
            this.Info = IdentifierInfo.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(this.Namespace);
            writer.Write(this.LocalName);
            this.Info.Write(writer);
        }
    }
}
