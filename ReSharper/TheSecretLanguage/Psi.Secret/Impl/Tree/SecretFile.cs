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
        private ISymbolTable myNamespacePrefixSymbolTable;

        public override PsiLanguageType Language
        {
            get { return SecretLanguage.Instance; }
        }

        public ISymbolTable FileNamespacePrefixSymbolTable
        {
            get
            {
                if (this.myNamespacePrefixSymbolTable != null)
                {
                    return this.myNamespacePrefixSymbolTable;
                }
                lock (this)
                {
                    return this.myNamespacePrefixSymbolTable ?? (this.myNamespacePrefixSymbolTable = this.CreateNamespacePrefixesSymbolTable());
                }
            }
        }

        private readonly Dictionary<string, IDeclaredElement> myNamespacePrefixes = new Dictionary<string, IDeclaredElement>();

        private void CollectNamespacePrefixes()
        {
            var namespaces = new RecursiveElementCollector<NamespacePrefix>(p => true).GetResults();
            foreach (var ns in namespaces)
            {
                IDeclaredElement element = new NamespacePrefixDeclaredElement(this, ns.GetText(), GetPsiServices());
                this.myNamespacePrefixes[ns.GetText()] = element;
            }
            ITreeNode child = firstChild;
            while (child != null)
            {
                var declaration = child as IDeclaration;
                if (declaration != null)
                {
                    string s = declaration.DeclaredName;
                    this.myNamespacePrefixes[s] = declaration.DeclaredElement;
                }
                child = child.NextSibling;
            }
        }

        private class NamespacePrefixesVisitor : TreeNodeVisitor<Dictionary<string, IDeclaredElement>>
        {
            public override void VisitNamespacePrefix(INamespacePrefix namespacePrefix, Dictionary<string, IDeclaredElement> context)
            {
                var declaration = namespacePrefix as IDeclaration;
                if (declaration != null)
                {
                    var s = declaration.DeclaredName;
                    context[s] = declaration.DeclaredElement;
                }
            }
        }

        public ISymbolTable CreateNamespacePrefixesSymbolTable()
        {
            this.CollectNamespacePrefixes();
            if (GetSourceFile() != null)
            {
                Dictionary<string, IDeclaredElement>.ValueCollection elements = this.myNamespacePrefixes.Values;
                myNamespacePrefixSymbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
            }
            else
            {
                myNamespacePrefixSymbolTable = null;
            }

            /*var optionsDefinition = FirstChild as IOptionsDefinition;
            if (optionsDefinition != null)
            {
                ITreeNode child = optionsDefinition.FirstChild;
                ITreeNode tokenTypeClassFQNNode = null;
                ITreeNode parserClassNameNode = null;
                ITreeNode parserPackageNode = null;
                ITreeNode treeInterfacesPackageNode = null;
                ITreeNode treeClassesPackageNode = null;
                ITreeNode visitorClassNameNode = null;
                ITreeNode interfacePrefixNode = null;
                ITreeNode visitorMethodSuffixNode = null;
                ITreeNode visitorMethodPrefixNode = null;
                while (child != null)
                {
                    var optionDefinition = child as IOptionDefinition;
                    if (optionDefinition != null)
                    {
                        IOptionName optionName = optionDefinition.OptionName;
                        var token = (optionName.FirstChild as PsiTokenBase);
                        if (token.NodeType.Equals(PsiTokenType.STRING_LITERAL))
                        {
                            if ("\"tokenTypeClassFQName\"".Equals(token.GetText()))
                            {
                                tokenTypeClassFQNNode = optionDefinition.OptionStringValue;
                            }
                            if ("\"visitMethodPrefix\"".Equals(token.GetText()))
                            {
                                visitorMethodPrefixNode = optionDefinition.OptionStringValue;
                            }
                        }
                        if ("parserClassName".Equals(optionName.GetText()))
                        {
                            parserClassNameNode = optionDefinition.OptionStringValue;
                        }
                        if ("parserPackage".Equals(optionName.GetText()))
                        {
                            parserPackageNode = optionDefinition.OptionStringValue;
                        }
                        if ("psiInterfacePackageName".Equals(optionName.GetText()))
                        {
                            treeInterfacesPackageNode = optionDefinition.OptionStringValue;
                        }
                        if ("psiStubsPackageName".Equals(optionName.GetText()))
                        {
                            treeClassesPackageNode = optionDefinition.OptionStringValue;
                        }
                        if ("visitorClassName".Equals(optionName.GetText()))
                        {
                            visitorClassNameNode = optionDefinition.OptionStringValue;
                        }
                        if ("visitorMethodSuffix".Equals(optionName.GetText()))
                        {
                            visitorMethodSuffixNode = optionDefinition.OptionStringValue;
                        }
                        if ("\"interfaceNamePrefix\"".Equals(optionName.GetText()))
                        {
                            interfacePrefixNode = optionDefinition.OptionStringValue;
                        }
                    }
                    child = child.NextSibling;
                }
                if (tokenTypeClassFQNNode != null)
                {
                    AddTokensToSymbolTable(tokenTypeClassFQNNode);
                }

                if ((parserPackageNode != null) && (parserClassNameNode != null))
                {
                    AddDerivedElementsToSymbolTable(visitorMethodSuffixNode, treeInterfacesPackageNode, parserPackageNode, parserClassNameNode, treeClassesPackageNode, interfacePrefixNode, visitorMethodPrefixNode, visitorClassNameNode);
                }
            }*/

            return myNamespacePrefixSymbolTable;
        }
    }
}