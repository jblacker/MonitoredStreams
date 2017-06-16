using System;

namespace MonitoredStreams
{
    public class StreamReportEventArgs : EventArgs
    {
        public int ByteCount { get; private set; }
        public long StreamLength { get; private set; }
        public long StreamPosition { get; private set; }
        public StreamEventType EventType { get; private set; }

        public StreamReportEventArgs(int bytesChanged, long currentStreamLength, long currentStreamPosition,
            StreamEventType eventType)
        {
            this.ByteCount = bytesChanged;
            this.StreamLength = currentStreamLength;
            this.StreamPosition = currentStreamPosition;
            this.EventType = eventType;
        }
    }
}
