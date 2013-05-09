// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   Identifier.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Tree;
using IStatement = ReSharper.NTriples.Tree.IStatement;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class Identifier
    {
        public IdentifierKind GetKind()
        {
            var kind = IdentifierKind.Other;
            var parent2 = this.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = IdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = IdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = IdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            return kind;
        }

        /*public IdentifierRole GetIdentifierRole()
        {
            var statement = this.GetContainingNode<IStatement>();
            if (statement == null)
            {
                return IdentifierRole.None;
            }

            
            foreach (var fact in statement.FactsEnumerable)
            {
                var predicate = fact.Predicate;
                if (predicate != null && predicate.FirstChild != null &&
                    predicate.FirstChild.GetTokenType() == SecretTokenType.A_KEYWORD)
                {
                    foreach (var obj in fact.ObjectsEnumerable)
                    {
                        
                    }
                }
            }

            return false;
        }*/
    }
}
