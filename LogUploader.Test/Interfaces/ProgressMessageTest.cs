using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Interfaces;
using LogUploader.Data;

namespace LogUploader.Test.Interfaces
{
    public class ProgressMessageTest
    {
        [Test]
        public void CreateProgressMessageTest()
        {
            double val = 0.5;
            string msg = "message";
            ProgressMessage p = new ProgressMessage(val, msg);

            Assert.AreEqual(val, p.Percent);
            Assert.AreEqual(msg, p.Message);
        }

        [Test]
        public void InvalidProgressMessageMessagesTest([Values("", " \n", "\t", "      ")] string msg)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProgressMessage(0.5, msg));
        }

        [Test]
        public void InvalidProgressMessageMessageNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => new ProgressMessage(0.25, null));
        }

        [Test]
        public void InvalidNegativeProgressValueTest([Values(-10d, -1d, -0.5d)] double val)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProgressMessage(val, "invalid value"));
        }

        [Test, Ignore("Values >1 used, so this test would cause programm crasches")]
        public void InvalidPositiveProgressValueTest([Values(2, 10)] double val)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProgressMessage(val, "invalid value"));
        }
    }
}
