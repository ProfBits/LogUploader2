using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class MiscData : IEquatable<MiscData>
    {
        /// <summary>
        /// Trows an Arugemtn exception if a emote is invalid in some way
        /// </summary>
        /// <param name="emoteKill"></param>
        /// <param name="emoteWipe"></param>
        public MiscData(string emoteKill, string emoteWipe)
        {
            Tools.GP.ValidateDiscordEmote(emoteKill);
            Tools.GP.ValidateDiscordEmote(emoteWipe);
            EmoteKill = emoteKill;
            EmoteWipe = emoteWipe;
        }

        public string EmoteKill { get; }
        public string EmoteWipe { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MiscData);
        }

        public bool Equals(MiscData other)
        {
            return other != null &&
                   EmoteKill == other.EmoteKill &&
                   EmoteWipe == other.EmoteWipe;
        }

        public override int GetHashCode()
        {
            int hashCode = -221645645;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EmoteKill);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EmoteWipe);
            return hashCode;
        }

        public static bool operator ==(MiscData left, MiscData right)
        {
            return EqualityComparer<MiscData>.Default.Equals(left, right);
        }

        public static bool operator !=(MiscData left, MiscData right)
        {
            return !(left == right);
        }
    }
}
