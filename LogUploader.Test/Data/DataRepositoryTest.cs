using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;

using NUnit.Framework;

namespace LogUploader.Test.Data
{
    public class MiscDataRepositoryTest
    {
        [Test]
        public void MiscDataConstructorTest()
        {
            MiscData miscData = new MiscData(":kill:", ":wipe:");
            Assert.That(miscData.EmoteKill, Is.EqualTo(":kill:"));
            Assert.That(miscData.EmoteWipe, Is.EqualTo(":wipe:"));
        }

        [Test]
        public void MiscDataConstructorInvaldiKillEmotArgTest([Values(null, "", "   ", "kill")] string killEmote)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new MiscData(killEmote, ":wipe:"));
            TestHelper.ValidateArugumentException(ex);
        }
    }

}
