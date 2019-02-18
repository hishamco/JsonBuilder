using System;

namespace JsonBuilder
{
    public class JsonFormatException : FormatException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonFormatException"/> class.
        /// </summary>
        public JsonFormatException()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonFormatException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JsonFormatException(string message) : base(message)
        {

        }
    }
}
