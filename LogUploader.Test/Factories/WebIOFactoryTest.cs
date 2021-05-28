using System;

using NUnit.Framework;
using LogUploader.Wrapper;


namespace LogUploader.Test.Factories
{
    class WebIOFactoryTest
    {
        [Test]
        [NonParallelizable]
        public void SameInstanceForRandomDomains()
        {
            Mocks.MockSetup.InjectMoks();
            var clientA = WebIOFactory.Create(null, new Uri("https://domin.tld/"));
            var clientB = WebIOFactory.Create(null, new Uri("https://test.tld/"));
            Assert.AreSame(clientA, clientB);
        }
    }
}
