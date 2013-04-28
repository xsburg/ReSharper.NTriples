using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class SecretFile
    {
        private ISymbolTable myPrefixesSymbolTable;
        private ISymbolTable mySubjectsSymbolTable;
        private ISymbolTable myUriIdentifiersSymbolTable;

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

        public ISymbolTable FileSubjectsSymbolTable
        {
            get
            {
                if (this.mySubjectsSymbolTable != null)
                {
                    return this.mySubjectsSymbolTable;
                }
                lock (this)
                {
                    return this.mySubjectsSymbolTable ?? (this.mySubjectsSymbolTable = this.CreateSubjectsSymbolTable());
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
        private readonly Dictionary<string, IList<IDeclaredElement>> mySubjects = new Dictionary<string, IList<IDeclaredElement>>();
        private readonly Dictionary<string, IList<IDeclaredElement>> myUriIdentifiers = new Dictionary<string, IList<IDeclaredElement>>();

        private void CollectPrefixes()
        {
            var declarations = new RecursiveElementCollector<PrefixDeclaration>().ProcessElement(this).GetResults();
            foreach (var declaration in declarations)
            {
                string s = declaration.DeclaredName;
                myPrefixes[s] = declaration.DeclaredElement;
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
                var fullName = uriIdentifier.GetUri(this);
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

        private void CollectSubjects()
        {
            var uriIdentifiers =
                new RecursiveElementCollector<IUriIdentifier>(i => i.GetKind() == UriIdentifierKind.Subject)
                    .ProcessElement(this)
                    .GetResults();
            foreach (var uriIdentifier in uriIdentifiers)
            {
                var declaration = uriIdentifier.LocalName as IDeclaredElement;
                string prefix = uriIdentifier.Prefix.GetText();
                var @namespace = this.GetDeclaredElements(prefix).FirstOrDefault();
                if (@namespace != null)
                {
                    var fullName = @namespace + "#" + uriIdentifier.LocalName;
                    IList<IDeclaredElement> elements;
                    if (!this.mySubjects.TryGetValue(fullName, out elements))
                    {
                        this.mySubjects[fullName] = elements = new List<IDeclaredElement>();
                    }

                    elements.Add(declaration);
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

        public ISymbolTable CreateSubjectsSymbolTable()
        {
            this.CollectSubjects();
            if (GetSourceFile() != null)
            {
                var elements = this.mySubjects.Values.SelectMany(x => x);
                this.mySubjectsSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
            }
            else
            {
                this.mySubjectsSymbolTable = null;
            }


            return this.mySubjectsSymbolTable;
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

        public void ClearTables()
        {
            myPrefixesSymbolTable = null;
            mySubjectsSymbolTable = null;
            myUriIdentifiersSymbolTable = null;
            myPrefixes.Clear();
            mySubjects.Clear();
            myUriIdentifiers.Clear();
        }

        public IList<IDeclaration> GetSubjects()
        {
            if (FileSubjectsSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            return mySubjects.Values.SelectMany(x => x).Cast<IDeclaration>().ToList();
        }

        public IList<IDeclaration> GetUriIdentifiers(string fullName)
        {
            if (FileUriIdentifiersSymbolTable == null)
            {
                throw new Exception("never thrown");
            }

            IList<IDeclaredElement> elements;
            if (!myUriIdentifiers.TryGetValue(fullName, out elements))
            {
                return EmptyList<IDeclaration>.InstanceList;
            }
            
            return elements.Cast<IDeclaration>().ToList();
        }
    }
}