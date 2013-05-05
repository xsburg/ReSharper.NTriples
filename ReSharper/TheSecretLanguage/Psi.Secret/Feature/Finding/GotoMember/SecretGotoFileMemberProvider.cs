// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretGotoFileMemberProvider.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Goto;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Finding.GotoMember
{
    [FeaturePart]
    public class SecretGotoFileMemberProvider : IGotoFileMemberProvider
    {
        public IEnumerable<MatchingInfo> FindMatchingInfos(
            IdentifierMatcher matcher, INavigationScope scope, CheckForInterrupt checkCancelled, GotoContext gotoContext)
        {
            var fileMemberScope = scope as FileMemberNavigationScope;
            if (fileMemberScope == null)
            {
                return EmptyList<MatchingInfo>.InstanceList;
            }

            var primaryMembersData = this.GetPrimaryMembers(fileMemberScope);

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
        }

        public virtual bool IsApplicable(INavigationScope scope, GotoContext gotoContext)
        {
            return true;
        }

        [CanBeNull]
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

        protected virtual bool IsSourceFileAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.IsValid();
        }

        private IEnumerable<SecretFileMemberData> GetPrimaryMembers(FileMemberNavigationScope fileMemberScope)
        {
            var primarySourceFile = fileMemberScope.GetPrimarySourceFile();
            if (!this.IsSourceFileAvailable(primarySourceFile))
            {
                return EmptyList<SecretFileMemberData>.InstanceList;
            }

            var psiManager = primarySourceFile.GetSolution().GetComponent<PsiManager>();
            var file = psiManager.GetPrimaryPsiFile(primarySourceFile) as SecretFile;
            if (file == null)
            {
                return EmptyList<SecretFileMemberData>.InstanceList;
            }

            var primaryMembers = new LinkedList<SecretFileMemberData>();
            foreach (var declaredElement in file.GetAllPrefixDeclaredElements())
            {
                primaryMembers.AddFirst(new SecretFileMemberData(declaredElement, ContainerDisplayStyle.Namespace));
            }

            var subjects = SecretIdentifierFilter.GetImportantSubjects(file.GetAllUriIdentifierDeclaredElements());
            foreach (var declaredElement in subjects)
            {
                primaryMembers.AddFirst(new SecretFileMemberData(declaredElement, ContainerDisplayStyle.Namespace));
            }

            return primaryMembers;
        }

        private IEnumerable<JetTuple<string, bool>> GetQuickSearchTexts(IDeclaredElement declaredElement)
        {
            return new[] { JetTuple.Of(declaredElement.ShortName, true) };
        }
    }
}
