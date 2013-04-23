using System.IO;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    public abstract class SecretSymbolBase : ISecretSymbol
    {
        public string Name { get; private set; }
        public int Offset { get; private set; }
        public IPsiSourceFile SourceFile { get; private set; }

        protected SecretSymbolBase(string name, int offset, IPsiSourceFile sourceFile)
        {
            this.Name = name;
            this.Offset = offset;
            this.SourceFile = sourceFile;
        }

        protected SecretSymbolBase()
        {
        }

        public void SetSourceFile(IPsiSourceFile sourceFile)
        {
            this.SourceFile = sourceFile;
        }

        public virtual void Read(BinaryReader reader)
        {
            this.Name = reader.ReadString();
            this.Offset = reader.ReadInt32();
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(this.Name);
            writer.Write(this.Offset);
        }
    }
}