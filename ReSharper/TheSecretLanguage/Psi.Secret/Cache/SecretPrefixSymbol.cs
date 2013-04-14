// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretOptionSymbol.cs
// </summary>
// ***********************************************************************

using System.IO;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    public class SecretPrefixSymbol : SecretSymbolBase, ISecretSymbol
    {
        public SecretPrefixSymbol(IPsiSourceFile psiSourceFile) : base(null, 0, psiSourceFile)
        {
        }

        public SecretPrefixSymbol(string name, int offset, string uri, bool isStandard, IPsiSourceFile psiSourceFile) : base(name, offset, psiSourceFile)
        {
            this.Uri = uri;
            this.IsStandard = isStandard;
        }

        public string Uri { get; private set; }

        public bool IsStandard { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.Uri = reader.ReadString();
            this.IsStandard = reader.ReadBoolean();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(this.Uri);
            writer.Write(this.IsStandard);
        }
    }
}
