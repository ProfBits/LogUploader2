using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Tools.Database;

using NUnit.Framework;

namespace LogUploader.Test.Tools.Database
{
    public abstract class AbstractLogDatabaseConnectorTest
    {
        public abstract ILogDatabaseConnector CreateLogDatabaseConnector();
        public abstract void SetupDatabase();
        public abstract void CleanUpDataBase();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SetupDatabase();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CleanUpDataBase();
        }

        public abstract void CreateDatabaseConnectorTest();
    }

    public class LogDatabaseConnectorOldTest : AbstractLogDatabaseConnectorTest
    {
        private string DbFileName = "LogDBv10.test.db";
        private string ConnectionString { get => $@"Data Source=""{TestSetup.GetPathToTestFiles("Database", "LogDBv10.test.db")}""; Version=3;Connect Timeout=30"; }

        public override void CleanUpDataBase()
        {
            throw new NotImplementedException();
        }

        public override void CreateDatabaseConnectorTest()
        {
            throw new NotImplementedException();
        }

        public override ILogDatabaseConnector CreateLogDatabaseConnector()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void SetupDatabase()
        {
            Assert.Pass();
        }
    }
}
