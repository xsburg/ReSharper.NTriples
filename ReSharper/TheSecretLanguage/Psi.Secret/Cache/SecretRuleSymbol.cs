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
    public class SecretRuleSymbol : ISecretSymbol
    {
        private readonly IPsiSourceFile myPsiSourceFile;
        private string myName;
        private int myOffset;

        public SecretRuleSymbol(IPsiSourceFile psiSourceFile)
        {
            this.myPsiSourceFile = psiSourceFile;
        }

        public SecretRuleSymbol(string name, int offset, IPsiSourceFile psiSourceFile)
        {
            this.myName = name;
            this.myOffset = offset;
            this.myPsiSourceFile = psiSourceFile;
        }

        public string Name
        {
            get
            {
                return this.myName;
            }
        }

        public int Offset
        {
            get
            {
                return this.myOffset;
            }
        }

        public IPsiSourceFile SourceFile
        {
            get
            {
                return this.myPsiSourceFile;
            }
        }

        public void Read(BinaryReader reader)
        {
            this.myName = reader.ReadString();
            this.myOffset = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Name);
            writer.Write(this.Offset);
        }
    }
}
