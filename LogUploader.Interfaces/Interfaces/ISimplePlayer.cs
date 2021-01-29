using System.Collections.Generic;
using System.Drawing;

namespace LogUploader.Data
{
    public interface ISimplePlayer
    {
        string Account { get; }
        int Concentration { get; }
        int Condition { get; }
        int DpsAll { get; }
        int DpsAllCondi { get; }
        int DpsAllPower { get; }
        List<ISimpleDps> DpsTarget { get; }
        int DpsTargets { get; }
        int DpsTargetsCondi { get; }
        int DpsTargetsPower { get; }
        int Group { get; }
        float GroupAlacrity { get; }
        float GroupQuickness { get; }
        int Healing { get; }
        string Name { get; }
        IProfession Profession { get; }
        Image ProfessionIcon { get; }
        string ProfessionStr { get; }
        int Toughness { get; }
        int Version { get; }

        void UpdateTargetIDs(Dictionary<int, int> table);
        string ToString();
    }
}