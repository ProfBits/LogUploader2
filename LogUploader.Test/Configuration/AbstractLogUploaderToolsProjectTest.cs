namespace LogUploader.Test.Configuration
{
    public abstract class AbstractLogUploaderToolsProjectTest : AbstractLogUploaderProjectTest
    {
        protected override string Pefix { get => $"{base.Pefix}.Tools"; }
    }

    public class LogUploaderToolsDatabaseTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "Database"; }
    }

    public class LogUploaderToolsDiscordTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "Discord"; }
    }

    public class LogUploaderToolsDpsReportTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "DpsReport"; }
    }

    public class LogUploaderToolsEliteInsightsTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "EliteInsights"; }
    }

    public class LogUploaderToolsLoggerTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "Logger"; }
    }

    public class LogUploaderToolsRaidOrgaPlusTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "RaidOrgaPlus"; }
    }

    public class LogUploaderToolsSettingsTest : AbstractLogUploaderToolsProjectTest
    {
        internal override string ProjectName { get => "Settings"; }
    }

}
