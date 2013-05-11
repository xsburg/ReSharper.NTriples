using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Cache
{
    public class IdentifierInfo
    {
        public IdentifierKind Kind { get; private set; }
        public bool IsTypeDeclaration { get; private set; }
        public bool IsClassDeclaration { get; private set; }
        public bool IsPropertyDeclaration { get; private set; }
        public bool IsUserPropertyDeclaration { get; private set; }
        public string[] DeclaredTypeNames { get; private set; }

        public IdentifierInfo(
            IdentifierKind kind,
            bool isClassDeclaration,
            bool isPropertyDeclaration,
            bool isUserPropertyDeclaration,
            string[] declaredTypeName)
        {
            this.Kind = kind;
            this.IsClassDeclaration = isClassDeclaration;
            this.IsPropertyDeclaration = isPropertyDeclaration;
            this.IsUserPropertyDeclaration = isUserPropertyDeclaration;
            this.DeclaredTypeNames = declaredTypeName;
            this.IsTypeDeclaration = IsClassDeclaration || IsPropertyDeclaration || IsUserPropertyDeclaration;
        }

        public IdentifierInfo(IdentifierKind kind)
        {
            if (kind == IdentifierKind.Subject)
            {
                throw new ArgumentException("The subject identifiers should have more information.");
            }

            Kind = kind;
            DeclaredTypeNames = new string[0];
        }

        public static IdentifierInfo Read(BinaryReader reader)
        {
            var kind = (IdentifierKind)reader.ReadInt32();
            var info = new IdentifierInfo(kind);
            info.IsClassDeclaration = reader.ReadBoolean();
            info.IsPropertyDeclaration = reader.ReadBoolean();
            info.IsUserPropertyDeclaration = reader.ReadBoolean();
            info.IsTypeDeclaration = info.IsClassDeclaration || info.IsPropertyDeclaration || info.IsUserPropertyDeclaration;
            var count = reader.ReadInt32();
            info.DeclaredTypeNames = Enumerable.Range(0, count).Select(_ => reader.ReadString()).ToArray();
            return info;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((int)this.Kind);
            writer.Write(IsClassDeclaration);
            writer.Write(IsPropertyDeclaration);
            writer.Write(IsUserPropertyDeclaration);
            writer.Write(DeclaredTypeNames.Length);
            DeclaredTypeNames.Apply(writer.Write);
        }
    }
}