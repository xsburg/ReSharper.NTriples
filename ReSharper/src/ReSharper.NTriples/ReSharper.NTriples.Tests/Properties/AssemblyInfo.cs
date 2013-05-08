// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.ReSharper;
using JetBrains.Threading;
using NUnit.Framework;
using ReSharper.NTriples.Impl;

// ReSharper disable CheckNamespace
[SetUpFixture]
public class IsolatedReSharperTestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
{
    public override IApplicationDescriptor CreateApplicationDescriptor()
    {
        return new IsolatedReSharperApplicationDescriptor();
    }

    private class IsolatedReSharperApplicationDescriptor : ReSharperApplicationDescriptor
    {
        public override string ProductName
        {
            get
            {
                // The product name is used to find settings under APPDATA. Leaving it at
                // ReSharper would use your normal ReSharper settings (+plugins!) while
                // running tests. This keeps them isolated
                return base.ProductName + "_Isolated";
            }
        }
    }
}

/// <summary>
///     Test environment. Must be in the global namespace.
/// </summary>
[SetUpFixture]
public class TestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
{
    public override void SetUp()
    {
        base.SetUp();
        ReentrancyGuard.Current.Execute(
            "LoadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                this.GetType().Name, GetAssembliesToLoad()));
    }

    public override void TearDown()
    {
        ReentrancyGuard.Current.Execute(
            "UnloadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                this.GetType().Name, GetAssembliesToLoad()));
        base.TearDown();
    }

    /// <summary>
    ///     Gets the assemblies to load into test environment.
    ///     Should include all assemblies which contain components.
    /// </summary>
    private static IEnumerable<Assembly> GetAssembliesToLoad()
    {
        yield return Assembly.GetExecutingAssembly();
        yield return typeof(SecretLanguage).Assembly;
    }
}

[assembly: AssemblyTitleAttribute("")]

// ReSharper restore CheckNamespace


[assembly: AssemblyTitleAttribute("")]