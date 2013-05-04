// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ClrGotoSymbolProvider.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Goto;
using JetBrains.ReSharper.Feature.Services.Goto.ChainedProviders;
using JetBrains.ReSharper.Feature.Services.Goto.GotoProviders;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Finding.GotoMember
{
    [FeaturePart]
    public class SecretGotoSymbolProvider
        : //CachedGotoSymbolBase<SecretCache>,
          IGotoSymbolProvider,
          IOccurenceNavigationProvider,
          //IChainedSymbolProvider,
          //IChainedSearchProvider,
          IApplicableGotoProvider
    {
        /*public IEnumerable<ChainedNavigationItemData> GetNextChainedScopes(
            GotoContext gotoContext, IdentifierMatcher matcher, INavigationScope containingScope, CheckForInterrupt checkCancelled)
        {
            var solution = containingScope.GetSolution();
            var cache = this.GetCache(containingScope, solution, gotoContext);

            return ChainedScopesUtil.GetNextCodeModelScope(matcher, containingScope, checkCancelled, cache, true);
        }
*/
        protected IOccurence CreateOccurence(SecretFileMemberData secretFileMemberData)
        {
            var localName = secretFileMemberData.Element as LocalName;
            if (localName != null)
            {
                localName.ScopeToMainFile = true;
            }

            var declaredElementOccurence = new DeclaredElementOccurence(secretFileMemberData.Element, new OccurencePresentationOptions
            {
                ContainerStyle = !(secretFileMemberData.Element is ITypeElement)
                                     ? secretFileMemberData.ContainerDisplayStyle
                                     : ContainerDisplayStyle.NoContainer,
                LocationStyle = GlobalLocationStyle.None
            });

            if (localName != null)
            {
                localName.ScopeToMainFile = false;
            }

            return declaredElementOccurence;
        }

        public IEnumerable<MatchingInfo> FindMatchingInfos(
            IdentifierMatcher matcher, INavigationScope scope, CheckForInterrupt checkCancelled, GotoContext gotoContext)
        {
            var primaryMembersData = this.GetPrimaryMembers(scope.GetSolution());

            var secretFileMembersMap = new SecretFileMembersMap();

            var result = new Collection<MatchingInfo>();
            foreach (var data in primaryMembersData)
            {
                var quickSearchTexts = this.GetQuickSearchTexts(data.Element);
                var matchedText = quickSearchTexts.FirstOrDefault(tuple => matcher.Matches(tuple.A));
                if (matchedText == null)
                {
                    continue;
                }

                secretFileMembersMap.Add(matchedText.A, data);

                var matchingIndicies = matchedText.B
                                           ? matcher.MatchingIndicies(matchedText.A)
                                           : EmptyArray<IdentifierMatch>.Instance;
                result.Add(
                    new MatchingInfo(
                        matchedText.A,
                        matcher.Filter == "*"
                            ? EmptyList<IdentifierMatch>.InstanceList
                            : matchingIndicies,
                        matchedText.B));
            }

            gotoContext.PutData(SecretFileMembersMap.SecretFileMembersMapKey, secretFileMembersMap);
            return result;
        }

        private IEnumerable<JetTuple<string, bool>> GetQuickSearchTexts(IDeclaredElement declaredElement)
        {
            return new[] { JetTuple.Of(declaredElement.ShortName, true) };
        }

        protected virtual bool IsSourceFileAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.IsValid();
        }

        private IEnumerable<SecretFileMemberData> GetPrimaryMembers(ISolution solution)
        {
            var cache = solution.GetComponent<SecretCache>();

            var symbolsByFile = cache.GetAllUriIdentifierSymbolsByFile();
            var services = solution.GetPsiServices();
            foreach (var symbols in symbolsByFile)
            {
                var file = symbols.Key;
                var sourceFile = file.GetPsiFile(SecretLanguage.Instance, new DocumentRange(file.Document, 0));
                foreach (var symbol in symbols.Value)
                {
                    var uriIdentifier = new UriIdentifierDeclaredElement(sourceFile, symbol.Namespace, symbol.LocalName, symbol.Kind, services, true);
                    yield return new SecretFileMemberData(uriIdentifier, ContainerDisplayStyle.NoContainer);
                }
            }
        }

        public IEnumerable<IOccurence> GetOccurencesByMatchingInfo(
            MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext)
        {
            var fileMembersMap = gotoContext.GetData(SecretFileMembersMap.SecretFileMembersMapKey);
            if (fileMembersMap == null)
            {
                yield break;
            }

            var membersData = fileMembersMap[navigationInfo.Identifier];
            foreach (var clrFileMemberData in membersData)
            {
                var occurence = this.CreateOccurence(clrFileMemberData);
                if (occurence != null)
                {
                    yield return occurence;
                }
            }
            /*

            ISolution solution = scope.GetSolution();
            var cache = this.GetCache(scope, solution, gotoContext);
            var namespaceNavigationScope = scope as NamespaceNavigationScope;
            if (namespaceNavigationScope != null)
            {
                INamespace namespaceScope = namespaceNavigationScope.DeclaredElement as INamespace;
                if (namespaceScope != null)
                {
                    return (IEnumerable<IOccurence>)EmptyList<IOccurence>.InstanceList;
                }
                //return Enumerable.Select<IClrDeclaredElement, IOccurence>(Enumerable.Where<IClrDeclaredElement>(Enumerable.Where<IClrDeclaredElement>(ChainedScopesUtil.GetAllSubElements(namespaceScope, cache, true), (Func<IClrDeclaredElement, bool>)(element => element.ShortName == navigationInfo.Identifier)), new Func<IClrDeclaredElement, bool>(this.IsDeclaredElementVisible)), (Func<IClrDeclaredElement, IOccurence>)(x => (IOccurence)new ChainedCodeModelOccurence((IDeclaredElement)x, navigationInfo, OccurencePresentationOptions.DefaultOptions)));
            }
            if (!(scope is SolutionNavigationScope))
            {
                return (IEnumerable<IOccurence>)EmptyList<IOccurence>.InstanceList;
            }
            List<IClrDeclaredElement> list1 = new List<IClrDeclaredElement>();
            foreach (ITypeMember typeMember in cache.GetSourceMembers(navigationInfo.Identifier))
            {
                if (!typeMember.IsSynthetic() && !(typeMember is IAccessor))
                {
                    list1.Add((IClrDeclaredElement)typeMember);
                }
            }
            List<IClrDeclaredElement> list2 = new List<IClrDeclaredElement>();
            if (scope.ExtendedSearchFlag == LibrariesFlag.SolutionAndLibraries)
            {
                foreach (ITypeMember typeMember in cache.GetCompiledMembers(navigationInfo.Identifier))
                {
                    list2.Add((IClrDeclaredElement)typeMember);
                }
            }
            IClrDeclaredElement[] elementsByShortName = cache.GetElementsByShortName(navigationInfo.Identifier);
            return
                Enumerable.Select<IClrDeclaredElement, IOccurence>(
                    Enumerable.Where<IClrDeclaredElement>(
                        Enumerable.Concat<IClrDeclaredElement>(
                            CollectionUtil.Concat<IClrDeclaredElement>(
                                (IEnumerable<IClrDeclaredElement>)list1, elementsByShortName),
                            (IEnumerable<IClrDeclaredElement>)list2),
                        new Func<IClrDeclaredElement, bool>(this.IsDeclaredElementVisible)),
                    (Func<IClrDeclaredElement, IOccurence>)
                    (x => (IOccurence)new DeclaredElementOccurence((IDeclaredElement)x, OccurenceType.Occurence)));*/
        }

        public bool IsApplicable(INavigationScope scope, GotoContext gotoContext)
        {
            if (!(scope is SolutionNavigationScope))
            {
                return scope is NamespaceNavigationScope;
            }

            return true;
        }

        protected SecretCache GetCache(ISolution solution)
        {
            // always perform unscoped search
            return solution.GetComponent<SecretCache>();
        }

        protected IEnumerable<SecretUriIdentifierSymbol> GetNames(SecretCache cache, INavigationScope scope)
        {
            return cache.GetAllUriIdentifierSymbols().Where(s => s.Kind == IdentifierKind.Subject);
        }

        protected virtual bool IsDeclaredElementVisible(IClrDeclaredElement element)
        {
            return true;
        }
    }
}
