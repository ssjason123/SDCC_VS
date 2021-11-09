using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace EmuliciousPassThroughAdapter.Modifiers
{
    /// <summary>
    ///     Modifier used to re-map source file locations.
    /// </summary>
    public class LegacySourcePathRenameModifier : IJsonModifier
    {
        /// <summary>
        ///     Collection of path mappings for path swapping.
        /// </summary>
        private List<KeyValuePair<string, string>> PathMappings;

        /// <summary>
        ///     The debug stream.
        /// </summary>
        private StreamWriter DebugStream;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="pathMappings">
        ///     The replacement path mapping folders.
        /// </param>
        /// <param name="debugStream">
        ///     The debug stream to write to.
        /// </param>
        public LegacySourcePathRenameModifier(IDictionary<string, string> pathMappings, StreamWriter debugStream)
        {
            PathMappings = pathMappings.OrderBy(pair => pair.Key.Length).ToList();
            DebugStream = debugStream;
        }

        /// <inheritdoc />
        public void ProcessJson(JToken message, StreamDirection direction)
        {
            ProcessJsonFilePaths(message, direction);
        }

        /// <summary>
        ///     Modify the path to the desired mapping format.
        /// </summary>
        /// <param name="sourcePath">
        ///     The path to the source file.
        /// </param>
        /// <returns>
        ///     The updated path if a file is located elsewhere.
        /// </returns>
        private string ApplyPathUpdate(string sourcePath, StreamDirection direction)
        {
            var resultPath = sourcePath;

            foreach (var path in PathMappings)
            {
                if (sourcePath.StartsWith(path.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    // Replace the path.
                    resultPath = path.Value + resultPath.Substring(path.Key.Length);

                    if (DebugStream != null)
                    {
                        DebugStream.WriteLine("*** Replaced Path({2}): {0} -> {1}", path.Key, path.Value, direction);
                        DebugStream.WriteLine("*** Source Path: {0}", sourcePath);
                        DebugStream.WriteLine("*** Target Path: {0}", resultPath);
                    }
                    break;
                }
            }

            return resultPath;
        }

        /// <summary>
        ///     Process any JSON source file paths.
        /// </summary>
        /// <param name="currentNode">
        ///     The current json node.
        /// </param>
        private void ProcessJsonFilePaths(JToken currentNode, StreamDirection direction)
        {
            if (currentNode != null)
            {
                if (currentNode.Type == JTokenType.Property)
                {
                    var prop = currentNode as JProperty;

                    // Attempt to process any source.path nodes to correct to the original source location.
                    if (prop.Name == "path" && prop.Parent.Path.EndsWith("source"))
                    {
                        string sourceValue = prop.Value.ToString();

                        prop.Value = JValue.CreateString(ApplyPathUpdate(sourceValue, direction));
                    }
                }
                foreach (var child in currentNode.Children())
                {
                    if (child.HasValues)
                    {
                        ProcessJsonFilePaths(child, direction);
                    }
                }
            }
        }
    }
}
