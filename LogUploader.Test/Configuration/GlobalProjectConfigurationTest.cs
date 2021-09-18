using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Evaluation;

using NUnit.Framework;


namespace LogUploader.Test.Configuration
{
    public class GlobalProjectConfigurationTest
    {
        List<ProjectConfigurationTest> projectConfigurationTests = new List<ProjectConfigurationTest>();
        List<Project> Projects = new List<Project>();
        List<string> ProjectFiles = new List<string>();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var testAssambly = System.Reflection.Assembly.GetExecutingAssembly();
            var projectConfigurationTestsImplemtations = testAssambly.GetTypes().Where(t => t.IsSubclassOf(typeof(ProjectConfigurationTest)) && !t.IsAbstract);
            foreach (var implementation in projectConfigurationTestsImplemtations)
            {
                projectConfigurationTests.Add((ProjectConfigurationTest)testAssambly.CreateInstance(implementation.FullName));
            }

            string repoRoot = ProjectConfigurationTest.GetProjectRootPath();
            var projectFiles = System.IO.Directory.GetFiles(repoRoot, "*.csproj", System.IO.SearchOption.AllDirectories).ToList();

            //TODO This one is special, will be tested differently
            ProjectFiles = projectFiles.Where(pf => !pf.EndsWith("InstallerPackager.csproj")).ToList();

            Projects = projectConfigurationTests.Select(pct => pct.GetProject()).ToList();
        }

        [Test]
        public void AllProjectsHaveTestsClassesTest()
        {
            foreach (var projectTest in projectConfigurationTests)
            {
                Assert.That(ProjectFiles, Has.One.EqualTo(projectTest.ProjectPaht));
            }

            foreach (var projectFile in ProjectFiles)
            {
                Assert.That(projectConfigurationTests.Select(pct => pct.ProjectPaht), Has.One.EqualTo(projectFile));
            }
        }

        [Test]
        public void AllProjectsHaveSameDependencyVersionsTest()
        {
            Dictionary<string, List<string>> nugetReferences = new Dictionary<string, List<string>>();

            foreach (var project in Projects)
            {
                var packageReferences = project.AllEvaluatedItems.Where(item => item.ItemType == "PackageReference")
                    .Select(pRef => (pRef.EvaluatedInclude, pRef.DirectMetadata.First(md => md.Name == "Version").EvaluatedValue));
                foreach ((string pakage, string version) pakageRef in packageReferences)
                {
                    (string pakage, string version) = pakageRef;
                    if (!nugetReferences.ContainsKey(pakage)) nugetReferences.Add(pakage, new List<string>());
                    if (!nugetReferences[pakage].Contains(version)) nugetReferences[pakage].Add(version);
                }
            }

            foreach (var nugetRef in nugetReferences)
            {
                Assert.That(nugetRef.Value, Has.Count.EqualTo(1), $"Nuget packages should use the same version in every project. However {nugetRef.Key} is present in Versions {string.Join(", ", nugetRef.Value)}");
            }
        }

        [Test]
        public void AllProjectsHaveSameFileVersionsTest()
        {
            List<string> FileVersions = new List<string>();

            foreach (var project in Projects)
            {
                FileVersions.AddRange(project.AllEvaluatedProperties.Where(prop => prop.Name == "FileVersion").Select(prop => prop.EvaluatedValue));
            }

            Assert.That(FileVersions.Distinct().Count(), Is.EqualTo(1));
        }
    }
}
