
using System;
using System.Collections;
using System.Collections.Generic;

namespace LogUploader.Helper
{
    public class KeyValueList<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {

        private readonly List<KeyValuePair<TKey, TValue>> Data;

        //
        // Summary:
        //     Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get or set.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- index is equal to or greater than System.Collections.Generic.List`1.Count.
        public KeyValuePair<TKey, TValue> this[int index] { get => Data[index]; set => Data[index] = value; }

        //
        // Summary:
        //     Gets the number of elements contained in the System.Collections.Generic.List`1.
        //
        // Returns:
        //     The number of elements contained in the System.Collections.Generic.List`1.
        public int Count { get => Data.Count; }
            
        //
        // Summary:
        //     Gets or sets the total number of elements the internal data structure can hold
        //     without resizing.
        //
        // Returns:
        //     The number of elements that the System.Collections.Generic.List`1 can contain
        //     before resizing is required.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     System.Collections.Generic.List`1.Capacity is set to a value that is less than
        //     System.Collections.Generic.List`1.Count.
        //
        //   T:System.OutOfMemoryException:
        //     There is not enough memory available on the system.
        public int Capacity { get => Data.Capacity; set => Data.Capacity = value; }

        //
        // Summary:
        //     Gets a value indicating whether the System.Collections.Generic.ICollection`1
        //     is read-only.
        //
        // Returns:
        //     true if the System.Collections.Generic.ICollection`1 is read-only; otherwise,
        //     false.
        public bool IsReadOnly { get => false; }

        public KeyValueList()
        {
            Data = new List<KeyValuePair<TKey, TValue>>();
        }

        public KeyValueList(int capacity)
        {
            Data = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        public KeyValueList(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            Data = new List<KeyValuePair<TKey, TValue>>(collection);
        }

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <param name="key">The key of the pair to add to the System.Collections.Generic.ICollection`1.</param>
        /// <param name="value">The value of the pair to add to the System.Collections.Generic.ICollection`1.</param>
        /// <exception cref="System.NotSupportedException">The System.Collections.Generic.ICollection`1 is read-only</exception>
        public void Add(TKey key, TValue value) => Data.Add(new KeyValuePair<TKey, TValue>(key, value));

        /// <summary>
        /// Removes the element at the specified index of the System.Collections.Generic.List`1.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- index is equal to or greater than System.Collections.Generic.List`1.Count.</exception>
        public void RemoveAt(int index) => Data.RemoveAt(index);

        //
        // Summary:
        //     Determines whether the System.Collections.Generic.List`1 contains elements that
        //     match the conditions defined by the specified predicate.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the elements to
        //     search for.
        //
        // Returns:
        //     true if the System.Collections.Generic.List`1 contains one or more elements that
        //     match the conditions defined by the specified predicate; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public bool Exists(Predicate<KeyValuePair<TKey, TValue>> match) => Data.Exists(match);
        //
        // Summary:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // Returns:
        //     The first element that matches the conditions defined by the specified predicate,
        //     if found; otherwise, the default value for type T.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public KeyValuePair<TKey, TValue> Find(Predicate<KeyValuePair<TKey, TValue>> match) => Data.Find(match);
        //
        // Summary:
        //     Retrieves all the elements that match the conditions defined by the specified
        //     predicate.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the elements to
        //     search for.
        //
        // Returns:
        //     A System.Collections.Generic.List`1 containing all the elements that match the
        //     conditions defined by the specified predicate, if found; otherwise, an empty
        //     System.Collections.Generic.List`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public KeyValueList<TKey, TValue> FindAll(Predicate<KeyValuePair<TKey, TValue>> match) => new KeyValueList<TKey, TValue>(Data.FindAll(match));
        //
        // Summary:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the first occurrence within the
        //     entire System.Collections.Generic.List`1.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // Returns:
        //     The zero-based index of the first occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public int FindIndex(Predicate<KeyValuePair<TKey, TValue>> match) => Data.FindIndex(match);
        //
        // Summary:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the last occurrence within the entire System.Collections.Generic.List`1.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // Returns:
        //     The last element that matches the conditions defined by the specified predicate,
        //     if found; otherwise, the default value for type T.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public KeyValuePair<TKey, TValue> FindLast(Predicate<KeyValuePair<TKey, TValue>> match) => Data.FindLast(match);
        //
        // Summary:
        //     Searches for an element that matches the conditions defined by the specified
        //     predicate, and returns the zero-based index of the last occurrence within the
        //     entire System.Collections.Generic.List`1.
        //
        // Parameters:
        //   match:
        //     The System.Predicate`1 delegate that defines the conditions of the element to
        //     search for.
        //
        // Returns:
        //     The zero-based index of the last occurrence of an element that matches the conditions
        //     defined by match, if found; otherwise, -1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     match is null.
        public int FindLastIndex(Predicate<KeyValuePair<TKey, TValue>> match) => Data.FindLastIndex(match);
        //
        // Summary:
        //     Performs the specified action on each element of the System.Collections.Generic.List`1.
        //
        // Parameters:
        //   action:
        //     The System.Action`1 delegate to perform on each element of the System.Collections.Generic.List`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     action is null.
        //
        //   T:System.InvalidOperationException:
        //     An element in the collection has been modified.
        public void ForEach(Action<KeyValuePair<TKey, TValue>> action) => Data.ForEach(action);
        //
        // Summary:
        //     Inserts an element into the System.Collections.Generic.List`1 at the specified
        //     index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert. The value can be null for reference types.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- index is greater than System.Collections.Generic.List`1.Count.
        public void Insert(int index, KeyValuePair<TKey, TValue> item) => Data.Insert(index, item);
        //
        // Summary:
        //     Inserts the elements of a collection into the System.Collections.Generic.List`1
        //     at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which the new elements should be inserted.
        //
        //   collection:
        //     The collection whose elements should be inserted into the System.Collections.Generic.List`1.
        //     The collection itself cannot be null, but it can contain elements that are null,
        //     if type T is a reference type.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     collection is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than 0. -or- index is greater than System.Collections.Generic.List`1.Count.
        public void InsertRange(int index, IEnumerable<KeyValuePair<TKey, TValue>> collection) => Data.InsertRange(index, collection);

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Data.GetEnumerator();

        //
        // Summary:
        //     Returns an enumerator that iterates through a collection.
        //
        // Returns:
        //     An System.Collections.IEnumerator object that can be used to iterate through
        //     the collection.
        IEnumerator IEnumerable.GetEnumerator() => Data.GetEnumerator();

        //
        // Summary:
        //     Adds an item to the System.Collections.Generic.ICollection`1.
        //
        // Parameters:
        //   item:
        //     The object to add to the System.Collections.Generic.ICollection`1.
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        //
        // Summary:
        //     Removes all items from the System.Collections.Generic.ICollection`1.
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        public void Clear() => Data.Clear();
        //
        // Summary:
        //     Determines whether the System.Collections.Generic.ICollection`1 contains a specific
        //     value.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.ICollection`1.
        //
        // Returns:
        //     true if item is found in the System.Collections.Generic.ICollection`1; otherwise,
        //     false.
        public bool Contains(KeyValuePair<TKey, TValue> item) => Data.Contains(item);
        //
        // Summary:
        //     Copies the elements of the System.Collections.Generic.ICollection`1 to an System.Array,
        //     starting at a particular System.Array index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements copied
        //     from System.Collections.Generic.ICollection`1. The System.Array must have zero-based
        //     indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     array is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   T:System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.ICollection`1
        //     is greater than the available space from arrayIndex to the end of the destination
        //     array.
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Data.CopyTo(array, arrayIndex);
        //
        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection`1.
        //
        // Parameters:
        //   item:
        //     The object to remove from the System.Collections.Generic.ICollection`1.
        //
        // Returns:
        //     true if item was successfully removed from the System.Collections.Generic.ICollection`1;
        //     otherwise, false. This method also returns false if item is not found in the
        //     original System.Collections.Generic.ICollection`1.
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        public bool Remove(KeyValuePair<TKey, TValue> item) => Data.Remove(item);
    }
}
