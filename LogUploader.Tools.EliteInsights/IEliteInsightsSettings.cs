namespace LogUploader.Tools.EliteInsights
{
    public interface IEliteInsightsSettings
    {
        bool AutoUpdateEI { get; set; }
        bool CreateCombatReplay { get; set; }
        bool LightTheme { get; set; }
    }
}
