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

namespace JetBrains.ReSharper.Psi.Secret
{
    [ProjectFileTypeDefinition(Name)]
    public class N3ProjectFileType : KnownProjectFileType
    {
        public const string N3Extension = ".n3";
        public new const string Name = "N3";
        public new static readonly N3ProjectFileType Instance;

        protected N3ProjectFileType(string name) : base(name)
        {
        }

        protected N3ProjectFileType(string name, string presentableName) : base(name, presentableName)
        {
        }

        protected N3ProjectFileType(string name, string presentableName, IEnumerable<string> extensions)
            : base(name, presentableName, extensions)
        {
        }

        private N3ProjectFileType() : base(Name, "N3", new[] { N3Extension })
        {
        }
    }
}
