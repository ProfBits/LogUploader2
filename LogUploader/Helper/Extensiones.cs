using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensiones
{
    public static class Extensiones
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

        public static LogUploader.Data.Profession Get(this LogUploader.eProfession profession)
        {
            return LogUploader.Data.Profession.Get(profession);
        }

        public static LogUploader.Data.RaidOrgaPlus.Role GetRoleByAbbreviation(this LogUploader.Data.RaidOrgaPlus.Role _, string roleAbbreviation)
        {
            return LogUploader.Helper.GP.GetRoleByAbbreviation(roleAbbreviation);
        }
    }
}

namespace LogUploader.Extensiones
{
    using global::Extensiones;

    public static class LogUploaderExtensiones
    {
    }
}

namespace Extensiones.Linq
{
    // Static class to contain the extension methods.
    public static class LinquExtensiones
    {
        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ChunkBy(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            // Flag to signal end of source sequence.
            const bool noMoreSourceElements = true;

            // Auto-generated iterator for the source array.       
            var enumerator = source.GetEnumerator();

            // Move to the first element in the source sequence.
            if (!enumerator.MoveNext()) yield break;

            // Iterate through source sequence and create a copy of each Chunk.
            // On each pass, the iterator advances to the first element of the next "Chunk"
            // in the source sequence. This loop corresponds to the outer foreach loop that
            // executes the query.
            Chunk<TKey, TSource> current = null;
            while (true)
            {
                // Get the key for the current Chunk. The source iterator will churn through
                // the source sequence until it finds an element with a key that doesn't match.
                var key = keySelector(enumerator.Current);

                // Make a new Chunk (group) object that initially has one GroupItem, which is a copy of the current source element.
                current = new Chunk<TKey, TSource>(key, enumerator, value => comparer.Equals(key, keySelector(value)));

                // Return the Chunk. A Chunk is an IGrouping<TKey,TSource>, which is the return value of the ChunkBy method.
                // At this point the Chunk only has the first element in its source sequence. The remaining elements will be
                // returned only when the client code foreach's over this chunk. See Chunk.GetEnumerator for more info.
                yield return current;

                // Check to see whether (a) the chunk has made a copy of all its source elements or 
                // (b) the iterator has reached the end of the source sequence. If the caller uses an inner
                // foreach loop to iterate the chunk items, and that loop ran to completion,
                // then the Chunk.GetEnumerator method will already have made
                // copies of all chunk items before we get here. If the Chunk.GetEnumerator loop did not
                // enumerate all elements in the chunk, we need to do it here to avoid corrupting the iterator
                // for clients that may be calling us on a separate thread.
                if (current.CopyAllChunkElements() == noMoreSourceElements)
                {
                    yield break;
                }
            }
        }

        // A Chunk is a contiguous group of one or more source elements that have the same key. A Chunk 
        // has a key and a list of ChunkItem objects, which are copies of the elements in the source sequence.
        class Chunk<TKey, TSource> : IGrouping<TKey, TSource>
        {
            // INVARIANT: DoneCopyingChunk == true || 
            //   (predicate != null && predicate(enumerator.Current) && current.Value == enumerator.Current)

            // A Chunk has a linked list of ChunkItems, which represent the elements in the current chunk. Each ChunkItem
            // has a reference to the next ChunkItem in the list.
            class ChunkItem
            {
                public ChunkItem(TSource value)
                {
                    Value = value;
                }
                public readonly TSource Value;
                public ChunkItem Next = null;
            }

            // The value that is used to determine matching elements
            private readonly TKey key;

            // Stores a reference to the enumerator for the source sequence
            private IEnumerator<TSource> enumerator;

            // A reference to the predicate that is used to compare keys.
            private Func<TSource, bool> predicate;

            // Stores the contents of the first source element that
            // belongs with this chunk.
            private readonly ChunkItem head;

            // End of the list. It is repositioned each time a new
            // ChunkItem is added.
            private ChunkItem tail;

            // Flag to indicate the source iterator has reached the end of the source sequence.
            internal bool isLastSourceElement = false;

            // Private object for thread syncronization
            private readonly object m_Lock;

            // REQUIRES: enumerator != null && predicate != null
            public Chunk(TKey key, IEnumerator<TSource> enumerator, Func<TSource, bool> predicate)
            {
                this.key = key;
                this.enumerator = enumerator;
                this.predicate = predicate;

                // A Chunk always contains at least one element.
                head = new ChunkItem(enumerator.Current);

                // The end and beginning are the same until the list contains > 1 elements.
                tail = head;

                m_Lock = new object();
            }

            // Indicates that all chunk elements have been copied to the list of ChunkItems, 
            // and the source enumerator is either at the end, or else on an element with a new key.
            // the tail of the linked list is set to null in the CopyNextChunkElement method if the
            // key of the next element does not match the current chunk's key, or there are no more elements in the source.
            private bool DoneCopyingChunk => tail == null;

            // Adds one ChunkItem to the current group
            // REQUIRES: !DoneCopyingChunk && lock(this)
            private void CopyNextChunkElement()
            {
                // Try to advance the iterator on the source sequence.
                // If MoveNext returns false we are at the end, and isLastSourceElement is set to true
                isLastSourceElement = !enumerator.MoveNext();

                // If we are (a) at the end of the source, or (b) at the end of the current chunk
                // then null out the enumerator and predicate for reuse with the next chunk.
                if (isLastSourceElement || !predicate(enumerator.Current))
                {
                    enumerator = null;
                    predicate = null;
                }
                else
                {
                    tail.Next = new ChunkItem(enumerator.Current);
                }

                // tail will be null if we are at the end of the chunk elements
                // This check is made in DoneCopyingChunk.
                tail = tail.Next;
            }

            // Called after the end of the last chunk was reached. It first checks whether
            // there are more elements in the source sequence. If there are, it 
            // Returns true if enumerator for this chunk was exhausted.
            internal bool CopyAllChunkElements()
            {
                while (true)
                {
                    lock (m_Lock)
                    {
                        if (DoneCopyingChunk)
                        {
                            // If isLastSourceElement is false,
                            // it signals to the outer iterator
                            // to continue iterating.
                            return isLastSourceElement;
                        }
                        else
                        {
                            CopyNextChunkElement();
                        }
                    }
                }
            }

            public TKey Key => key;

            // Invoked by the inner foreach loop. This method stays just one step ahead
            // of the client requests. It adds the next element of the chunk only after
            // the clients requests the last element in the list so far.
            public IEnumerator<TSource> GetEnumerator()
            {
                //Specify the initial element to enumerate.
                ChunkItem current = head;

                // There should always be at least one ChunkItem in a Chunk.
                while (current != null)
                {
                    // Yield the current item in the list.
                    yield return current.Value;

                    // Copy the next item from the source sequence, 
                    // if we are at the end of our local list.
                    lock (m_Lock)
                    {
                        if (current == tail)
                        {
                            CopyNextChunkElement();
                        }
                    }

                    // Move to the next ChunkItem in the list.
                    current = current.Next;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public static T MaxBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new { value = t, eval = evaluate(t) })
                .Aggregate((max, next) => next.eval.CompareTo(max.eval) > 0 ? next : max).value;
        }

        public static T MinBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new { value = t, eval = evaluate(t) })
                .Aggregate((max, next) => next.eval.CompareTo(max.eval) < 0 ? next : max).value;
        }
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> array)
        {
            return array.Select((item, index) => (index, item));
        }
    }
}

namespace Extensiones.HTTPClient
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.IO;

    using Stream;

    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    // Ignore progress reporting when no progress reporter was 
                    // passed or when the content length is unknown
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((double)totalBytes / contentLength.Value));
                    // Use extension method to report progress while downloading
                    await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
                    progress.Report(1);
                }
            }
        }
    }
}

namespace Extensiones.Stream
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.IO;

    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
                Console.WriteLine("bytes read: " + totalBytesRead);
            }
        }
    }
}
