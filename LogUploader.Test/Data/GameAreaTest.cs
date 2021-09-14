using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using System.Reflection;
using LogUploader.Test.Localisation;
using System.Security.AccessControl;

namespace LogUploader.Test.Data
{
    public class GameAreaTest
    {
        private int classesToTest = -1;
        private int everyGameAreaDefaultCount = -1;
        private int everyGameAreaSameArgsCount = -1;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            classesToTest = TestHelper.GetAllSubClassesOfType(typeof(GameArea)).Count();
            classesToTest -= 1; // Remove the TetGameArea from the count
            everyGameAreaDefaultCount = GetEveryGameAreaDefault().Count();
            everyGameAreaSameArgsCount = GetEveryGameAreaSameArgs().Count(); ;
        }

        public static IEnumerable<GameArea> GetEveryGameAreaDefault()
        {
            yield return new Training();
            yield return new WvW();
            yield return new UnkowenGameArea();
            yield return new Raid();
            yield return new Strike();
            yield return new Fractal();
            yield return new DragonResponseMission();
        }

        [Test, Combinatorial]
        public void AreasAreUnequalOverTypesDefaultTest(
            [ValueSource(nameof(GetEveryGameAreaDefault))] GameArea a,
            [ValueSource(nameof(GetEveryGameAreaDefault))] GameArea b)
        {
            if (everyGameAreaDefaultCount != classesToTest)
                Assert.Warn($"Nuber of subclasses in {nameof(GetEveryGameAreaDefault)} is not equal to the number of concret implementations of {nameof(GameArea)}.\n" +
                    $"Expected: {classesToTest}\n" +
                    $"Got: {everyGameAreaDefaultCount}");

            bool expected = (a.GetType().Equals(b.GetType()));

            Assert.That(a.Equals(b), Is.EqualTo(expected));
            Assert.That(b.Equals(a), Is.EqualTo(expected));

            Assert.That(a.GetHashCode().Equals(b.GetHashCode()), Is.EqualTo(expected));

            Assert.That(a == b, Is.EqualTo(expected));
            Assert.That(b == a, Is.EqualTo(expected));

            Assert.That(a != b, Is.Not.EqualTo(expected));
            Assert.That(b != a, Is.Not.EqualTo(expected));
        }

        public static IEnumerable<GameArea> GetEveryGameAreaSameArgs()
        {
            yield return new Training("nen", "nde", "snen", "snde", "avatar");
            yield return new WvW("nen", "nde", "snen", "snde", "avatar");
            yield return new UnkowenGameArea("nen", "nde", "snen", "snde", "avatar");
            yield return new Raid(1, "nen", "nde", "snen", "snde", "avatar");
            yield return new Strike(1, "nen", "nde", "snen", "snde", "avatar");
            yield return new Fractal(1, "nen", "nde", "snen", "snde", "avatar");
            yield return new DragonResponseMission(1, "nen", "nde", "snen", "snde", "avatar");
        }

        [Test, Combinatorial]
        public void AreasAreUnequalOverTypesSameArgsTest(
            [ValueSource(nameof(GetEveryGameAreaSameArgs))] GameArea a,
            [ValueSource(nameof(GetEveryGameAreaSameArgs))] GameArea b)
        {
            if (everyGameAreaSameArgsCount != classesToTest)
                Assert.Warn($"Nuber of subclasses in {nameof(GetEveryGameAreaSameArgs)} is not equal to the number of concret implementations of {nameof(GameArea)}.\n" +
                    $"Expected: {classesToTest}\n" +
                    $"Got: {everyGameAreaSameArgsCount}");

            bool expected = (a.GetType().Equals(b.GetType()));

            Assert.That(a.Equals(b), Is.EqualTo(expected));
            Assert.That(b.Equals(a), Is.EqualTo(expected));

            Assert.That(a == b, Is.EqualTo(expected));
            Assert.That(b == a, Is.EqualTo(expected));

            Assert.That(a != b, Is.Not.EqualTo(expected));
            Assert.That(b != a, Is.Not.EqualTo(expected));
        }

        [Test]
        public void AllAreasAreTestedTest()
        {
            int concreteTestClasses = TestHelper.GetAllSubClassesOfType(typeof(AbstractGameAreaTest)).Count();
            Assert.That(classesToTest, Is.EqualTo(concreteTestClasses), $"All concrete children of {nameof(GameArea)} should have exactly 1 concret testclass, " +
                $"that extends {nameof(GameAreaTest<GameArea>)}");
        }

    }

    public class AbstractGameAreaTest { };

    public abstract class GameAreaTest<T> : AbstractGameAreaTest where T : GameArea
    {
        internal virtual T CreateNumberedGameAarea(int number)
        {
            return CreateGameAarea(GetNumberdData(number));
        }
        internal abstract T CreateGameAarea((string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data);

        [Test]
        public void CreateGameAreaTest()
        {
            Assert.That(() => CreateNumberedGameAarea(1), Throws.Nothing);
        }

        [Test]
        public void GameAreaConstructorAssigenTest()
        {
            T area = CreateNumberedGameAarea(1);
            ValidateNumberedArea(area, 1);
        }

        [Test, Combinatorial]
        public void CreateGameAreaNullTest(
            [Values("nameEN", null)] string nameEN,
            [Values("nameDE", null)] string nameDE,
            [Values("shortNameEN", null)] string shortNameEN,
            [Values("shortNameDE", null)] string shortNameDE,
            [Values("avatarUrl", null)] string avatarUrl)
        {
            if (!(new string[] { nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl }).Contains(null))
                Assert.Pass("Not relevent for testcase, some null arguemnt requried.");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => CreateGameAarea((nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl))));
        }

        [Test]
        public void CreateGameAreaInvalidStringTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidName)
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateGameAarea((invalidName, "nameDE", "shortNameEN", "shortNameDE", "avatarUrl"))));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateGameAarea(("nameEN", invalidName, "shortNameEN", "shortNameDE", "avatarUrl"))));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateGameAarea(("nameEN", "nameDE", invalidName, "shortNameDE", "avatarUrl"))));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateGameAarea(("nameEN", "nameDE", "shortNameEN", invalidName, "avatarUrl"))));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => CreateGameAarea(("nameEN", "nameDE", "shortNameEN", "shortNameDE", invalidName))));
        }

        [Test]
        public void GameAreaGetShortNameTest()
        {
            var expcted = GetNumberdData(1);
            T area = CreateNumberedGameAarea(1);

            Assert.That(area.GetShortName(LogUploader.Localisation.eLanguage.EN), Is.EqualTo(expcted.shortNameEN));
            Assert.That(area.GetShortName(LogUploader.Localisation.eLanguage.DE), Is.EqualTo(expcted.shortNameDE));
        }

        [Test]
        public virtual void GameAreaEqualsTest()
        {
            T area1a = CreateNumberedGameAarea(1);
            T area1b = CreateNumberedGameAarea(1);
            T area1c = CreateNumberedGameAarea(1);
            T area2 = CreateNumberedGameAarea(2);
            
            Assert.That(area1a, Is.EqualTo(area1a), $"GameArea{nameof(T)} equals should be reflexive");
            Assert.That(area1a, Is.EqualTo(area1b), $"GameArea{nameof(T)} equals should be by value");
            Assert.That(area1b, Is.EqualTo(area1a), $"GameArea{nameof(T)} equals should be symmetric");
            Assert.That(area1b, Is.EqualTo(area1c), $"GameArea{nameof(T)} equals should be by value 2");
            Assert.That(area1a, Is.EqualTo(area1c), $"GameArea{nameof(T)} equals should be transitive");
            Assert.That(area1a, Is.Not.EqualTo(area2), $"GameArea{nameof(T)} should not equals");
            Assert.That(area2, Is.Not.EqualTo(area1a), $"GameArea{nameof(T)} should not equals symmetric");
            Assert.That(area1b, Is.Not.EqualTo(area2), $"GameArea{nameof(T)} should not equals transitive");
            Assert.That(area1c, Is.Not.EqualTo(area2), $"GameArea{nameof(T)} should not equals transitive 2");
        }

        [Test]
        public virtual void GameAreaGetHashCodeTest()
        {
            int area1a = CreateNumberedGameAarea(1).GetHashCode();
            int area1b = CreateNumberedGameAarea(1).GetHashCode();
            int area1c = CreateNumberedGameAarea(1).GetHashCode();
            int area2 = CreateNumberedGameAarea(2).GetHashCode();

            Assert.That(area1a, Is.EqualTo(area1a), $"GameArea{nameof(T)}.GetHashCode equals should be reflexive");
            Assert.That(area1a, Is.EqualTo(area1b), $"GameArea{nameof(T)}.GetHashCode equals should be by value");
            Assert.That(area1b, Is.EqualTo(area1a), $"GameArea{nameof(T)}.GetHashCode equals should be symmetric");
            Assert.That(area1b, Is.EqualTo(area1c), $"GameArea{nameof(T)}.GetHashCode equals should be by value 2");
            Assert.That(area1a, Is.EqualTo(area1c), $"GameArea{nameof(T)}.GetHashCode equals should be transitive");
            Assert.That(area1a, Is.Not.EqualTo(area2), $"GameArea{nameof(T)}.GetHashCode should not equals");
            Assert.That(area2, Is.Not.EqualTo(area1a), $"GameArea{nameof(T)}.GetHashCode should not equals symmetric");
            Assert.That(area1b, Is.Not.EqualTo(area2), $"GameArea{nameof(T)}.GetHashCode should not equals transitive");
            Assert.That(area1c, Is.Not.EqualTo(area2), $"GameArea{nameof(T)}.GetHashCode should not equals transitive 2");
        }

        [Test]
        public virtual void GameAreaEqualOperatorTest()
        {
            T area1a = CreateNumberedGameAarea(1);
            T area1b = CreateNumberedGameAarea(1);
            T area1c = CreateNumberedGameAarea(1);
            T area2 = CreateNumberedGameAarea(2);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.That(area1a == area1a, Is.True, $"Area{nameof(T)} '==' should be reflexive");
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.That(area1a == area1b, Is.True, $"Area{nameof(T)} '==' should be by value");
            Assert.That(area1b == area1a, Is.True, $"Area{nameof(T)} '==' should be symmetric");
            Assert.That(area1b == area1c, Is.True, $"Area{nameof(T)} '==' should be by value 2");
            Assert.That(area1a == area1c, Is.True, $"Area{nameof(T)} '==' should be transitive");

            Assert.That(area1a == area2, Is.False, $"Area{nameof(T)} should not equals");
            Assert.That(area2 == area1a, Is.False, $"Area{nameof(T)} should not equals symmetric");
            Assert.That(area1b == area2, Is.False, $"Area{nameof(T)} should not equals transitive");
            Assert.That(area1c == area2, Is.False, $"Area{nameof(T)} should not equals transitive 2");
        }

        [Test]
        public virtual void GameAreaUnEqualOperatorTest()
        {
            T area1a = CreateNumberedGameAarea(1);
            T area1b = CreateNumberedGameAarea(1);
            T area1c = CreateNumberedGameAarea(1);
            T area2 = CreateNumberedGameAarea(2);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.That(area1a != area1a, Is.False, $"Area{nameof(T)} '!=' should be reflexive");
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.That(area1a != area1b, Is.False, $"Area{nameof(T)} '!=' should be by value");
            Assert.That(area1b != area1a, Is.False, $"Area{nameof(T)} '!=' should be symmetric");
            Assert.That(area1b != area1c, Is.False, $"Area{nameof(T)} '!=' should be by value 2");
            Assert.That(area1a != area1c, Is.False, $"Area{nameof(T)} '!=' should be transitive");

            Assert.That(area1a != area2, Is.True, $"Area{nameof(T)} '!=' should not not equals");
            Assert.That(area2 != area1a, Is.True, $"Area{nameof(T)} '!=' should not not equals symmetric");
            Assert.That(area1b != area2, Is.True, $"Area{nameof(T)} '!=' should not not equals transitive");
            Assert.That(area1c != area2, Is.True, $"Area{nameof(T)} '!=' should not not equals transitive 2");
        }

        protected virtual void ValidateNumberedArea(T area, int number)
        {
            ValidateArea(area, GetNumberdData(number));
        }

        protected void ValidateArea(T area, (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            Assert.That(area.NameEN, Is.EqualTo(nameEN));
            Assert.That(area.NameDE, Is.EqualTo(nameDE));
            Assert.That(area.ShortNameEN, Is.EqualTo(shortNameEN));
            Assert.That(area.ShortNameDE, Is.EqualTo(shortNameDE));
            Assert.That(area.AvatarURL, Is.EqualTo(avatarUrl));
        }

        protected string GetNumberedString(string str, int number)
        {
            return $"{str}_{number}";
        }
        internal abstract ( string nameEN, string nameDE,string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number);
        internal ( string nameEN, string nameDE,string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number, string area)
        {
            return (
                GetNumberedString($"{area}AreaEN", number),
                GetNumberedString($"{area}AreaDE", number),
                GetNumberedString($"{area.Substring(0, 3)}EN", number),
                GetNumberedString($"{area.Substring(0, 3)}DE", number),
                GetNumberedString($"avatarURL{area}", number)
                );
        }
    }

    public class GameAreaTrainingTest : GameAreaTest<Training>
    {
        internal override Training CreateGameAarea((string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new Training(nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number) => GetNumberdData(number, nameof(Training));
    }

    public class GameAreaWvWTest : GameAreaTest<WvW>
    {
        internal override WvW CreateGameAarea((string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new WvW(nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number) => GetNumberdData(number, nameof(Training));
    }

    public class GameAreaUnkowenTest : GameAreaTest<UnkowenGameArea>
    {
        internal override UnkowenGameArea CreateGameAarea((string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new UnkowenGameArea(nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number) => GetNumberdData(number, nameof(Training));
    }

    public abstract class MultiGameAreaTest<T> : GameAreaTest<T> where T : MultiGameArea
    {
        internal override T CreateGameAarea((string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return CreateMultiGameAarea((1, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl));
        }

        internal override T CreateNumberedGameAarea(int number)
        {
            return CreateMultiGameAarea(GetNumberdMulitData(number));
        }

        internal abstract T CreateMultiGameAarea((int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data);

        protected override void ValidateNumberedArea(T area, int number)
        {
            ValidateArea(area, GetNumberdData(number));
        }

        protected void ValidateMultiArea(T area, (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            Assert.That(area.ID, Is.EqualTo(id));
            ValidateArea(area, (nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl));
        }

        internal override (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdData(int number)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = GetNumberdMulitData(number);
            return (nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal abstract (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number);
        internal (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number, string area)
        {
            (string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = GetNumberdData(number, area);
            return (number, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }
    }

    public class MultiGameAreaRaidTest : MultiGameAreaTest<Raid>
    {
        internal override Raid CreateMultiGameAarea((int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new Raid(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number) => GetNumberdMulitData(number, nameof(Raid));
    }

    public class MultiGameAreaStrikeTest : MultiGameAreaTest<Strike>
    {
        internal override Strike CreateMultiGameAarea((int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new Strike(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number) => GetNumberdMulitData(number, nameof(Strike));
    }

    public class MultiGameAreaFractalTest : MultiGameAreaTest<Fractal>
    {
        internal override Fractal CreateMultiGameAarea((int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new Fractal(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number) => GetNumberdMulitData(number, nameof(Fractal));
    }

    public class MultiGameAreaDragonResponseMissionTest : MultiGameAreaTest<DragonResponseMission>
    {
        internal override DragonResponseMission CreateMultiGameAarea((int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) data)
        {
            (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) = data;
            return new DragonResponseMission(id, nameEN, nameDE, shortNameEN, shortNameDE, avatarUrl);
        }

        internal override (int id, string nameEN, string nameDE, string shortNameEN, string shortNameDE, string avatarUrl) GetNumberdMulitData(int number) => GetNumberdMulitData(number, nameof(DragonResponseMission));
    }
}
