using System.Linq;

using NUnit.Framework;


namespace LogUploader.Test.Configuration
{
    public class LogUploaderProjectTest : ProjectConfigurationTest
    {
        internal override string FullProjectName { get => "LogUploader"; }


        [Test]
        public void ProjectAssemblyVersionTest()
        {
            var assemblyVersionProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "AssemblyVersion");
            Assert.That(assemblyVersionProp?.EvaluatedValue, Does.Match($"^\\d+\\.\\d+\\.\\d+\\.\\d+$"));
        }

        [Test]
        public void ProjectProductVersionTest()
        {
            var productVersionProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "ProductVersion");
            Assert.That(productVersionProp?.EvaluatedValue, Does.Match($"^\\d+\\.\\d+\\.\\d+\\.\\d+$"));
        }

        [Test]
        public void ProjectInformationalVersionTest()
        {
            var informationalVersionProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "InformationalVersion");
            Assert.That(informationalVersionProp?.EvaluatedValue, Does.Match($"^\\d+\\.\\d+\\.\\d+\\.\\d+$"));
        }

        [Test]
        public void ProjectVersionEqualsTest()
        {
            var fileVersion = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "FileVersion").EvaluatedValue;
            var productVersion = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "ProductVersion").EvaluatedValue;
            var informationalVersion = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "InformationalVersion").EvaluatedValue;
            var assemblyVersion = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "AssemblyVersion").EvaluatedValue;

            Assert.That(fileVersion, Is.EqualTo(productVersion));
            Assert.That(fileVersion, Is.EqualTo(informationalVersion));
            Assert.That(fileVersion, Is.EqualTo(assemblyVersion));
        }

        [Test]
        public void ApplicationManifestSetTest()
        {
            var applicationManifestProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "ApplicationManifest");
            Assert.That(applicationManifestProp?.EvaluatedValue, Is.Not.Empty);
        }

        [Test]
        public void ApplicationIconSetTest()
        {
            var applicationIconProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "ApplicationIcon");
            Assert.That(applicationIconProp?.EvaluatedValue, Is.Not.Empty);
        }
        public override void ProjectAssemblyTitleTest()
        {
            var assemblyTitleProp = Proj.AllEvaluatedProperties.LastOrDefault(p => p.Name == "AssemblyTitle");
            Assert.That(assemblyTitleProp?.EvaluatedValue, Is.EqualTo("Log Uploader"));
        }
    }

    public class LogUploaderDataTest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "Data"; }
    }

    public class LogUploaderGUITest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "GUI"; }
    }

    public class LogUploaderInterfacesTest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "Interfaces"; }
    }

    public class LogUploaderLocalisationTest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "Localisation"; }
    }

    public class LogUploaderTestTest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "Test"; }
    }

    public class LogUploaderToolsTest : AbstractLogUploaderProjectTest
    {
        internal override string ProjectName { get => "Tools"; }
    }

}
