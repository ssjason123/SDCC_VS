﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EmuliciousPassThroughAdapter.Modifiers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmuliciousPassThroughAdapter
{
    /// <summary>
    ///     Stream direction.
    /// </summary>
    public enum StreamDirection
    {
        /// <summary>
        ///     To the client application.
        /// </summary>
        ToClient,

        /// <summary>
        ///     To the VS Host process.
        /// </summary>
        ToHost
    }

    /// <summary>
    ///     Custom stream for remapping VS Debug Adapter file paths.
    /// </summary>
    public class PathRenameStream : Stream
    {
        /// <summary>
        ///     Stream for debug output.
        /// </summary>
        public StreamWriter DebugStream { get; set; }

        /// <summary>
        ///     The stream to write to.
        /// </summary>
        public Stream SourceStream { get; }

        /// <summary>
        ///     The stream write direction.
        /// </summary>
        public StreamDirection Direction { get; }

        /// <summary>
        ///     Storage for input buffer.
        /// </summary>
        private List<byte> inputBuffer = new List<byte>();

        /// <summary>
        ///     Storage for the current header read size.
        /// </summary>
        private int? readSize = null;

        /// <summary>
        ///     The custom modifiers to execute.
        /// </summary>
        private IList<IJsonModifier> Modifiers = new List<IJsonModifier>();

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="sourceStream">
        ///     The source stream to write to.
        /// </param>
        /// <param name="direction">
        ///     The direction events are sent.
        /// </param>
        /// <param name="modifiers">
        ///     The Json modifiers to execute.
        /// </param>
        public PathRenameStream(Stream sourceStream, StreamDirection direction, IList<IJsonModifier> modifiers)
        {
            SourceStream = sourceStream;
            Direction = direction;

            if (modifiers != null)
            {
                Modifiers = modifiers;
            }
        }

        /// <inheritdoc />
        public override void Flush()
        {
            SourceStream.Flush();
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            return SourceStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            SourceStream.SetLength(value);
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            return SourceStream.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            // Append to the buffer, process it and send any updates.
            inputBuffer.AddRange(buffer.Skip(offset).Take(count));

            ProcessBuffer();
        }

        /// <inheritdoc />
        public override bool CanRead
        {
            get { return SourceStream.CanRead; }
        }

        /// <inheritdoc />
        public override bool CanSeek
        {
            get { return SourceStream.CanSeek; }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            get { return SourceStream.CanWrite; }
        }

        /// <inheritdoc />
        public override long Length
        {
            get { return SourceStream.Length; }
        }

        /// <inheritdoc />
        public override long Position
        {
            get { return SourceStream.Position; }
            set { SourceStream.Position = value; }
        }

        /// <summary>
        ///     Process the write buffer.
        /// </summary>
        private void ProcessBuffer()
        {
            if (readSize == null)
            {
                // Attempt to read the header.
                var indexOf = inputBuffer.IndexOf(0x0A);
                if (indexOf != -1)
                {
                    indexOf = inputBuffer.IndexOf(0x0A, indexOf + 1);

                    if (indexOf != -1)
                    {
                        // This is the length of the header string.
                        var headerString = Encoding.UTF8.GetString(inputBuffer.Take(indexOf + 1).ToArray());

                        // Decode the header string to determine the buffer length.
                        var decodeString = "([0-9]+)";

                        var buffMatch = Regex.Match(headerString, decodeString);
                        if (buffMatch.Success)
                        {
                            readSize = int.Parse(buffMatch.Value);
                        }

                        // On a successful read, trim buffer and apply the read size.
                        inputBuffer.RemoveRange(0, indexOf + 1);
                    }
                }
            }

            if (readSize != null && readSize.Value <= inputBuffer.Count)
            {
                // Parse the buffer to a generic JSON object.
                var message = Encoding.UTF8.GetString(inputBuffer.Take(readSize.Value).ToArray());
                JToken jsonParse = JsonConvert.DeserializeObject(message) as JToken;

                foreach (var modifier in Modifiers)
                {
                    modifier.ProcessJson(jsonParse, Direction);
                }

                // Send the message.
                WriteJsonMessage(jsonParse.ToString(Formatting.None));

                // Trim the buffer.
                inputBuffer.RemoveRange(0, readSize.Value);
                readSize = null;

                // If there are additional buffer data try to process the next message.
                ProcessBuffer();
            }
        }

        /// <summary>
        ///     Write the JSON message to the output stream.
        /// </summary>
        /// <param name="message">
        ///     The message to write.
        /// </param>
        private void WriteJsonMessage(string message)
        {
            // Build the header message.
            var header = string.Format("Content-Length: {0}{1}{2}{1}{2}", message.Length, (char)0x0D, (char)0x0A);
            var headerData = Encoding.UTF8.GetBytes(header);
            SourceStream.Write(headerData, 0, headerData.Length);

            // Write the body data.
            var bodyData = Encoding.UTF8.GetBytes(message);
            SourceStream.Write(bodyData, 0, bodyData.Length);

            SourceStream.Flush();

            // debug the new message.
            if (DebugStream != null)
            {
                lock (DebugStream)
                {
                    DebugStream.WriteLine("*****************************************");
                    DebugStream.WriteLine(" Direction: {0}", Direction);
                    DebugStream.WriteLine(" Timestamp: {0}", DateTime.Now);
                    DebugStream.WriteLine("*****************************************");
                    DebugStream.WriteLine(message);
                    DebugStream.WriteLine("*****************************************");
                    DebugStream.Flush();
                }
            }
        }
    }
}
