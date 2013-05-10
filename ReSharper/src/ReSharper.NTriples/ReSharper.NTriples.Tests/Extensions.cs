// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   Extensions.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using JetBrains.Application.Test;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tests
{
    public static class Extensions
    {
        public static string[] GetFilesToTest(this BaseTestNoShell testClass)
        {
            return testClass.TestDataPath2.GetDirectoryEntries("*" + NTriplesProjectFileType.SecretExtension, true)
                            .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                            .ToArray();
        }
    }
}
