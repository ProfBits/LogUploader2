using LogUploader.Helper;

namespace LogUploader.Data.RaidOrgaPlus
{
    public enum Role : byte
    {
        [StringValue("")]
        Empty = 0,
        [StringValue("P")]
        Power = 1,
        [StringValue("C")]
        Condi = 2,
        [StringValue("T")]
        Tank = 3,
        [StringValue("H")]
        Heal = 4,
        [StringValue("U")]
        Utility = 5,
        [StringValue("B")]
        Banner = 6,
        [StringValue("S")]
        Special = 7,
        [StringValue("K")]
        Kiter = 8,
        [StringValue("Q")]
        Quickness = 9,
        [StringValue("A")]
        Alacrity = 10,
    }
}