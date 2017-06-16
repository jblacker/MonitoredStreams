using System;
using System.IO;

namespace MonitoredStreams
{
    /// <summary>
    /// Delegate for handling a ReportingStream Reporting Event
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="args">The event data raised with the event</param>
    public delegate void ReportingStreamReportDelegate(object sender, StreamReportEventArgs args);

    /// <summary>
    /// Wraps a stream to provide reporting via event emitting whenever data is written/read from the underlying stream
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    public class MonitoredStream : Stream, IMonitoredStream
    {
        private readonly Stream internalStream;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoredStream"/> class wrapping around another stream.
        /// </summary>
        /// <param name="stream">The stream that should be wrapped for reporting.</param>
        /// <exception cref="System.ArgumentNullException">stream is null</exception>
        /// <exception cref="System.ArgumentException">If the passed stream is of type <see cref="MonitoredStream"/></exception>
        public MonitoredStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream is MonitoredStream)
                throw new ArgumentException("Cannot wrap a ReportingStream in a ReportingStream");

            this.internalStream = stream;
            this.disposed = false;
        }

        /// <summary>
        /// Occurs when bytes are read from the stream.
        /// </summary>
        public event ReportingStreamReportDelegate BytesRead;
        /// <summary>
        /// Occurs when bytes are written to the stream
        /// </summary>
        public event ReportingStreamReportDelegate BytesWritten;
        /// <summary>
        /// Occurs when the stream is modified in any way.  This 
        /// </summary>
        public event ReportingStreamReportDelegate StreamModified;

        #region PassThroughMethods
        public override bool CanRead
        {
            get { return internalStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return internalStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return internalStream.CanWrite; }
        }

        public override void Flush()
        {
            internalStream.Flush();
        }

        public override long Length
        {
            get { return internalStream.Length; }
        }

        public override long Position
        {
            get { return internalStream.Position; }
            set
            {
                internalStream.Position = value;
            }
        }
        #endregion

        protected virtual void OnBytesRead(int bytesRead)
        {
            if (BytesRead != null)
            {
                BytesRead(this, new StreamReportEventArgs(bytesRead, internalStream.Length, internalStream.Position, StreamEventType.READ));
            }
        }

        protected virtual void OnBytesWritten(int bytesWritten)
        {
            if (BytesWritten != null)
            {
                BytesWritten(this, new StreamReportEventArgs(bytesWritten, internalStream.Length, internalStream.Position, StreamEventType.WRITE));
            }
        }

        protected virtual void OnStreamModified(int change, StreamEventType eventType)
        {
            if (StreamModified != null)
            {
                StreamModified(this, new StreamReportEventArgs(change, internalStream.Length, internalStream.Position, eventType));
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = internalStream.Read(buffer, offset, count);

            OnBytesRead(bytesRead);
            OnStreamModified(bytesRead, StreamEventType.READ);
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var updatedPosition = internalStream.Seek(offset, origin);
            OnStreamModified(0, StreamEventType.SEEK);
            return updatedPosition;
        }

        public override void SetLength(long value)
        {
            internalStream.SetLength(value);
            OnStreamModified(0, StreamEventType.LENGTHSET);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            internalStream.Write(buffer, offset, count);
            OnBytesWritten(count);
            OnStreamModified(count, StreamEventType.WRITE);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Clear all subscribers
                    BytesRead = null;
                    BytesWritten = null;
                    StreamModified = null;

                    internalStream.Dispose();
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
