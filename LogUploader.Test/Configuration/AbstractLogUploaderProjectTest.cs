namespace LogUploader.Test.Configuration
{
    public abstract class AbstractLogUploaderProjectTest : ProjectConfigurationTest
    {
        protected virtual string Pefix { get => "LogUploader"; }
        internal override string FullProjectName { get => $"{Pefix}.{ProjectName}"; }
        internal abstract string ProjectName { get; }
    }
}
