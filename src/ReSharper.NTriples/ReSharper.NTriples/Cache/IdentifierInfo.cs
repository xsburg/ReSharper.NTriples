using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Cache
{
    public class IdentifierInfo
    {
        /// <summary>Subject, Predicate, Object on something else.</summary>
        public IdentifierKind Kind { get; private set; }

        public bool IsClassDeclaration { get; private set; }

        public bool IsTypePropertyDeclaration { get; private set; }

        public string[] DeclaredTypeNames { get; private set; }

        public string[] DeclaredTypePropertyNames { get; private set; }

        public static IdentifierInfo CreateClassDeclaration(bool isClassDeclaration, string[] declaredTypePropertyNames)
        {
            var info = new IdentifierInfo
                {
                    Kind = IdentifierKind.Subject,
                    IsClassDeclaration = isClassDeclaration,
                    IsTypePropertyDeclaration = declaredTypePropertyNames.Length > 0,
                    DeclaredTypeNames = new string[0],
                    DeclaredTypePropertyNames = declaredTypePropertyNames
                };
            return info;
        }

        public static IdentifierInfo CreateClassInstantiation(string[] declaredTypeName)
        {
            var info = new IdentifierInfo
                {
                    Kind = IdentifierKind.Subject,
                    IsClassDeclaration = false,
                    IsTypePropertyDeclaration = false,
                    DeclaredTypeNames = declaredTypeName,
                    DeclaredTypePropertyNames = new string[0]
                };
            return info;
        }
        
        private IdentifierInfo()
        {
        }

        public IdentifierInfo(IdentifierKind kind)
        {
            Kind = kind;
            DeclaredTypeNames = new string[0];
            DeclaredTypePropertyNames = new string[0];
        }

        public static IdentifierInfo Read(BinaryReader reader)
        {
            var kind = (IdentifierKind)reader.ReadInt32();
            var info = new IdentifierInfo(kind);
            info.IsClassDeclaration = reader.ReadBoolean();
            info.IsTypePropertyDeclaration = reader.ReadBoolean();
            var count = reader.ReadInt32();
            info.DeclaredTypeNames = Enumerable.Range(0, count).Select(_ => reader.ReadString()).ToArray();
            count = reader.ReadInt32();
            info.DeclaredTypePropertyNames = Enumerable.Range(0, count).Select(_ => reader.ReadString()).ToArray();
            return info;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((int)this.Kind);
            writer.Write(IsClassDeclaration);
            writer.Write(IsTypePropertyDeclaration);
            writer.Write(DeclaredTypeNames.Length);
            DeclaredTypeNames.Apply(writer.Write);
            writer.Write(DeclaredTypePropertyNames.Length);
            DeclaredTypePropertyNames.Apply(writer.Write);
        }
    }
}