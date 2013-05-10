using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class SecretFile
    {
        private ISymbolTable myPrefixesSymbolTable;
        private ISymbolTable myUriIdentifiersSymbolTable;

        public override PsiLanguageType Language
        {
            get { return NTriplesLanguage.Instance; }
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

        public ISymbolTable FileUriIdentifiersSymbolTable
        {
            get
            {
                if (this.myUriIdentifiersSymbolTable != null)
                {
                    return this.myUriIdentifiersSymbolTable;
                }
                lock (this)
                {
                    return this.myUriIdentifiersSymbolTable ?? (this.myUriIdentifiersSymbolTable = this.CreateUriIdentifiersSymbolTable());
                }
            }
        }

        private readonly Dictionary<string, IDeclaredElement> myPrefixes = new Dictionary<string, IDeclaredElement>();
        private readonly Dictionary<string, IList<IDeclaredElement>> myUriIdentifiers = new Dictionary<string, IList<IDeclaredElement>>();

        private void CollectPrefixes()
        {
            var declarations = new RecursiveElementCollector<PrefixDeclaration>().ProcessElement(this).GetResults();
            foreach (var declaration in declarations)
            {
                string s = declaration.DeclaredName;
                this.myPrefixes[s] = declaration.DeclaredElement;
            }
        }

        public ISymbolTable CreateUriIdentifiersSymbolTable()
        {
            this.CollectUriIdentifiers();
            if (GetSourceFile() != null)
            {
                var elements = this.myUriIdentifiers.Values.SelectMany(x => x);
                this.myUriIdentifiersSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
            }
            else
            {
                this.myUriIdentifiersSymbolTable = null;
            }


            return this.myUriIdentifiersSymbolTable;
        }

        private void CollectUriIdentifiers()
        {
            var uriIdentifiers =
                new RecursiveElementCollector<IUriIdentifier>()
                    .ProcessElement(this)
                    .GetResults();
            foreach (var uriIdentifier in uriIdentifiers)
            {
                var fullName = uriIdentifier.GetUri();
                if (fullName != null)
                {
                    IList<IDeclaredElement> elements;
                    if (!this.myUriIdentifiers.TryGetValue(fullName, out elements))
                    {
                        this.myUriIdentifiers[fullName] = elements = new List<IDeclaredElement>();
                    }

                    elements.Add(uriIdentifier.DescendantDeclaredElement);
                }
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

        public IEnumerable<IDeclaredElement> GetPrefixDeclaredElements(string name)
        {
            if (this.FilePrefixesSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            var list = new LinkedList<IDeclaredElement>();
            IDeclaredElement declaredElement;
            if (this.myPrefixes.TryGetValue(name, out declaredElement))
            {
                list.AddFirst(declaredElement);
            }

            return list;
        }

        public IEnumerable<IDeclaredElement> GetAllPrefixDeclaredElements()
        {
            if (this.FilePrefixesSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            return this.myPrefixes.Select(x => x.Value);
        }

        public IEnumerable<IDeclaredElement> GetAllUriIdentifierDeclaredElements()
        {
            if (this.FileUriIdentifiersSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            return this.myUriIdentifiers.SelectMany(x => x.Value);
        }

        public IList<IDeclaration> GetUriIdentifiers(string fullName)
        {
            if (this.FileUriIdentifiersSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            IList<IDeclaredElement> elements;
            if (!this.myUriIdentifiers.TryGetValue(fullName, out elements))
            {
                return EmptyList<IDeclaration>.InstanceList;
            }
            
            return elements.Cast<IDeclaration>().ToList();
        }

        public void ClearTables()
        {
            this.myPrefixesSymbolTable = null;
            this.myUriIdentifiersSymbolTable = null;
            this.myPrefixes.Clear();
            this.myUriIdentifiers.Clear();
        }
    }
}