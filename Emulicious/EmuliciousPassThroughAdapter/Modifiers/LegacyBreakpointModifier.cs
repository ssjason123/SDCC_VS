using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace EmuliciousPassThroughAdapter.Modifiers
{
    /// <summary>
    ///     Applies fixes for the legacy breakpoint modification.
    /// </summary>
    public class LegacyBreakpointModifier : IJsonModifier
    {
        /// <summary>
        ///     Create a variable to manage locking around the breakpointDictionary.
        /// </summary>
        private static object dictLock = new object();

        /// <summary>
        ///     Dictionary of breakpoints for 'line' remapping.
        /// </summary>
        private static Dictionary<int, JToken> breakpointDictionary = new Dictionary<int, JToken>();

        /// <inheritdoc />
        public void ProcessJson(JToken message, StreamDirection direction)
        {
            if (message != null)
            {
                var command = message.SelectToken("$.command")?.Value<string>();
                if (command == "setBreakpoints")
                {
                    var type = message.SelectToken("$.type")?.Value<string>();
                    if (type == "request")
                    {
                        // Request
                        var sequence = message.SelectToken("$.seq").Value<int>();

                        lock (dictLock)
                        {
                            breakpointDictionary[sequence] = message;
                        }
                    }
                    else
                    {
                        // Response
                        var request_seq = message.SelectToken("$.request_seq").Value<int>();

                        JToken requestEvent;
                        if (breakpointDictionary.TryGetValue(request_seq, out requestEvent))
                        {
                            // Overwrite the body.breakpoints with arguments.breakpoints.
                            var requestBreakpoints = requestEvent.SelectToken("$.arguments.breakpoints");
                            var respBreakpoints = message.SelectToken("$.body.breakpoints");
                            if (requestBreakpoints != null && respBreakpoints != null)
                            {
                                var reqParent = requestBreakpoints as JContainer;
                                var respParent = respBreakpoints as JContainer;

                                if (reqParent != null && respParent != null)
                                {
                                    // Meger breakpoint line data.
                                    respParent.Merge(reqParent,
                                        new JsonMergeSettings()
                                        {
                                            MergeArrayHandling = MergeArrayHandling.Merge
                                        });
                                }
                            }
                        }

                        lock (dictLock)
                        {
                            breakpointDictionary.Remove(request_seq);
                        }
                    }
                }
            }
        }
    }
}
