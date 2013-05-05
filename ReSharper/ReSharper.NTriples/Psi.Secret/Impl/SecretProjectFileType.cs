// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3ProjectFileType.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace ReSharper.NTriples.Impl
{
    [ProjectFileTypeDefinition(Name)]
    public class SecretProjectFileType : KnownProjectFileType
    {
        public const string SecretExtension = ".n3";
        public new const string Name = "Secret";
        public new static readonly SecretProjectFileType Instance;

        protected SecretProjectFileType(string name) : base(name)
        {
        }

        protected SecretProjectFileType(string name, string presentableName) : base(name, presentableName)
        {
        }

        protected SecretProjectFileType(string name, string presentableName, IEnumerable<string> extensions)
            : base(name, presentableName, extensions)
        {
        }

        private SecretProjectFileType() : base(Name, "The Secret Project File", new[] { SecretExtension })
        {
        }
    }
}
