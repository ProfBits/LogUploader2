using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using LogUploader.Data;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using LogUploader.Test.Data;

namespace LogUploader.Test.Data
{
    public abstract class DictionaryBaseTests
    {
        [Test]
        public void GetElementTest([Values(1, 3)] int number) => GetElementTestImpl(number);
        public abstract void GetElementTestImpl(int number);

        [Test]
        public void AddElementTest([Values(1, 2, 5)] int number) => AddElementTestImpl(number);
        public abstract void AddElementTestImpl(int number);

        [Test]
        public void AddElementConflictTest() => AddElementConflictTestTestImpl();
        public abstract void AddElementConflictTestTestImpl();

        protected const int RemoveElementCollectionSize = 5;
        [Test]
        public void RemoveElementTest([Values(0, 2, 4)] int number) => RemoveElementTestImpl(number);
        public abstract void RemoveElementTestImpl(int number);

        [Test]
        public void ClearTest([Values(1, 2, 5)] int number) => ClearTestImpl(number);
        public abstract void ClearTestImpl(int number);

        [Test]
        public void GetNonExistingElementTest() => GetNonExistingElementTestImpl();
        public abstract void GetNonExistingElementTestImpl();

        [Test]
        public void GetFromEmptyTest() => GetFromEmptyTestImpl();
        public abstract void GetFromEmptyTestImpl();
    }

    public abstract class AbstractTwoKeyDictionaryTest : DictionaryBaseTests
    {
        protected const string KEY1 = "key1";
        protected const string KEY2 = "key2";
        protected const string VALUE = "value";

        protected virtual IMultiKeyBaseDictionary<string, string, string> CreateIMultiKeyBaseDictionary() => CreateDictionary();

        protected abstract MultiKeyDictionary<string, string, string> CreateDictionary();

        protected virtual IMultiKeyBaseDictionary<string, string, string> CreateIMultiKeyBaseDictionaryWithData(int count)
        {
            var collection =  new MultiKeyDictionary<string, string, string>();
            for (int i = 0; i < count; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i)), GetNumberedString(VALUE, i));
            }
            return collection;
        }

        [Test]
        public void CreateEmptyTest()
        {
            Assert.That(CreateIMultiKeyBaseDictionary().Count, Is.EqualTo(0));
        }

        [Test]
        public void CreateWithElementTest([Values(1, 3)] int number)
        {
            Assert.That(CreateIMultiKeyBaseDictionaryWithData(number).Count, Is.EqualTo(number));
        }

        public override void ClearTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData(number);
            collection.Clear();
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, 0)), Is.False);
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, 0)), Is.False);
        }

        public override void GetElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData(number);
            for (int i = 0; i < number; i++)
            {
                ContainsNumberedElement(collection, i);
            }
        }

        public override void AddElementTestImpl(int number)
        {
            var collection = CreateDictionary();
            for (int i = 0; i < number; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i)), GetNumberedString(VALUE, i));
                Assert.That(collection.Count, Is.EqualTo(i + 1));
                ContainsNumberedElement(collection, i);
            }
            collection.Add((GetNumberedString(KEY1, number), GetNumberedString(KEY2, number)), GetNumberedString(VALUE, number - 1));
            Assert.That(collection.Count, Is.EqualTo(number + 1));
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(VALUE, number - 1));
        }

        public override void RemoveElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData(RemoveElementCollectionSize);
            collection.Remove(key1: GetNumberedString(KEY1, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData(RemoveElementCollectionSize);
            collection.Remove(key2: GetNumberedString(KEY2, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, number)), Is.False);
        }

        public override void AddElementConflictTestTestImpl()
        {
            var collection = CreateDictionary();
            collection.Add(("x", "x"), "val");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x"), "valNeu")));
            ContainsElement(collection, "x", "x", "val");
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetNonExistingElementTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData(1);
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetFromEmptyTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionary();
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        protected void ContainsNumberedElement(IMultiKeyBaseDictionary<string, string, string> collection, int number)
        {
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(VALUE, number));
        }

        protected void ContainsElement(IMultiKeyBaseDictionary<string, string, string> collection, string key1, string key2, string value)
        {
            Assert.That(collection.Get(key1: key1), Is.EqualTo(value));
            Assert.That(collection.Get(key2: key2), Is.EqualTo(value));
        }

        protected string GetNumberedString(string str, int number)
        {
            return $"{str}_{number}";
        }
    }

    public abstract class AbstractTreeKeyDictionaryTest : TwoKeyDictionaryTest
    {
        protected const string KEY3 = "key3";

        protected override IMultiKeyBaseDictionary<string, string, string> CreateIMultiKeyBaseDictionary() => CreateIMultiKeyBaseDictionary3();
        protected virtual IMultiKeyBaseDictionary<string, string, string, string> CreateIMultiKeyBaseDictionary3() => CreateDictionary3();
        protected abstract MultiKeyDictionary<string, string, string, string> CreateDictionary3();

        protected override IMultiKeyBaseDictionary<string, string, string> CreateIMultiKeyBaseDictionaryWithData(int count) => CreateIMultiKeyBaseDictionaryWithData3(count);
        protected virtual IMultiKeyBaseDictionary<string, string, string, string> CreateIMultiKeyBaseDictionaryWithData3(int count)
        {
            var collection = new MultiKeyDictionary<string, string, string, string>();
            for (int i = 0; i < count; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i)), GetNumberedString(VALUE, i));
            }
            return collection;
        }

        public override void ClearTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData3(number);
            collection.Clear();
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, 0)), Is.False);
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, 0)), Is.False);
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, 0)), Is.False);
        }


        public override void GetElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData3(number);
            for (int i = 0; i < number; i++)
            {
                ContainsNumberedElement(collection, i);
            }
        }

        public override void AddElementTestImpl(int number)
        {
            var collection = CreateDictionary3();
            for (int i = 0; i < number; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i)), GetNumberedString(VALUE, i));
                Assert.That(collection.Count, Is.EqualTo(i + 1));
                ContainsNumberedElement(collection, i);
            }
            collection.Add((GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number)), GetNumberedString(VALUE, number - 1));
            Assert.That(collection.Count, Is.EqualTo(number + 1));
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(VALUE, number - 1));
        }

        public override void RemoveElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData3(RemoveElementCollectionSize);
            collection.Remove(key1: GetNumberedString(KEY1, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData3(RemoveElementCollectionSize);
            collection.Remove(key2: GetNumberedString(KEY2, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData3(RemoveElementCollectionSize);
            collection.Remove(key3: GetNumberedString(KEY3, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, number)), Is.False);
        }

        public override void AddElementConflictTestTestImpl()
        {
            var collection = CreateDictionary3();
            collection.Add(("x", "x", "x"), "val");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a"), "valnei")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a"), "valnei")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a"), "valnei")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x"), "valnei")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x"), "valnei")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x"), "valnei")));
            ContainsElement(collection, "x", "x", "x", "val");
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetNonExistingElementTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData3(1);
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetFromEmptyTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionary3();
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        protected void ContainsNumberedElement(IMultiKeyBaseDictionary<string, string, string, string> collection, int number)
        {
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(VALUE, number));
        }

        protected void ContainsElement(IMultiKeyBaseDictionary<string, string, string, string> collection, string key1, string key2, string key3, string value)
        {
            Assert.That(collection.Get(key3: key3), Is.EqualTo(value));
            base.ContainsElement(collection, key1, key2, value);
        }
    }

    public abstract class AbstractFourKeyDictionaryTest : TreeKeyDictionaryTest
    {
        protected const string KEY4 = "key4";

        protected override IMultiKeyBaseDictionary<string, string, string, string> CreateIMultiKeyBaseDictionary3() => CreateIMultiKeyBaseDictionary4();
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string> CreateIMultiKeyBaseDictionary4() => CreateDictionary4();
        protected abstract MultiKeyDictionary<string, string, string, string, string> CreateDictionary4();

        protected override IMultiKeyBaseDictionary<string, string, string, string> CreateIMultiKeyBaseDictionaryWithData3(int count) => CreateIMultiKeyBaseDictionaryWithData4(count);
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string> CreateIMultiKeyBaseDictionaryWithData4(int count)
        {
            var collection = new MultiKeyDictionary<string, string, string, string, string>();
            for (int i = 0; i < count; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i)), GetNumberedString(VALUE, i));
            }
            return collection;
        }

        public override void ClearTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData4(number);
            collection.Clear();
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, 0)), Is.False);
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, 0)), Is.False);
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, 0)), Is.False);
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, 0)), Is.False);
        }

        public override void GetElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData4(number);
            for (int i = 0; i < number; i++)
            {
                ContainsNumberedElement(collection, i);
            }
        }

        public override void AddElementTestImpl(int number)
        {
            var collection = CreateDictionary4();
            for (int i = 0; i < number; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i)), GetNumberedString(VALUE, i));
                Assert.That(collection.Count, Is.EqualTo(i + 1));
                ContainsNumberedElement(collection, i);
            }
            collection.Add((GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number)), GetNumberedString(VALUE, number - 1));
            Assert.That(collection.Count, Is.EqualTo(number + 1));
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(VALUE, number - 1));
        }

        public override void RemoveElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData4(RemoveElementCollectionSize);
            collection.Remove(key1: GetNumberedString(KEY1, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData4(RemoveElementCollectionSize);
            collection.Remove(key2: GetNumberedString(KEY2, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData4(RemoveElementCollectionSize);
            collection.Remove(key3: GetNumberedString(KEY3, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData4(RemoveElementCollectionSize);
            collection.Remove(key4: GetNumberedString(KEY4, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, number)), Is.False);
        }

        public override void AddElementConflictTestTestImpl()
        {
            var collection = CreateDictionary4();
            collection.Add(("x", "x", "x", "x"), "val");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x"), "valNeu")));
            ContainsElement(collection, "x", "x", "x", "x", "val");
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetNonExistingElementTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData4(1);
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetFromEmptyTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionary4();
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        protected void ContainsNumberedElement(IMultiKeyBaseDictionary<string, string, string, string, string> collection, int number)
        {
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(VALUE, number));
        }

        protected void ContainsElement(IMultiKeyBaseDictionary<string, string, string, string, string> collection, string key1, string key2, string key3, string key4, string value)
        {
            Assert.That(collection.Get(key4: key4), Is.EqualTo(value));
            base.ContainsElement(collection, key1, key2, key3, value);
        }
    }

    public abstract class AbstractFiveKeyDictionaryTest : FourKeyDictionaryTest
    {
        protected const string KEY5 = "key5";

        protected override IMultiKeyBaseDictionary<string, string, string, string, string> CreateIMultiKeyBaseDictionary4() => CreateIMultiKeyBaseDictionary5();
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string, string> CreateIMultiKeyBaseDictionary5() => CreateDictionary5();
        protected abstract MultiKeyDictionary<string, string, string, string, string, string> CreateDictionary5();

        protected override IMultiKeyBaseDictionary<string, string, string, string, string> CreateIMultiKeyBaseDictionaryWithData4(int count) => CreateIMultiKeyBaseDictionaryWithData5(count);
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string, string> CreateIMultiKeyBaseDictionaryWithData5(int count)
        {
            var collection = new MultiKeyDictionary<string, string, string, string, string, string>();
            for (int i = 0; i < count; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i), GetNumberedString(KEY5, i)), GetNumberedString(VALUE, i));
            }
            return collection;
        }

        public override void ClearTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData5(number);
            collection.Clear();
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, 0)), Is.False);
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, 0)), Is.False);
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, 0)), Is.False);
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, 0)), Is.False);
            Assert.That(collection.ContainsKey(key5: GetNumberedString(KEY5, 0)), Is.False);
        }

        public override void GetElementTestImpl([Values(1, 3)] int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData5(number);
            for (int i = 0; i < number; i++)
            {
                ContainsNumberedElement(collection, i);
            }
        }

        public override void AddElementTestImpl(int number)
        {
            var collection = CreateDictionary5();
            for (int i = 0; i < number; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i), GetNumberedString(KEY5, i)), GetNumberedString(VALUE, i));
                Assert.That(collection.Count, Is.EqualTo(i + 1));
                ContainsNumberedElement(collection, i);
            }
            collection.Add((GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number)), GetNumberedString(VALUE, number - 1));
            Assert.That(collection.Count, Is.EqualTo(number + 1));
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number), GetNumberedString(VALUE, number - 1));
        }
        public override void RemoveElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData5(RemoveElementCollectionSize);
            collection.Remove(key1: GetNumberedString(KEY1, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData5(RemoveElementCollectionSize);
            collection.Remove(key2: GetNumberedString(KEY2, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData5(RemoveElementCollectionSize);
            collection.Remove(key3: GetNumberedString(KEY3, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData5(RemoveElementCollectionSize);
            collection.Remove(key4: GetNumberedString(KEY4, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData5(RemoveElementCollectionSize);
            collection.Remove(key5: GetNumberedString(KEY5, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key5: GetNumberedString(KEY5, number)), Is.False);
        }

        public override void AddElementConflictTestTestImpl()
        {
            var collection = CreateDictionary5();
            collection.Add(("x", "x", "x", "x", "x"), "val");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "x"), "valNeu")));
            ContainsElement(collection, "x", "x", "x", "x", "x", "val");
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetNonExistingElementTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData5(1);
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key5: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(1));
        }

        public override void GetFromEmptyTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionary5();
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key5: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(0));
        }

        protected void ContainsNumberedElement(IMultiKeyBaseDictionary<string, string, string, string, string, string> collection, int number)
        {
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number), GetNumberedString(VALUE, number));
        }

        protected void ContainsElement(IMultiKeyBaseDictionary<string, string, string, string, string, string> collection, string key1, string key2, string key3, string key4, string key5, string value)
        {
            Assert.That(collection.Get(key5: key5), Is.EqualTo(value));
            base.ContainsElement(collection, key1, key2, key3, key4, value);
        }
    }

    public abstract class AbstractSixKeyDictionaryTest : FiveKeyDictionaryTest
    {
        protected const string KEY6 = "key6";
    
        protected override IMultiKeyBaseDictionary<string, string, string, string, string, string> CreateIMultiKeyBaseDictionary5() => CreateIMultiKeyBaseDictionary6();
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string, string, string> CreateIMultiKeyBaseDictionary6() => CreateDictionary6();
        protected abstract MultiKeyDictionary<string, string, string, string, string, string, string> CreateDictionary6();
    
        protected override IMultiKeyBaseDictionary<string, string, string, string, string, string> CreateIMultiKeyBaseDictionaryWithData5(int count) => CreateIMultiKeyBaseDictionaryWithData6(count);
        protected virtual IMultiKeyBaseDictionary<string, string, string, string, string, string, string> CreateIMultiKeyBaseDictionaryWithData6(int count)
        {
            var collection = new MultiKeyDictionary<string, string, string, string, string, string, string>();
            for (int i = 0; i < count; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i), GetNumberedString(KEY5, i), GetNumberedString(KEY6, i)), GetNumberedString(VALUE, i));
            }
            return collection;
        }
    
        public override void ClearTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData6(number);
            collection.Clear();
            Assert.That(collection.Count, Is.EqualTo(0));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, 0)), Is.False);
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, 0)), Is.False);
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, 0)), Is.False);
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, 0)), Is.False);
            Assert.That(collection.ContainsKey(key5: GetNumberedString(KEY5, 0)), Is.False);
            Assert.That(collection.ContainsKey(key6: GetNumberedString(KEY6, 0)), Is.False);
        }
    
        public override void GetElementTestImpl([Values(1, 3)] int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData6(number);
            for (int i = 0; i < number; i++)
            {
                ContainsNumberedElement(collection, i);
            }
        }
    
        public override void AddElementTestImpl(int number)
        {
            var collection = CreateDictionary6();
            for (int i = 0; i < number; i++)
            {
                collection.Add((GetNumberedString(KEY1, i), GetNumberedString(KEY2, i), GetNumberedString(KEY3, i), GetNumberedString(KEY4, i), GetNumberedString(KEY5, i), GetNumberedString(KEY6, i)), GetNumberedString(VALUE, i));
                Assert.That(collection.Count, Is.EqualTo(i + 1));
                ContainsNumberedElement(collection, i);
            }
            collection.Add((GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number), GetNumberedString(KEY6, number)), GetNumberedString(VALUE, number - 1));
            Assert.That(collection.Count, Is.EqualTo(number + 1));
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number), GetNumberedString(KEY6, number), GetNumberedString(VALUE, number - 1));
        }
        public override void RemoveElementTestImpl(int number)
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key1: GetNumberedString(KEY1, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key1: GetNumberedString(KEY1, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key2: GetNumberedString(KEY2, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key2: GetNumberedString(KEY2, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key3: GetNumberedString(KEY3, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key3: GetNumberedString(KEY3, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key4: GetNumberedString(KEY4, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key4: GetNumberedString(KEY4, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key5: GetNumberedString(KEY5, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key5: GetNumberedString(KEY5, number)), Is.False);
            collection = CreateIMultiKeyBaseDictionaryWithData6(RemoveElementCollectionSize);
            collection.Remove(key6: GetNumberedString(KEY6, number));
            Assert.That(collection.Count, Is.EqualTo(RemoveElementCollectionSize - 1));
            Assert.That(collection.ContainsKey(key6: GetNumberedString(KEY6, number)), Is.False);
        }
    
        public override void AddElementConflictTestTestImpl()
        {
            var collection = CreateDictionary6();
            collection.Add(("x", "x", "x", "x", "x", "x"), "val");
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "a", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "x", "a"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "a", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "a", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "a", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "a", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "a", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "a", "x", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("a", "x", "x", "x", "x", "x"), "valNeu")));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentException>(() => collection.Add(("x", "x", "x", "x", "x", "x"), "valNeu")));
            ContainsElement(collection, "x", "x", "x", "x", "x", "x", "val");
            Assert.That(collection.Count, Is.EqualTo(1));
        }
    
        public override void GetNonExistingElementTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionaryWithData6(1);
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key5: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key6: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(1));
        }
    
        public override void GetFromEmptyTestImpl()
        {
            var collection = CreateIMultiKeyBaseDictionary6();
            var ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key1: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key2: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key3: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key4: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key5: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            ex = Assert.Catch<KeyNotFoundException>(() => collection.Get(key6: "notAKey"));
            Assert.That(string.IsNullOrWhiteSpace(ex.Message), Is.False);
            Assert.That(collection.Count, Is.EqualTo(0));
        }
    
        protected void ContainsNumberedElement(IMultiKeyBaseDictionary<string, string, string, string, string, string, string> collection, int number)
        {
            ContainsElement(collection, GetNumberedString(KEY1, number), GetNumberedString(KEY2, number), GetNumberedString(KEY3, number), GetNumberedString(KEY4, number), GetNumberedString(KEY5, number), GetNumberedString(KEY6, number), GetNumberedString(VALUE, number));
        }
    
        protected void ContainsElement(IMultiKeyBaseDictionary<string, string, string, string, string, string, string> collection, string key1, string key2, string key3, string key4, string key5, string key6, string value)
        {
            Assert.That(collection.Get(key6: key6), Is.EqualTo(value));
            base.ContainsElement(collection, key1, key2, key3, key4, key5, value);
        }
    }

    public class TwoKeyDictionaryTest : AbstractTwoKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string> CreateDictionary()
        {
            return new MultiKeyDictionary<string, string, string>();
        }
    }

    public class TreeKeyDictionaryTest : AbstractTreeKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string> CreateDictionary3()
        {
            return new MultiKeyDictionary<string, string, string, string>();
        }
    }

    public class FourKeyDictionaryTest : AbstractFourKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string> CreateDictionary4()
        {
            return new MultiKeyDictionary<string, string, string, string, string>();
        }
    }

    public class FiveKeyDictionaryTest : AbstractFiveKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string, string> CreateDictionary5()
        {
            return new MultiKeyDictionary<string, string, string, string, string, string>();
        }
    }

    public class SixKeyDictionaryTest : AbstractSixKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string, string, string> CreateDictionary6()
        {
            return new MultiKeyDictionary<string, string, string, string, string, string, string>();
        }
    }


    public class TwoKeyInValueDictionaryTest : AbstractTwoKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string> CreateDictionary()
        {
            return CreateInValueDictionary();
        }

        private static MultiKeyInValueDictionary<string, string, string> CreateInValueDictionary(IEnumerable<string> initData = null)
        {
            if (initData is null)
                return new MultiKeyInValueDictionary<string, string, string>(
                    s => $"{s[0]}",
                    s => $"{s[1]}"
                    );
            return new MultiKeyInValueDictionary<string, string, string>(initData,
                s => $"{s[0]}",
                s => $"{s[1]}"
                );
        }

        [Test]
        public void AddViaKeyMapperTest()
        {
            var collection = CreateInValueDictionary();
            collection.Add(value: "xxVal");
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxVal"));
        }

        [Test]
        public void AddConstructorTest()
        {
            var emptyCollection = CreateInValueDictionary(new string[] { });
            Assert.That(emptyCollection.Count, Is.EqualTo(0));
            var collection = CreateInValueDictionary(new string[] { "xxVal" });
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxVal"));
        }

        [Test]
        public void CreateWithNullKeyMapperTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(s => s, null)));

            string[] arr = new string[] { };
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(arr, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(arr, s => s, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(null, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string>(null, s => s, null)));
        }
    }

    public class TreeKeyInValueDictionaryTest : AbstractTreeKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string> CreateDictionary3()
        {
            return CreateInValueDictionary();
        }

        private static MultiKeyInValueDictionary<string, string, string, string> CreateInValueDictionary(IEnumerable<string> initData = null)
        {
            if (initData is null)
                return new MultiKeyInValueDictionary<string, string, string, string>(
                    s => $"{s[0]}",
                    s => $"{s[1]}",
                    s => $"{s[2]}"
                    );
            return new MultiKeyInValueDictionary<string, string, string, string>(initData,
                s => $"{s[0]}",
                s => $"{s[1]}",
                s => $"{s[2]}"
                );
        }

        [Test]
        public void AddViaKeyMapperTest()
        {
            var collection = CreateInValueDictionary();
            collection.Add(value: "xxxVal");
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxVal"));
        }

        [Test]
        public void AddConstructorTest()
        {
            var emptyCollection = CreateInValueDictionary(new string[] { });
            Assert.That(emptyCollection.Count, Is.EqualTo(0));
            var collection = CreateInValueDictionary(new string[] { "xxxVal" });
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxVal"));
        }

        [Test]
        public void CreateWithNullKeyMapperTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(s => s, s => s, null)));
            string[] arr = new string[] { };
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(arr, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(arr, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(arr, s => s, s => s, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(null, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(null, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string>(null, s => s, s => s, null)));
        }
    }

    public class FourKeyInValueDictionaryTest : AbstractFourKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string> CreateDictionary4()
        {
            return CreateInValueDictionary();
        }

        private static MultiKeyInValueDictionary<string, string, string, string, string> CreateInValueDictionary(IEnumerable<string> initData = null)
        {
            if (initData is null)
                return new MultiKeyInValueDictionary<string, string, string, string, string>(
                s => $"{s[0]}",
                s => $"{s[1]}",
                s => $"{s[2]}",
                s => $"{s[3]}"
                );
            return new MultiKeyInValueDictionary<string, string, string, string, string>(initData,
            s => $"{s[0]}",
            s => $"{s[1]}",
            s => $"{s[2]}",
            s => $"{s[3]}"
            );
        }

        [Test]
        public void AddViaKeyMapperTest()
        {
            var collection = CreateInValueDictionary();
            collection.Add(value: "xxxxVal");
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxVal"));
        }

        [Test]
        public void AddConstructorTest()
        {
            var emptyCollection = CreateInValueDictionary(new string[] { });
            Assert.That(emptyCollection.Count, Is.EqualTo(0));
            var collection = CreateInValueDictionary(new string[] { "xxxxVal" });
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxVal"));
        }

        [Test]
        public void CreateWithNullKeyMapperTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(s => s, s => s, s => s, null)));
            string[] arr = new string[] { };
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(arr, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(arr, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(arr, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(arr, s => s, s => s, s => s, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(null, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(null, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(null, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string>(null, s => s, s => s, s => s, null)));
        }
    }

    public class FiveKeyInValueDictionaryTest : AbstractFiveKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string, string> CreateDictionary5()
        {
            return CreateInValueDictionary();
        }

        private static MultiKeyInValueDictionary<string, string, string, string, string, string> CreateInValueDictionary(IEnumerable<string> initData = null)
        {
            if (initData is null)
                return new MultiKeyInValueDictionary<string, string, string, string, string, string>(
                s => $"{s[0]}",
                s => $"{s[1]}",
                s => $"{s[2]}",
                s => $"{s[3]}",
                s => $"{s[4]}"
                );
            return new MultiKeyInValueDictionary<string, string, string, string, string, string>(initData,
            s => $"{s[0]}",
            s => $"{s[1]}",
            s => $"{s[2]}",
            s => $"{s[3]}",
            s => $"{s[4]}"
            );
        }

        [Test]
        public void AddViaKeyMapperTest()
        {
            var collection = CreateInValueDictionary();
            collection.Add(value: "xxxxxVal");
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key5: "x"), Is.EqualTo("xxxxxVal"));
        }

        [Test]
        public void AddConstructorTest()
        {
            var emptyCollection = CreateInValueDictionary(new string[] { });
            Assert.That(emptyCollection.Count, Is.EqualTo(0));
            var collection = CreateInValueDictionary(new string[] { "xxxxxVal" });
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxxVal"));
            Assert.That(collection.Get(key5: "x"), Is.EqualTo("xxxxxVal"));
        }


        [Test]
        public void CreateWithNullKeyMapperTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(s => s, s => s, s => s, s => s, null)));
            string[] arr = new string[] { };
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(arr, null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(arr, s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(arr, s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(arr, s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(arr, s => s, s => s, s => s, s => s, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string>(null, s => s, s => s, s => s, s => s, null)));
        }
    }

    public class SixKeyInValueDictionaryTest : AbstractSixKeyDictionaryTest
    {
        protected override MultiKeyDictionary<string, string, string, string, string, string, string> CreateDictionary6()
        {
            return CreateInValueDictionary();
        }
    
        private static MultiKeyInValueDictionary<string, string, string, string, string, string, string> CreateInValueDictionary(IEnumerable<string> initData = null)
        {
            if (initData is null)
                return new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(
                s => $"{s[0]}",
                s => $"{s[1]}",
                s => $"{s[2]}",
                s => $"{s[3]}",
                s => $"{s[4]}",
                s => $"{s[5]}"
                );
            return new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(initData,
            s => $"{s[0]}",
            s => $"{s[1]}",
            s => $"{s[2]}",
            s => $"{s[3]}",
            s => $"{s[4]}",
            s => $"{s[5]}"
            );
        }
    
        [Test]
        public void AddViaKeyMapperTest()
        {
            var collection = CreateInValueDictionary();
            collection.Add(value: "xxxxxxVal");
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key5: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key6: "x"), Is.EqualTo("xxxxxxVal"));
        }
    
        [Test]
        public void AddConstructorTest()
        {
            var emptyCollection = CreateInValueDictionary(new string[] { });
            Assert.That(emptyCollection.Count, Is.EqualTo(0));
            var collection = CreateInValueDictionary(new string[] { "xxxxxxVal" });
            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection.Get(key1: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key2: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key3: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key4: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key5: "x"), Is.EqualTo("xxxxxxVal"));
            Assert.That(collection.Get(key6: "x"), Is.EqualTo("xxxxxxVal"));
        }
    
    
        [Test]
        public void CreateWithNullKeyMapperTest()
        {
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(s => s, null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(s => s, s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(s => s, s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(s => s, s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(s => s, s => s, s => s, s => s, s => s, null)));
            string[] arr = new string[] { };
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, null, s => s, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, s => s, null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, s => s, s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, s => s, s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, s => s, s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(arr, s => s, s => s, s => s, s => s, s => s, null)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, null, s => s, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, null, s => s, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, s => s, null, s => s, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, s => s, s => s, null, s => s, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, s => s, s => s, s => s, null, s => s)));
            TestHelper.ValidateArugumentException(Assert.Catch<ArgumentNullException>(() => new MultiKeyInValueDictionary<string, string, string, string, string, string, string>(null, s => s, s => s, s => s, s => s, s => s, null)));
        }
    }
}
