using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.Build.Evaluation;

using NUnit.Framework;


namespace LogUploader.Test.Configuration
{
    public abstract class ProjectConfigurationTest
    {
        protected const int ACTUAL_TESTS_ORDER = 1000;

        internal string ProjectRootPath { get; }
        internal abstract string FullProjectName { get; }
        internal virtual string Extensione { get => ".csproj"; }
        internal string ProjectPaht { get => $"{ProjectRootPath}{FullProjectName}{System.IO.Path.DirectorySeparatorChar}{FullProjectName}{Extensione}"; }

        public ProjectConfigurationTest()
        {
            ProjectRootPath = GetProjectRootPath();
        }

        internal static string GetProjectRootPath ([CallerFilePath] string cfp = "")
        {
            string testProjectFolder = System.IO.Path.GetDirectoryName(cfp);
            string oneUp = ".." + System.IO.Path.DirectorySeparatorChar;
            string ProjectRoot = System.IO.Path.Combine(testProjectFolder, oneUp, oneUp);
            return System.IO.Path.GetFullPath(ProjectRoot);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ProjectExistsTest();
            Proj = ProjectIsParseableTest();
            TestIfSetUpSuccessful();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Proj = null;
        }

        public void ProjectExistsTest()
        {
            Assert.That(ProjectPaht, Is.Not.Null.And.Not.Empty);
            Assert.That(ProjectPaht, Does.Exist);
        }

        public Project ProjectIsParseableTest()
        {
            Project proj = null;
            Assert.That(() => proj = GetProject(), Throws.Nothing);
            Assert.That(proj, Is.Not.Null);
            return proj;
        }

        protected Project Proj { get; set; }

        internal virtual Project GetProject()
        {
            return ProjectCollection.GlobalProjectCollection.LoadProject(ProjectPaht);
        }

        private void TestIfSetUpSuccessful()
        {
            Assert.That(Proj, Is.Not.Null, $"Load of project {FullProjectName}{Extensione} faild. Can not test configuration.");
        }

        [Test]
        public virtual void ProjectAssemblyTitleTest()
        {
            var assemblyTitleProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "AssemblyTitle");
            Assert.That(assemblyTitleProp?.EvaluatedValue, Is.EqualTo(FullProjectName));
        }


        [Test]
        public void ProjectCopyrightTest()
        {
            var copyrightProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "Copyright");
            Assert.That(copyrightProp?.EvaluatedValue, Does.Match($"^Copyright © ProfBits 2019 - {DateTime.Now.Year}$"));
        }

        [Test]
        public void ProjectCompanyTest()
        {
            var companyProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "Company");
            Assert.That(companyProp?.EvaluatedValue, Is.EqualTo("Bits Inc."));
        }

        [Test]
        public void ProjectProductTest()
        {
            var productProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "Product");
            Assert.That(productProp?.EvaluatedValue, Is.EqualTo("Log Uploader"));
        }

        [Test]
        public void ProjectLangVersionTest()
        {
            var langVersionProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "LangVersion");
            Assert.That(langVersionProp?.EvaluatedValue, Is.EqualTo("7.3"));
        }

        [Test]
        public void ProjectFileVersionTest()
        {
            var FileVersionProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "FileVersion");
            Assert.That(FileVersionProp?.EvaluatedValue, Does.Match($"^\\d+\\.\\d+\\.\\d+\\.\\d+$"));
        }

        [Test]
        public void ProjectImportsLogUploaderProps()
        {
            var FileVersionProp = Proj.Imports.LastOrDefault(p => 
            p.ImportedProject.FullPath == $"{ProjectRootPath}build{System.IO.Path.DirectorySeparatorChar}LogUploader.build.props");
        }

        [Test]
        public void ProjectTargetFrameworkTest()
        {
            var targetFrameworkProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "TargetFramework");
            Assert.That(targetFrameworkProp?.EvaluatedValue, Is.EqualTo("net472"));
        }

        [Test]
        public void ProjectConfigurationsTest()
        {
            var configurationsProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "Configurations");
            Assert.That(configurationsProp?.EvaluatedValue, Does.Contain("Debug"));
            Assert.That(configurationsProp?.EvaluatedValue, Does.Contain("Release"));
            Assert.That(configurationsProp?.EvaluatedValue, Does.Contain("CreateLanguageXMLs"));
            Assert.That(configurationsProp?.EvaluatedValue, Does.Contain("AlphaRelease"));
            Assert.That(configurationsProp?.EvaluatedValue, Does.Contain("BetaRelease"));
            Assert.That(configurationsProp?.EvaluatedValue, Has.Length.EqualTo("Debug;Release;CreateLanguageXMLs;AlphaRelease;BetaRelease".Length));
        }

        [Test]
        public void ProjectNoWarningsDisabledTest()
        {
            var disabledWarningsProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "DisabledWarnings");
            Assert.That(disabledWarningsProp?.EvaluatedValue?.Replace(';', ' ')?.Trim(), Is.Null.Or.Empty);
        }

        [Test]
        public void ProjectTreatWarningsAsErrorsTest()
        {
            var treatWarningsAsErrorsProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "TreatWarningsAsErrors");
            Assert.That(treatWarningsAsErrorsProp?.EvaluatedValue.ToLowerInvariant(), Is.EqualTo("false"));
        }

        [Test]
        public void ProjectWarningsAsErrorsTest()
        {
            var warningsAsErrorsProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "WarningsAsErrors");
            Assert.That(warningsAsErrorsProp?.EvaluatedValue, Is.Not.Null);
            var warningsAsErrors = warningsAsErrorsProp?.EvaluatedValue.Split(';').Where(warning => !string.IsNullOrWhiteSpace(warning));
            string[] expectedWarningsAsErrors = { };
            foreach (var expectedWarningsAsError in expectedWarningsAsErrors)
            {
                Assert.That(warningsAsErrors, Does.Contain(expectedWarningsAsError));
            } 
        }

        [Test]
        public void ProjectWarningsNotAsErrorsTest()
        {
            var warningsNotAsErrorsProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "WarningsNotAsErrors");
            Assert.That(warningsNotAsErrorsProp?.EvaluatedValue?.Replace(';', ' ')?.Trim(), Is.Null.Or.Empty);
        }
    }
}
