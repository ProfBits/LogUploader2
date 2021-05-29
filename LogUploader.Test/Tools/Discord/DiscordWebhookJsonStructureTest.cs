using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data;
using LogUploader.Tools.Discord;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace LogUploader.Test.Tools.Discord
{
    public abstract class DiscordWebhookJsonStructureTest
    {
        public abstract IDiscordPostGen GetDiscordPostGen();

        public virtual ICachedLog GetSampleLog(int i)
        {
            return null;
        }

        [Test]
        public void TopLevlJsonTest()
        {
            //TODO implmement
            //https://discord.com/developers/docs/resources/webhook#execute-webhook
            Assert.Pass();
        }

        [Test]
        public void EmbedsTest()
        {
            //TODO implmement
            //https://discord.com/developers/docs/resources/channel#embed-object
            Assert.Pass();
        }

        [Test]
        public void EmbedsLimitsTest()
        {
            //TODO implmement
            //https://discord.com/developers/docs/resources/channel#embed-limits
            Assert.Pass();
        }

        [Test]
        public void FieldsTest()
        {
            //TODO implmement
            //https://discord.com/developers/docs/resources/channel#embed-limits
            Assert.Pass();
        }

        [Test]
        public void UsernameTest()
        {
            //TODO implmement
            Assert.Pass();
        }

        [Test]
        public void Avatar()
        {
            //TODO implmement
            Assert.Pass();
        }

        [Test]
        public void Author()
        {
            //TODO implmement
            //https://discord.com/developers/docs/resources/channel#embed-object-embed-author-structure
            Assert.Pass();
        }
    }

    public class DiscordWebhookJsonStructurePerWingTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new PerWingGen();
        }
    }

    public class DiscordWebhookJsonStructurePerBossTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new PerBossGenerator();
        }
    }

    public class DiscordWebhookJsonStructureDetaildGenTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new DetaildGenerator();
        }
    }

    public class DiscordWebhookJsonStructurePerWingWithEmoteTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new PerWingWithEmotes();
        }
    }

    public class DiscordWebhookJsonStructureCompactWithEmotesTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new CompactWithEmotesGenerator();
        }
    }

    public class DiscordWebhookJsonStructureCompactWithClasesTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new CompactWithClasesGenerator();
        }
    }

    public class DiscordWebhookJsonStructurePerWingWithClassesTest : DiscordWebhookJsonStructureTest
    {
        public override IDiscordPostGen GetDiscordPostGen()
        {
            return new PerWingWithClassesGenerator();
        }
    }
}
