using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.Repositories;
using LogUploader.Data;
using NUnit.Framework;

namespace LogUploader.Test.Data
{
    class ProfessionDataBuilderTest
    {
        [Test, Ignore("just a placeholder will be implmentat after repository")]
        public void ParseProfessionData()
        {
            ProfessionRepository testProfessionRepo = new TestProfessionRepo();
            ProfessionDataBuilder.ParseJson("json", testProfessionRepo);
        }
    }

    internal class TestProfessionRepo : ProfessionRepository
    {
        public TestProfessionRepo()
        {
        }
    }

}
