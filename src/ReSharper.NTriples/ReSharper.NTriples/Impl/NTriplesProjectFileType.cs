// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesProjectFileType.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace ReSharper.NTriples.Impl
{
    [ProjectFileTypeDefinition(Name)]
    public class NTriplesProjectFileType : KnownProjectFileType
    {
        public const string NTriplesExtension = ".n3";
        public new const string Name = "NTriples";
        public new static readonly NTriplesProjectFileType Instance;

        protected NTriplesProjectFileType(string name) : base(name)
        {
        }

        protected NTriplesProjectFileType(string name, string presentableName) : base(name, presentableName)
        {
        }

        protected NTriplesProjectFileType(string name, string presentableName, IEnumerable<string> extensions)
            : base(name, presentableName, extensions)
        {
        }

        private NTriplesProjectFileType() : base(Name, "The N-Triples File", new[] { NTriplesExtension })
        {
        }
    }
}
