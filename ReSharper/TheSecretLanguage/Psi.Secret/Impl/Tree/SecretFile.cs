using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class SecretFile
    {
        private ISymbolTable myPrefixesSymbolTable;

        public override PsiLanguageType Language
        {
            get { return SecretLanguage.Instance; }
        }

        public ISymbolTable FilePrefixesSymbolTable
        {
            get
            {
                if (this.myPrefixesSymbolTable != null)
                {
                    return this.myPrefixesSymbolTable;
                }
                lock (this)
                {
                    return this.myPrefixesSymbolTable ?? (this.myPrefixesSymbolTable = this.CreatePrefixesSymbolTable());
                }
            }
        }

        private readonly Dictionary<string, IDeclaredElement> myPrefixes = new Dictionary<string, IDeclaredElement>();

        private void CollectPrefixes()
        {
            var declarations = new RecursiveElementCollector<PrefixDeclaration>().ProcessElement(this).GetResults();
            foreach (var declaration in declarations)
            {
                string s = declaration.DeclaredName;
                myPrefixes[s] = declaration.DeclaredElement;
            }
        }

        public ISymbolTable CreatePrefixesSymbolTable()
        {
            this.CollectPrefixes();
            if (GetSourceFile() != null)
            {
                Dictionary<string, IDeclaredElement>.ValueCollection elements = this.myPrefixes.Values;
                this.myPrefixesSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
            }
            else
            {
                this.myPrefixesSymbolTable = null;
            }


            return this.myPrefixesSymbolTable;
        }

        public IEnumerable<IDeclaredElement> GetDeclaredElements(string name)
        {
            if (FilePrefixesSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            var list = new LinkedList<IDeclaredElement>();
            IDeclaredElement declaredElement;
            if (myPrefixes.TryGetValue(name, out declaredElement))
            {
                list.AddFirst(declaredElement);
            }

            return list;
        }
    }
}