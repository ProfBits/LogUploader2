using System;
using System.Collections.Generic;

using LogUploader.Data.GameAreas;

namespace LogUploader.Test.Data
{
    internal class TestGameArea : GameArea, IEquatable<TestGameArea>
    {
        internal string AreaName { get; }
        internal int AreaNumber { get; }

        public TestGameArea(string areaName, int areaNumber) : base($"Test{areaName}_{areaNumber}", $"TestArea_{areaNumber}", $"TestArea{areaNumber}IconURL")
        {
            this.AreaName = areaName;
            this.AreaNumber = areaNumber;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TestGameArea);
        }

        public bool Equals(TestGameArea other)
        {
            return other != null &&
                   AreaName == other.AreaName &&
                   AreaNumber == other.AreaNumber;
        }

        public override int GetHashCode()
        {
            int hashCode = -348508654;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AreaName);
            hashCode = hashCode * -1521134295 + AreaNumber.GetHashCode();
            return hashCode;
        }
    }
}
