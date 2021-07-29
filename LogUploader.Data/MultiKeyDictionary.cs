using System;
using System.Collections;
using System.Collections.Generic;

namespace LogUploader.Data
{
    public interface IMultiKeyBaseDictionary<TKey1, TKey2, TValue>
    {
        int Count { get; }

        void Clear();
        TValue GetByIndex(int index);
        TValue Get(TKey1 key1);
        TValue Get(TKey2 key2);
        bool RemoveAt(int index);
        bool Remove(TKey1 key1);
        bool Remove(TKey2 key2);
        bool ContainsKey(TKey1 key1);
        bool ContainsKey(TKey2 key2);
        int IndexOf(TKey1 key1);
        int IndexOf(TKey2 key2);
        TKey1 GetKey1(int index);
        TKey2 GetKey2(int index);
    }

    public interface IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TValue>
    {
        TValue Get(TKey3 key3);
        bool Remove(TKey3 key3);
        bool ContainsKey(TKey3 key3);
        int IndexOf(TKey3 key3);
        TKey3 GetKey3(int index);
    }

    public interface IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TValue>
    {
        TValue Get(TKey4 key4);
        bool Remove(TKey4 key4);
        bool ContainsKey(TKey4 key4);
        int IndexOf(TKey4 key4);
        TKey4 GetKey4(int index);
    }

    public interface IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TValue>
    {
        TValue Get(TKey5 key5);
        bool Remove(TKey5 key5);
        bool ContainsKey(TKey5 key5);
        int IndexOf(TKey5 key);
        TKey5 GetKey5(int index);
    }

    public interface IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        TValue Get(TKey6 key6);
        bool Remove(TKey6 key6);
        bool ContainsKey(TKey6 key6);
        int IndexOf(TKey6 key);
        TKey6 GetKey6(int index);
    }

    public interface IMultiKeyDictionary<TKey1, TKey2, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TValue>, IEnumerable<KeyValuePair<(TKey1, TKey2), TValue>>
    {
        void Add((TKey1, TKey2) key, TValue value);
    }

    public interface IMultiKeyDictionary<TKey1, TKey2, TKey3, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TValue>, IEnumerable<KeyValuePair<(TKey1, TKey2, TKey3), TValue>>
    {
        void Add((TKey1, TKey2, TKey3) key, TValue value);
    }

    public interface IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TValue>, IEnumerable<KeyValuePair<(TKey1, TKey2, TKey3, TKey4), TValue>>
    {
        void Add((TKey1, TKey2, TKey3, TKey4) key, TValue value);
    }

    public interface IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>, IEnumerable<KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5), TValue>>
    {
        void Add((TKey1, TKey2, TKey3, TKey4, TKey5) key, TValue value);
    }

    public interface IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue> : IMultiKeyBaseDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue>, IEnumerable<KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5, TKey6), TValue>>
    {
        void Add((TKey1, TKey2, TKey3, TKey4, TKey5, TKey6) key, TValue value);
    }

    public class MultiKeyDictionary<TKey1, TKey2, TValue> : IMultiKeyDictionary<TKey1, TKey2, TValue>
    {
        private readonly List<TKey1> Key1 = new List<TKey1>();
        private readonly List<TKey2> Key2 = new List<TKey2>();

        public int Count { get => Values.Count; }
        internal List<TValue> Values { get; } = new List<TValue>();

        public void Add((TKey1, TKey2) key, TValue value)
        {
            (TKey1 key1, TKey2 key2) = key;

            if (ContainsKey(key1)) throw new ArgumentException("Cannot add, key already exists", nameof(key1));
            if (ContainsKey(key2)) throw new ArgumentException("Cannot add, key already exists", nameof(key2));

            Key1.Add(key1);
            Key2.Add(key2);

            Values.Add(value);
        }

        public void Clear()
        {
            Key1.Clear();
            Key2.Clear();
            Values.Clear();
        }

        public bool ContainsKey(TKey1 key1)
        {
            var index = Key1.IndexOf(key1);
            return index > -1;
        }

        public bool ContainsKey(TKey2 key2)
        {
            var index = Key2.IndexOf(key2);
            return index > -1;
        }

        public TValue Get(TKey1 key1)
        {
            var index = IndexOf(key1);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue Get(TKey2 key2)
        {
            var index = IndexOf(key2);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue GetByIndex(int index)
        {
            return Values[index];
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2), TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<(TKey1, TKey2), TValue>((GetKey1(i), GetKey2(i)), GetByIndex(i));
            }
        }

        public TKey1 GetKey1(int index)
        {
            return Key1[index];
        }

        public TKey2 GetKey2(int index)
        {
            return Key2[index];
        }

        public int IndexOf(TKey1 key1)
        {
            return Key1.IndexOf(key1);
        }

        public int IndexOf(TKey2 key2)
        {
            return Key2.IndexOf(key2);
        }

        public bool Remove(TKey1 key1)
        {
            return RemoveAt(Key1.IndexOf(key1));
        }

        public bool Remove(TKey2 key2)
        {
            return RemoveAt(Key2.IndexOf(key2));
        }


        public bool RemoveAt(int index)
        {
            if (0 <= index && index < Values.Count)
            {
                Key1.RemoveAt(index);
                Key2.RemoveAt(index);
                Values.RemoveAt(index);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class MultiKeyDictionary<TKey1, TKey2, TKey3, TValue> : IMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>
    {
        private readonly IMultiKeyDictionary<TKey1, TKey2, TValue> backEnd = new MultiKeyDictionary<TKey1, TKey2, TValue>();
        private readonly List<TKey3> Key3 = new List<TKey3>();

        public int Count { get => backEnd.Count; }

        public void Add((TKey1, TKey2, TKey3) key, TValue value)
        {
            (TKey1 key1, TKey2 key2, TKey3 key3) = key;
            if (ContainsKey(key3)) throw new ArgumentException("Cannot add, key already exists", nameof(key3));
            backEnd.Add((key1, key2), value);
            Key3.Add(key3);
        }

        public void Clear()
        {
            Key3.Clear();
            backEnd.Clear();
        }

        public bool ContainsKey(TKey3 key3)
        {
            return Key3.Contains(key3);
        }

        public bool ContainsKey(TKey1 key1) => backEnd.ContainsKey(key1);

        public bool ContainsKey(TKey2 key2) => backEnd.ContainsKey(key2);

        public TValue Get(TKey3 key3)
        {
            var index = IndexOf(key3);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue Get(TKey1 key1) => backEnd.Get(key1);

        public TValue Get(TKey2 key2) => backEnd.Get(key2);

        public TValue GetByIndex(int index)
        {
            return backEnd.GetByIndex(index);
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2, TKey3), TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<(TKey1, TKey2, TKey3), TValue>((GetKey1(i), GetKey2(i), GetKey3(i)), GetByIndex(i));
            }
        }

        public TKey1 GetKey1(int index) => backEnd.GetKey1(index);

        public TKey2 GetKey2(int index) => backEnd.GetKey2(index);

        public TKey3 GetKey3(int index)
        {
            return Key3[index];
        }

        public int IndexOf(TKey3 key3)
        {
            return Key3.IndexOf(key3);
        }

        public int IndexOf(TKey1 key1) => backEnd.IndexOf(key1);

        public int IndexOf(TKey2 key2) => backEnd.IndexOf(key2);

        public bool Remove(TKey3 key3)
        {
            return RemoveAt(IndexOf(key3));
        }

        public bool Remove(TKey1 key1)
        {
            return RemoveAt(backEnd.IndexOf(key1));
        }

        public bool Remove(TKey2 key2)
        {
            return RemoveAt(backEnd.IndexOf(key2));
        }

        public bool RemoveAt(int index)
        {
            if (0 <= index && index < Key3.Count)
            {
                Key3.RemoveAt(index);
                return backEnd.RemoveAt(index);
            }
            return false;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue> : IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue>
    {
        private readonly IMultiKeyDictionary<TKey1, TKey2, TKey3, TValue> backEnd = new MultiKeyDictionary<TKey1, TKey2, TKey3, TValue>();
        private readonly List<TKey4> Key4 = new List<TKey4>();

        public int Count { get => backEnd.Count; }

        public void Add((TKey1, TKey2, TKey3, TKey4) key, TValue value)
        {
            (TKey1 key1, TKey2 key2, TKey3 key3, TKey4 key4) = key;
            if (ContainsKey(key4)) throw new ArgumentException("Cannot add, key already exists", nameof(key4));
            backEnd.Add((key1, key2, key3), value);
            Key4.Add(key4);
        }

        public void Clear()
        {
            Key4.Clear();
            backEnd.Clear();
        }

        public bool ContainsKey(TKey3 key3) => backEnd.ContainsKey(key3);

        public bool ContainsKey(TKey1 key1) => backEnd.ContainsKey(key1);

        public bool ContainsKey(TKey2 key2) => backEnd.ContainsKey(key2);

        public bool ContainsKey(TKey4 key4)
        {
            return Key4.Contains(key4);
        }

        public TValue Get(TKey4 key4)
        {
            var index = IndexOf(key4);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue Get(TKey1 key1) => backEnd.Get(key1);

        public TValue Get(TKey2 key2) => backEnd.Get(key2);
        public TValue Get(TKey3 key3) => backEnd.Get(key3);

        public TValue GetByIndex(int index)
        {
            return backEnd.GetByIndex(index);
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2, TKey3, TKey4), TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<(TKey1, TKey2, TKey3, TKey4), TValue>((GetKey1(i), GetKey2(i), GetKey3(i), GetKey4(i)), GetByIndex(i));
            }
        }

        public TKey1 GetKey1(int index) => backEnd.GetKey1(index);

        public TKey2 GetKey2(int index) => backEnd.GetKey2(index);

        public TKey3 GetKey3(int index) => backEnd.GetKey3(index);

        public TKey4 GetKey4(int index)
        {
            return Key4[index];
        }

        public int IndexOf(TKey4 key4)
        {
            return Key4.IndexOf(key4);
        }

        public int IndexOf(TKey1 key1) => backEnd.IndexOf(key1);

        public int IndexOf(TKey2 key2) => backEnd.IndexOf(key2);
        public int IndexOf(TKey3 key3) => backEnd.IndexOf(key3);


        public bool Remove(TKey4 key4)
        {
            return RemoveAt(IndexOf(key4));
        }

        public bool Remove(TKey1 key1)
        {
            return RemoveAt(backEnd.IndexOf(key1));
        }

        public bool Remove(TKey2 key2)
        {
            return RemoveAt(backEnd.IndexOf(key2));
        }

        public bool Remove(TKey3 key3)
        {
            return RemoveAt(backEnd.IndexOf(key3));
        }


        public bool RemoveAt(int index)
        {
            if (0 <= index && index < Key4.Count)
            {
                Key4.RemoveAt(index);
                return backEnd.RemoveAt(index);
            }
            return false;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        private readonly IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue> backEnd = new MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue>();
        private readonly List<TKey5> Key5 = new List<TKey5>();

        public int Count { get => backEnd.Count; }

        public void Add((TKey1, TKey2, TKey3, TKey4, TKey5) key, TValue value)
        {
            (TKey1 key1, TKey2 key2, TKey3 key3, TKey4 key4, TKey5 key5) = key;
            if (ContainsKey(key5)) throw new ArgumentException("Cannot add, key already exists", nameof(key5));
            backEnd.Add((key1, key2, key3, key4), value);
            Key5.Add(key5);
        }

        public void Clear()
        {
            Key5.Clear();
            backEnd.Clear();
        }


        public bool ContainsKey(TKey1 key1) => backEnd.ContainsKey(key1);
        public bool ContainsKey(TKey2 key2) => backEnd.ContainsKey(key2);
        public bool ContainsKey(TKey3 key3) => backEnd.ContainsKey(key3);
        public bool ContainsKey(TKey4 key4) => backEnd.ContainsKey(key4);

        public bool ContainsKey(TKey5 key5)
        {
            return Key5.Contains(key5);
        }

        public TValue Get(TKey5 key5)
        {
            var index = IndexOf(key5);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue Get(TKey1 key1) => backEnd.Get(key1);

        public TValue Get(TKey2 key2) => backEnd.Get(key2);
        public TValue Get(TKey3 key3) => backEnd.Get(key3);
        public TValue Get(TKey4 key4) => backEnd.Get(key4);

        public TValue GetByIndex(int index)
        {
            return backEnd.GetByIndex(index);
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5), TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5), TValue>((GetKey1(i), GetKey2(i), GetKey3(i), GetKey4(i), GetKey5(i)), GetByIndex(i));
            }
        }

        public TKey1 GetKey1(int index) => backEnd.GetKey1(index);

        public TKey2 GetKey2(int index) => backEnd.GetKey2(index);

        public TKey3 GetKey3(int index) => backEnd.GetKey3(index);

        public TKey4 GetKey4(int index) => backEnd.GetKey4(index);

        public TKey5 GetKey5(int index)
        {
            return Key5[index];
        }

        public int IndexOf(TKey5 key5)
        {
            return Key5.IndexOf(key5);
        }

        public int IndexOf(TKey1 key1) => backEnd.IndexOf(key1);

        public int IndexOf(TKey2 key2) => backEnd.IndexOf(key2);
        public int IndexOf(TKey3 key3) => backEnd.IndexOf(key3);
        public int IndexOf(TKey4 key4) => backEnd.IndexOf(key4);


        public bool Remove(TKey5 key5)
        {
            return RemoveAt(IndexOf(key5));
        }

        public bool Remove(TKey1 key1)
        {
            return RemoveAt(backEnd.IndexOf(key1));
        }

        public bool Remove(TKey2 key2)
        {
            return RemoveAt(backEnd.IndexOf(key2));
        }

        public bool Remove(TKey3 key3)
        {
            return RemoveAt(backEnd.IndexOf(key3));
        }

        public bool Remove(TKey4 key4)
        {
            return RemoveAt(backEnd.IndexOf(key4));
        }


        public bool RemoveAt(int index)
        {
            if (0 <= index && index < Key5.Count)
            {
                Key5.RemoveAt(index);
                return backEnd.RemoveAt(index);
            }
            return false;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue> : IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue>
    {
        private readonly IMultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> backEnd = new MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>();
        private readonly List<TKey6> Key6 = new List<TKey6>();

        public int Count { get => backEnd.Count; }

        public void Add((TKey1, TKey2, TKey3, TKey4, TKey5, TKey6) key, TValue value)
        {
            (TKey1 key1, TKey2 key2, TKey3 key3, TKey4 key4, TKey5 key5, TKey6 key6) = key;
            if (ContainsKey(key6)) throw new ArgumentException("Cannot add, key already exists", nameof(key6));
            backEnd.Add((key1, key2, key3, key4, key5), value);
            Key6.Add(key6);
        }

        public void Clear()
        {
            Key6.Clear();
            backEnd.Clear();
        }


        public bool ContainsKey(TKey1 key1) => backEnd.ContainsKey(key1);
        public bool ContainsKey(TKey2 key2) => backEnd.ContainsKey(key2);
        public bool ContainsKey(TKey3 key3) => backEnd.ContainsKey(key3);
        public bool ContainsKey(TKey4 key4) => backEnd.ContainsKey(key4);
        public bool ContainsKey(TKey5 key5) => backEnd.ContainsKey(key5);

        public bool ContainsKey(TKey6 key6)
        {
            return Key6.Contains(key6);
        }

        public TValue Get(TKey6 key6)
        {
            var index = IndexOf(key6);
            if (index == -1) throw new KeyNotFoundException();
            return GetByIndex(index);
        }

        public TValue Get(TKey1 key1) => backEnd.Get(key1);

        public TValue Get(TKey2 key2) => backEnd.Get(key2);
        public TValue Get(TKey3 key3) => backEnd.Get(key3);
        public TValue Get(TKey4 key4) => backEnd.Get(key4);
        public TValue Get(TKey5 key5) => backEnd.Get(key5);

        public TValue GetByIndex(int index)
        {
            return backEnd.GetByIndex(index);
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5, TKey6), TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<(TKey1, TKey2, TKey3, TKey4, TKey5, TKey6), TValue>((GetKey1(i), GetKey2(i), GetKey3(i), GetKey4(i), GetKey5(i), GetKey6(i)), GetByIndex(i));
            }
        }

        public TKey1 GetKey1(int index) => backEnd.GetKey1(index);

        public TKey2 GetKey2(int index) => backEnd.GetKey2(index);

        public TKey3 GetKey3(int index) => backEnd.GetKey3(index);

        public TKey4 GetKey4(int index) => backEnd.GetKey4(index);

        public TKey5 GetKey5(int index) => backEnd.GetKey5(index);

        public TKey6 GetKey6(int index)
        {
            return Key6[index];
        }

        public int IndexOf(TKey6 key6)
        {
            return Key6.IndexOf(key6);
        }

        public int IndexOf(TKey1 key1) => backEnd.IndexOf(key1);

        public int IndexOf(TKey2 key2) => backEnd.IndexOf(key2);
        public int IndexOf(TKey3 key3) => backEnd.IndexOf(key3);
        public int IndexOf(TKey4 key4) => backEnd.IndexOf(key4);
        public int IndexOf(TKey5 key5) => backEnd.IndexOf(key5);


        public bool Remove(TKey6 key6)
        {
            return RemoveAt(IndexOf(key6));
        }

        public bool Remove(TKey1 key1)
        {
            return RemoveAt(backEnd.IndexOf(key1));
        }

        public bool Remove(TKey2 key2)
        {
            return RemoveAt(backEnd.IndexOf(key2));
        }

        public bool Remove(TKey3 key3)
        {
            return RemoveAt(backEnd.IndexOf(key3));
        }

        public bool Remove(TKey4 key4)
        {
            return RemoveAt(backEnd.IndexOf(key4));
        }

        public bool Remove(TKey5 key5)
        {
            return RemoveAt(backEnd.IndexOf(key5));
        }


        public bool RemoveAt(int index)
        {
            if (0 <= index && index < Key6.Count)
            {
                Key6.RemoveAt(index);
                return backEnd.RemoveAt(index);
            }
            return false;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class MultiKeyInValueDictionary<TKey1, TKey2, TValue> : MultiKeyDictionary<TKey1, TKey2, TValue>
    {
        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;

        public MultiKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator) :
            this(key1Generator, key2Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public MultiKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator)
        {
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value));
            base.Add(key, value);
        }
    }
    public class MultiKeyInValueDictionary<TKey1, TKey2, TKey3, TValue> : MultiKeyDictionary<TKey1, TKey2, TKey3, TValue>
    {
        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;
        readonly Func<TValue, TKey3> Key3Generator;

        public MultiKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator) :
            this(key1Generator, key2Generator, key3Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public MultiKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator)
        {
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
            Key3Generator = key3Generator ?? throw new ArgumentNullException(nameof(key3Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value), Key3Generator(value));
            base.Add(key, value);
        }
    }
    public class MultiKeyInValueDictionary<TKey1, TKey2, TKey3, TKey4, TValue> : MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TValue>
    {
        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;
        readonly Func<TValue, TKey3> Key3Generator;
        readonly Func<TValue, TKey4> Key4Generator;

        public MultiKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator) :
            this(key1Generator, key2Generator, key3Generator, key4Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public MultiKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator)
        {
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
            Key3Generator = key3Generator ?? throw new ArgumentNullException(nameof(key3Generator));
            Key4Generator = key4Generator ?? throw new ArgumentNullException(nameof(key4Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value), Key3Generator(value), Key4Generator(value));
            base.Add(key, value);
        }
    }
    public class MultiKeyInValueDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> : MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>
    {
        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;
        readonly Func<TValue, TKey3> Key3Generator;
        readonly Func<TValue, TKey4> Key4Generator;
        readonly Func<TValue, TKey5> Key5Generator;

        public MultiKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator) :
            this(key1Generator, key2Generator, key3Generator, key4Generator, key5Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public MultiKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator)
        {
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
            Key3Generator = key3Generator ?? throw new ArgumentNullException(nameof(key3Generator));
            Key4Generator = key4Generator ?? throw new ArgumentNullException(nameof(key4Generator));
            Key5Generator = key5Generator ?? throw new ArgumentNullException(nameof(key5Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value), Key3Generator(value), Key4Generator(value), Key5Generator(value));
            base.Add(key, value);
        }
    }
    public class MultiKeyInValueDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue> : MultiKeyDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue>
    {
        readonly Func<TValue, TKey1> Key1Generator;
        readonly Func<TValue, TKey2> Key2Generator;
        readonly Func<TValue, TKey3> Key3Generator;
        readonly Func<TValue, TKey4> Key4Generator;
        readonly Func<TValue, TKey5> Key5Generator;
        readonly Func<TValue, TKey6> Key6Generator;

        public MultiKeyInValueDictionary(IEnumerable<TValue> initalValues, Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator, Func<TValue, TKey6> key6Generator) :
            this(key1Generator, key2Generator, key3Generator, key4Generator, key5Generator, key6Generator)
        {
            foreach (TValue value in initalValues)
            {
                Add(value);
            }
        }

        public MultiKeyInValueDictionary(Func<TValue, TKey1> key1Generator, Func<TValue, TKey2> key2Generator, Func<TValue, TKey3> key3Generator, Func<TValue, TKey4> key4Generator, Func<TValue, TKey5> key5Generator, Func<TValue, TKey6> key6Generator)
        {
            Key1Generator = key1Generator ?? throw new ArgumentNullException(nameof(key1Generator));
            Key2Generator = key2Generator ?? throw new ArgumentNullException(nameof(key2Generator));
            Key3Generator = key3Generator ?? throw new ArgumentNullException(nameof(key3Generator));
            Key4Generator = key4Generator ?? throw new ArgumentNullException(nameof(key4Generator));
            Key5Generator = key5Generator ?? throw new ArgumentNullException(nameof(key5Generator));
            Key6Generator = key6Generator ?? throw new ArgumentNullException(nameof(key6Generator));
        }

        public void Add(TValue value)
        {
            var key = (Key1Generator(value), Key2Generator(value), Key3Generator(value), Key4Generator(value), Key5Generator(value), Key6Generator(value));
            base.Add(key, value);
        }
    }
}
