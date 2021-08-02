using System;
using System.Collections.Generic;

using NUnit.Framework;

using LogUploader.Data;
using LogUploader.Data.Repositories;

namespace LogUploader.Test.Data
{
    internal abstract class AbstractMultiAreaRepositoryTest<T> : AreaRepositoryTest where T : MultiGameArea
    {
        internal abstract MultiAreaRepository<T> CreateEmptyRepository();
        internal abstract T CreateNumberedArea(int number);

        [Test]
        public void MultiAreaRepositoryCreateDoesNotThrow()
        {
            Assert.That(() => CreateEmptyRepository(), Throws.Nothing);

            Assert.That(() => CreateNumberedArea(1), Throws.Nothing);
            Assert.That(() => CreateEmptyRepository(), Is.Not.Null);
            Assert.That(() => CreateNumberedArea(1), Is.Not.Null);
        }

        [Test]
        public void MultiAreaRepositoryGetThrowsWhenEmpty()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            MultiAreaProvider<T> provider = repo;

            Assert.Catch<KeyNotFoundException>(() => provider.Get(0));
            Assert.Catch<KeyNotFoundException>(() => provider.Get(1));
        }

        [Test]
        public void MultiAreaRepositoryGetNonExistingEntryThrows()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            T area = CreateNumberedArea(2);
            repo.Add(area);
            MultiAreaProvider<T> provider = repo;

            Assert.Catch<KeyNotFoundException>(() => provider.Get(3));
        }

        [Test]
        public void MultiAreaRepositoryGetExistingEntry()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            T areaA = CreateNumberedArea(2);
            T areaB = CreateNumberedArea(3);
            MultiAreaProvider<T> provider = repo;

            repo.Add(areaA);
            Assert.That(() => provider.Get(areaA.ID), Throws.Nothing);
            Assert.That(provider.Get(areaA.ID), Is.EqualTo(areaA));

            repo.Add(areaB);
            Assert.That(() => provider.Get(areaB.ID), Throws.Nothing);
            Assert.That(provider.Get(areaB.ID), Is.EqualTo(areaB));
        }

        [Test]
        public void MultiAreaRepositoryAddDuplicateThrows()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            T areaA = CreateNumberedArea(2);
            T areaB = CreateNumberedArea(areaA.ID);

            repo.Add(areaA);
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => repo.Add(areaB)));
        }

        [Test]
        public void MultiAreaRepositoryAddNullThrows()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();

            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => repo.Add(null)));
        }

        [Test]
        public void MultiAreaRepositoryExistsWhenEmptyIsFalse([Values(0, 1, 3)] int number)
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            MultiAreaProvider<T> provider = repo;

            Assert.That(() => provider.Exists(number), Throws.Nothing);
            Assert.That(provider.Exists(number), Is.False);
        }

        [Test]
        public void MultiAreaRepositoryExists()
        {
            MultiAreaRepository<T> repo = CreateEmptyRepository();
            T areaA = CreateNumberedArea(2);
            T areaB = CreateNumberedArea(3);
            MultiAreaProvider<T> provider = repo;

            repo.Add(areaA);
            Assert.That(() => provider.Exists(areaA.ID), Throws.Nothing);
            Assert.That(provider.Exists(areaA.ID), Is.True);
            Assert.That(provider.Exists(areaB.ID), Is.False);

            repo.Add(areaB);
            Assert.That(provider.Exists(areaA.ID), Is.True);
            Assert.That(provider.Exists(areaB.ID), Is.True);
        }
    }

    internal class RaidRepositoryTest : AbstractMultiAreaRepositoryTest<Raid>
    {
        internal override MultiAreaRepository<Raid> CreateEmptyRepository()
        {
            return new RaidRepository();
        }

        internal override Raid CreateNumberedArea(int number)
        {
            return new Raid(number, $"raidEN_{number}", $"raidDE_{number}", "raidAvatar");
        }
    }

    internal class StrikeRepositoryTest : AbstractMultiAreaRepositoryTest<Strike>
    {
        internal override MultiAreaRepository<Strike> CreateEmptyRepository()
        {
            return new StrikeRepository();
        }

        internal override Strike CreateNumberedArea(int number)
        {
            return new Strike(number, $"strikeEN_{number}", $"strikeDE_{number}", "strikeAvatar");
        }
    }

    internal class FractalRepositoryTest : AbstractMultiAreaRepositoryTest<Fractal>
    {
        internal override MultiAreaRepository<Fractal> CreateEmptyRepository()
        {
            return new FractalRepository();
        }

        internal override Fractal CreateNumberedArea(int number)
        {
            return new Fractal(number, $"fractalEN_{number}", $"fractalDE_{number}", "fractalAvatar");
        }
    }

    internal class DrmRepositoryTest : AbstractMultiAreaRepositoryTest<DragonResponseMission>
    {
        internal override MultiAreaRepository<DragonResponseMission> CreateEmptyRepository()
        {
            return new DragonResponseMissionRepository();
        }

        internal override DragonResponseMission CreateNumberedArea(int number)
        {
            return new DragonResponseMission(number, $"drmEN_{number}", $"drmDE_{number}", "drmAvatar");
        }
    }
}
