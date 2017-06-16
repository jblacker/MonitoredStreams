using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoredStreams
{
    public interface IMonitoredStream
    {
        /// <summary>
        /// Occurs when bytes are read from the stream.
        /// </summary>
        event ReportingStreamReportDelegate BytesRead;
        /// <summary>
        /// Occurs when bytes are written to the stream
        /// </summary>
        event ReportingStreamReportDelegate BytesWritten;
        /// <summary>
        /// Occurs when the stream is modified in any way.  This 
        /// </summary>
        event ReportingStreamReportDelegate StreamModified;
        /// <summary>
        /// Reads from the stream into the provided buffer.
        /// </summary>
        /// <param name="buffer">Byte array to buffer data read from the stream</param>
        /// <param name="offset">Initial index to begin writing into the buffer</param>
        /// <param name="count">Maximum amount of bytes to read from the stream</param>
        /// <returns>Number of bytes read</returns>
        int Read(byte[] buffer, int offset, int count);
        /// <summary>
        /// Seeks to the specified offset in the stream
        /// </summary>
        /// <param name="offset">The offset from the Seek Origin to </param>
        /// <param name="origin">The origin to use for the offset</param>
        /// <returns>The current location in the stream</returns>
        long Seek(long offset, SeekOrigin origin);
        /// <summary>
        /// Sets the current maximum length of the stream
        /// </summary>
        /// <param name="value">The updated length of the stream.</param>
        void SetLength(long value);
        /// <summary>
        /// Writes to the stream from the specified buffer
        /// </summary>
        /// <param name="buffer">The buffer containing the data to write to the stream</param>
        /// <param name="offset">The starting index to begin copying from the buffer to the stream</param>
        /// <param name="count">The amount of bytes to write from the buffer to the stream</param>
        void Write(byte[] buffer, int offset, int count);
        /// <summary>
        /// Determines if the stream is readable
        /// </summary>
        /// <value>
        ///   <c>true</c> if this stream is readable; otherwise, <c>false</c>.
        /// </value>
        bool CanRead { get; }
        /// <summary>
        /// Determines if the stream is seekable
        /// </summary>
        /// <value>
        ///   <c>true</c> if this stream is seekable; otherwise, <c>false</c>.
        /// </value>
        bool CanSeek { get; }
        /// <summary>
        /// Determines if the stream is writable
        /// </summary>
        /// <value>
        ///   <c>true</c> if this stream is writable; otherwise, <c>false</c>.
        /// </value>
        bool CanWrite { get; }
        /// <summary>
        /// Gets the current length of the stream.
        /// </summary>
        /// <value>
        /// The length of the stream.
        /// </value>
        long Length { get; }
        /// <summary>
        /// The location in the stream that is where read/write operations will occur.
        /// </summary>
        /// <value>
        /// The position of the read/write pointer in the stream.
        /// </value>
        long Position { get; }
    }
}
