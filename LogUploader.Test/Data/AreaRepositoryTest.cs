using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data.New;
using LogUploader.Data.Repositories;

namespace LogUploader.Test.Data
{

    public abstract class AreaRepositoryTest { }
    
    internal abstract class AbstractAreaRepositoryTest<T> : AreaRepositoryTest where T : GameArea
    {
        internal abstract AreaRepository<T> CreateEmptyRepository();
        internal abstract T GetDefaultAreaInstace();
        internal abstract T GetNonDefaultAreaInstace();

        [Test]
        public void CreateDoesNotThrowTest()
        {
            Assert.That(() => CreateEmptyRepository(), Throws.Nothing);
        }

        [Test]
        public void IsFilledByDefaultTest()
        {
            var repo = CreateEmptyRepository();
            LogUploader.Data.AreaProvider<T> provider = repo;
            Assert.That(provider.Get(), Is.Not.Null);
            Assert.That(provider.Get(), Is.EqualTo(GetDefaultAreaInstace()));
        }

        [Test]
        public void SetAreaTest()
        {
            var repo = CreateEmptyRepository();
            LogUploader.Data.AreaProvider<T> provider = repo;
            repo.SetArea(GetNonDefaultAreaInstace());
            Assert.That(provider.Get(), Is.EqualTo(GetNonDefaultAreaInstace()));
        }

        [Test]
        public void SetAreaNullTest()
        {
            var repo = CreateEmptyRepository();
            LogUploader.Data.AreaProvider<T> provider = repo;
            repo.SetArea(GetNonDefaultAreaInstace());

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => repo.SetArea(null)));
            Assert.That(provider.Get(), Is.Not.Null);
            Assert.That(provider.Get(), Is.EqualTo(GetNonDefaultAreaInstace()));
        }

        [Test]
        public void GetPropertyTest()
        {
            var repo = CreateEmptyRepository();
            T area = GetNonDefaultAreaInstace();
            LogUploader.Data.AreaProvider<T> provider = repo;
            repo.SetArea(area);

            Assert.That(provider.Name, Is.EqualTo(area.Name));
            Assert.That(provider.NameEN, Is.EqualTo(area.NameEN));
            Assert.That(provider.NameDE, Is.EqualTo(area.NameDE));
            Assert.That(provider.ShortName, Is.EqualTo(area.ShortName));
            Assert.That(provider.ShortNameEN, Is.EqualTo(area.ShortNameEN));
            Assert.That(provider.ShortNameDE, Is.EqualTo(area.ShortNameDE));
        }
    }

    internal class TrainingRepositoryTest : AbstractAreaRepositoryTest<Training>
    {
        internal override AreaRepository<Training> CreateEmptyRepository()
        {
            return new TrainingAreaRepository();
        }

        internal override Training GetDefaultAreaInstace()
        {
            return new Training();
        }

        internal override Training GetNonDefaultAreaInstace()
        {
            return new Training("testAreaEN", "testAreaDE", "taen", "tade", "testAreaAvatar");
        }
    }

    internal class WvWRepositoryTest : AbstractAreaRepositoryTest<WvW>
    {
        internal override AreaRepository<WvW> CreateEmptyRepository()
        {
            return new WvWAreaRepository();
        }

        internal override WvW GetDefaultAreaInstace()
        {
            return new WvW();
        }

        internal override WvW GetNonDefaultAreaInstace()
        {
            return new WvW("testAreaEN", "testAreaDE", "taen", "tade", "testAreaAvatar");
        }
    }

    internal class UnkowenGameAreaRepositoryTest : AbstractAreaRepositoryTest<UnkowenGameArea>
    {
        internal override AreaRepository<UnkowenGameArea> CreateEmptyRepository()
        {
            return new UnkowenAreaRepository();
        }

        internal override UnkowenGameArea GetDefaultAreaInstace()
        {
            return new UnkowenGameArea();
        }

        internal override UnkowenGameArea GetNonDefaultAreaInstace()
        {
            return new UnkowenGameArea("testAreaEN", "testAreaDE", "taen", "tade", "testAreaAvatar");
        }
    }

}
