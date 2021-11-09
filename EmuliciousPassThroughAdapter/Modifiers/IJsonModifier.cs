using Newtonsoft.Json.Linq;

namespace EmuliciousPassThroughAdapter.Modifiers
{
    /// <summary>
    ///     Interface used to modify a JSON object.
    /// </summary>
    public interface IJsonModifier
    {
        /// <summary>
        ///    Apply any JSON object modifications.
        /// </summary>
        /// <param name="message">
        ///     The message to process.
        /// </param>
        /// <param name="direction">
        ///     The stream direction.
        /// </param>
        void ProcessJson(JToken message, StreamDirection direction);
    }
}
