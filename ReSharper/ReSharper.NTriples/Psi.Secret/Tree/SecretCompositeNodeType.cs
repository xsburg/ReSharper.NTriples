// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCompositeNodeType.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public abstract class SecretCompositeNodeType : CompositeNodeType
    {
        protected SecretCompositeNodeType(string s)
            : base(s)
        {
        }
    }
}
