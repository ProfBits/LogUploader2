using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

namespace LogUploader.Data
{
    public interface IFiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        void Add((TKey1, TKey2, TKey3, TKey4, TKey5) key, TValue value);
        void Clear();
        TValue Get(TKey1 key1);
        TValue Get(TKey2 key2);
        TValue Get(TKey3 key3);
        TValue Get(TKey4 key4);
        TValue Get(TKey5 key5);
        bool Remove(TKey1 key1);
        bool Remove(TKey2 key2);
        bool Remove(TKey3 key3);
        bool Remove(TKey4 key4);
        bool Remove(TKey5 key5);
    }

    public class FiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : IFiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        private readonly List<TKey1> Key1 = new List<TKey1>();
        private readonly List<TKey2> Key2 = new List<TKey2>();
        private readonly List<TKey3> Key3 = new List<TKey3>();
        private readonly List<TKey4> Key4 = new List<TKey4>();
        private readonly List<TKey5> Key5 = new List<TKey5>();
        private readonly List<TValue> Values = new List<TValue>();

        public void Add((TKey1, TKey2, TKey3, TKey4, TKey5) key, TValue value)
        {
            (TKey1 key1, TKey2 key2, TKey3 key3, TKey4 key4, TKey5 key5) = key;
            if (Key1.Contains(key1)) throw new ArgumentException("Cannot add, Key1 alread exists", nameof(key1));
            if (Key2.Contains(key2)) throw new ArgumentException("Cannot add, Key2 alread exists", nameof(key2));
            if (Key3.Contains(key3)) throw new ArgumentException("Cannot add, Key3 alread exists", nameof(key3));
            if (Key4.Contains(key4)) throw new ArgumentException("Cannot add, Key4 alread exists", nameof(key4));
            if (Key5.Contains(key5)) throw new ArgumentException("Cannot add, Key5 alread exists", nameof(key5));

            Key1.Add(key1);
            Key2.Add(key2);
            Key3.Add(key3);
            Key4.Add(key4);
            Key5.Add(key5);

            Values.Add(value);
        }

        public TValue Get(TKey1 key1)
        {
            var index = Key1.IndexOf(key1);
            if (index == -1) throw new KeyNotFoundException();
            return Values[index];
        }

        public TValue Get(TKey2 key2)
        {
            var index = Key2.IndexOf(key2);
            if (index == -1) throw new KeyNotFoundException();
            return Values[index];
        }

        public TValue Get(TKey3 key3)
        {
            var index = Key3.IndexOf(key3);
            if (index == -1) throw new KeyNotFoundException();
            return Values[index];
        }

        public TValue Get(TKey4 key4)
        {
            var index = Key4.IndexOf(key4);
            if (index == -1) throw new KeyNotFoundException();
            return Values[index];
        }

        public TValue Get(TKey5 key5)
        {
            var index = Key5.IndexOf(key5);
            if (index == -1) throw new KeyNotFoundException();
            return Values[index];
        }

        public bool Remove(TKey1 key1)
        {
            var index = Key1.IndexOf(key1);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public bool Remove(TKey2 key2)
        {
            var index = Key2.IndexOf(key2);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public bool Remove(TKey3 key3)
        {
            var index = Key3.IndexOf(key3);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public bool Remove(TKey4 key4)
        {
            var index = Key4.IndexOf(key4);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public bool Remove(TKey5 key5)
        {
            var index = Key5.IndexOf(key5);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        private void RemoveAt(int index)
        {
            Key1.RemoveAt(index);
            Key2.RemoveAt(index);
            Key3.RemoveAt(index);
            Key4.RemoveAt(index);
            Key5.RemoveAt(index);
            Values.RemoveAt(index);
        }

        public void Clear()
        {
            Key1.Clear();
            Key2.Clear();
            Key3.Clear();
            Key4.Clear();
            Key5.Clear();
            Values.Clear();
        }

    }

    public class FiveKeyInValueDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : IFiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        IFiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> Data = new FiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>();

        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;
        readonly Func<TValue, TKey3> Key3Generator;
        readonly Func<TValue, TKey4> Key4Generator;
        readonly Func<TValue, TKey5> Key5Generator;

        public FiveKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator) :
            this(new FiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>(), key1Generator, key2Generator, key3Generator, key4Generator, key5Generator)
        { }

        public FiveKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator) :
            this(new FiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>(), key1Generator, key2Generator, key3Generator, key4Generator, key5Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public FiveKeyInValueDictionary(IFiveKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> bakingDict, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator)
        {
            Data = bakingDict;
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
            Key3Generator = key3Generator ?? throw new ArgumentNullException(nameof(key3Generator));
            Key4Generator = key4Generator ?? throw new ArgumentNullException(nameof(key4Generator));
            Key5Generator = key5Generator ?? throw new ArgumentNullException(nameof(key5Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value), Key3Generator(value), Key4Generator(value), Key5Generator(value));
            Data.Add(key, value);
        }

        public void Add((TKey1, TKey2, TKey3, TKey4, TKey5) key, TValue value)
        {
            Data.Add(key, value);
        }

        public TValue Get(TKey1 key1) => Data.Get(key1: key1);
        public TValue Get(TKey2 key2) => Data.Get(key2: key2);
        public TValue Get(TKey3 key3) => Data.Get(key3: key3);
        public TValue Get(TKey4 key4) => Data.Get(key4: key4);
        public TValue Get(TKey5 key5) => Data.Get(key5: key5);

        public bool Remove(TKey1 key1) => Data.Remove(key1: key1);
        public bool Remove(TKey2 key2) => Data.Remove(key2: key2);
        public bool Remove(TKey3 key3) => Data.Remove(key3: key3);
        public bool Remove(TKey4 key4) => Data.Remove(key4: key4);
        public bool Remove(TKey5 key5) => Data.Remove(key5: key5);

        public void Clear() => Data.Clear();

    }
}
