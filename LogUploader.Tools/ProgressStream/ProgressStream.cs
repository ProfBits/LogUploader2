using System;
using System.IO;

namespace LogUploader.Tools
{
    /* 
     * Big thanks to Code Green Software and Mel the author of the articel 
     * 
     * Slightly modernize and split into two files
     */
    /// <summary>
    /// Wraps another stream and provides reporting for when bytes are read or written to the stream.<br/>
    /// Adopted from https://web.archive.org/web/20200407110802/http://mel-green.com/2010/01/progressstream/<br/>
    /// © 2020 Code Green Software
    /// </summary>
    public class ProgressStream : Stream
    {
        #region Private Data Members
        private Stream innerStream;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new ProgressStream supplying the stream for it to report on.
        /// </summary>
        /// <param name="streamToReportOn">The underlying stream that will be reported on when bytes are read or written.</param>
        public ProgressStream(Stream streamToReportOn)
        {
            if (streamToReportOn != null)
            {
                this.innerStream = streamToReportOn;
            }
            else
            {
                throw new ArgumentNullException(nameof(streamToReportOn)); //Change to nameof operator instead of hardcoded string
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised when bytes are read from the stream.
        /// </summary>
        public event ProgressStreamReportDelegate BytesRead;

        /// <summary>
        /// Raised when bytes are written to the stream.
        /// </summary>
        public event ProgressStreamReportDelegate BytesWritten;

        /// <summary>
        /// Raised when bytes are either read or written to the stream.
        /// </summary>
        public event ProgressStreamReportDelegate BytesMoved;

        protected virtual void OnBytesRead(int bytesMoved)
        {
            if (BytesRead != null)
            {
                var args = new ProgressStreamReportEventArgs(bytesMoved, innerStream.Length, innerStream.Position, true);
                BytesRead(this, args);
            }
        }

        protected virtual void OnBytesWritten(int bytesMoved)
        {
            if (BytesWritten != null)
            {
                var args = new ProgressStreamReportEventArgs(bytesMoved, innerStream.Length, innerStream.Position, false);
                BytesWritten(this, args);
            }
        }

        protected virtual void OnBytesMoved(int bytesMoved, bool isRead)
        {
            if (BytesMoved != null)
            {
                var args = new ProgressStreamReportEventArgs(bytesMoved, innerStream.Length, innerStream.Position, isRead);
                BytesMoved(this, args);
            }
        }
        #endregion

        #region Stream Members

        public override bool CanRead
        {
            get { return innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return innerStream.CanWrite; }
        }

        public override void Flush()
        {
            innerStream.Flush();
        }

        public override long Length
        {
            get { return innerStream.Length; }
        }

        public override long Position
        {
            get
            {
                return innerStream.Position;
            }
            set
            {
                innerStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = innerStream.Read(buffer, offset, count);

            OnBytesRead(bytesRead);
            OnBytesMoved(bytesRead, true);

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);

            OnBytesWritten(count);
            OnBytesMoved(count, false);
        }

        public override void Close()
        {
            innerStream.Close();
            base.Close();
        }
        #endregion
    }
}
