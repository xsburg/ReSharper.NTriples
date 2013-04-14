// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   MSBuildPropertiesCache.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.impl;
using JetBrains.Util;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    [SolutionComponent]
    internal class MSBuildPropertiesCache
    {
        private readonly Dictionary<IProject, Dictionary<string, string>> myData;
        private readonly ViewableProjectsCollection myViewableProjectsCollection;

        public MSBuildPropertiesCache(Lifetime lifetime, ViewableProjectsCollection viewableProjectsCollection)
        {
            this.myViewableProjectsCollection = viewableProjectsCollection;
            this.myData = new Dictionary<IProject, Dictionary<string, string>>();
            this.myViewableProjectsCollection.Projects.View(
                lifetime,
                project => { },
                project =>
                {
                    if (this.myData.ContainsKey(project))
                    {
                        this.myData.Remove(project);
                    }
                });
        }

        public string GetProjectPropertyByName(IProject project, string name)
        {
            Dictionary<string, string> cachedProperties;
            if (this.myData.TryGetValue(project, out cachedProperties))
            {
                string value;
                if (cachedProperties.TryGetValue(name, out value))
                {
                    return value;
                }
            }
            else
            {
                cachedProperties = new Dictionary<string, string>();
                this.myData.Add(project, cachedProperties);
            }
            try
            {
                const string resolveassemblyreference = "ResolveAssemblyReferences";
                IProjectFile projectFile = project.ProjectFile;
                if (projectFile == null)
                {   
                    return null;
                }

                List<Project> loadedProjects =
                    ProjectCollection.GlobalProjectCollection.GetLoadedProjects(
                        projectFile.Location.FullPath).ToList();
                if (loadedProjects.Count != 1)
                {
                    return null;
                }

                Project loadedProject = loadedProjects[0];
                ProjectInstance projectInstance =
                    BuildManager.DefaultBuildManager.GetProjectInstanceForBuild(loadedProject);
                if (projectInstance.Build(resolveassemblyreference, EmptyList<Microsoft.Build.Framework.ILogger>.InstanceList))
                {
                    ICollection<ProjectPropertyInstance> allProperties = projectInstance.Properties;
                    foreach (ProjectPropertyInstance property in allProperties)
                    {
                        cachedProperties.Add(property.Name, property.EvaluatedValue);
                    }
                    ProjectPropertyInstance projectPropertyInstance = projectInstance.GetProperty(name);
                    if (projectPropertyInstance != null)
                    {
                        return projectPropertyInstance.EvaluatedValue;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogExceptionSilently(e);
            }
            return "";
        }
    }
}
