using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace JsonBuilder
{
    public sealed class JsonBuilder
    {
        private readonly ExpandoObject _currentObject;
        private static readonly JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();

        /// <summary>
        /// Create New Json Object
        /// </summary>
        /// <returns></returns>
        public JsonBuilder()
        {
            this._currentObject = new ExpandoObject();
        }

        /// <summary>
        /// Add new property to json object
        /// </summary>
        /// <param name="property">property Name</param>
        /// <param name="value">property value</param>
        /// <returns></returns>
        public JsonBuilder AppendProperty(string property, dynamic value)
        {
            if (property == null)
                throw new ArgumentNullException("The JSON properties can't be null!");
            property = property.Replace(" ", "_");

            var expandoDict = _currentObject as IDictionary<string, dynamic>;
            if (expandoDict.ContainsKey(property))
                expandoDict[property] = value;
            else
                expandoDict.Add(property, value);
            return this;
        }

        /// <inheritdoc />
        public sealed override string ToString() => ToString(Formatting.Indented);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="formatting">The JSON format.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(Formatting formatting)
        {
            var jsonString = JavaScriptSerializer.Serialize(_currentObject)
                .Replace("\"Key\":", "")
                .Replace(",\"Value\":", ":")
                .Replace("},{", ",")
                .TrimStart('[')
                .TrimEnd(']');

            return formatting == Formatting.Indented
                ? FormatOutput(JavaScriptSerializer.Serialize(ConvertToObject(jsonString)))
                : JavaScriptSerializer.Serialize(ConvertToObject(jsonString));
        }

        /// <summary>
        /// Bind Json Objects To Array Of Json Objects
        /// </summary>
        /// <param name="formatting"></param>
        /// <param name="jsonStrings"></param>
        /// <returns>string contains array of json objects</returns>
        public static string MargeJsonObjects(Formatting formatting = Formatting.Indented, params string[] jsonStrings)
        {
            var jsonObject = jsonStrings.Where(IsValidJson).Select(js => JavaScriptSerializer.DeserializeObject(js)).ToList();
            return formatting == Formatting.Indented ? FormatOutput(JavaScriptSerializer.Serialize(jsonObject)) : JavaScriptSerializer.Serialize(jsonObject);
        }

        /// <summary>
        /// Check if the json string is a valid json or not.
        /// </summary>
        /// <param name="jsonString">JSON</param>
        /// <returns>True if is valid, otherwise False.</returns>
        public static bool IsValidJson(string jsonString)
        {
            jsonString = jsonString.Trim();
            if (jsonString.StartsWith("{") && jsonString.EndsWith("}") || //For object
                jsonString.StartsWith("[") && jsonString.EndsWith("]")) //For array
                try
                {
                    JavaScriptSerializer.DeserializeObject(jsonString);
                    return true;
                }
                catch
                {
                    //Debug.WriteLine(jex.Message);
                    return false;
                }

            return false;
        }

        private static dynamic ConvertToObject(string jsonString)
        {
            if (!IsValidJson(jsonString))
                throw new Exception("Not Valid Json Object!");

            var jsDes = JavaScriptSerializer.DeserializeObject(jsonString);
            return jsDes;
        }


        /// <summary>
        /// Convert JSON string to .NET object
        /// </summary>
        /// <param name="jsonString">JSON string</param>
        /// <returns>The property value your require. example: ToObject(Person).HisCourses</returns>
        public static Dictionary<string, dynamic> ToObject(string jsonString)
        {
            if (!IsValidJson(jsonString))
                throw new Exception("Not Valid Json Object!");

            var jsDes = JavaScriptSerializer.Deserialize<dynamic>(jsonString);
            return jsDes;
        }

        /// <summary>
        /// Check the JSON object match the Generic class you passed or not.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <returns>True if the JSON object match the class, false otherwise</returns>
        public bool IsMatchThisClass<T>() where T : class
        {
            var properties = typeof(T).GetProperties();
            var expandoDict = this._currentObject as IDictionary<string, dynamic>;
            return expandoDict.Keys.Count == properties.Length && properties.All(propertyInfo => expandoDict.Keys.Contains(propertyInfo.Name));
        }

        private static string FormatOutput(string jsonString)
        {
            var stringBuilder = new StringBuilder();

            var escaping = false;
            var inQuotes = false;
            var indentation = 0;

            foreach (var character in jsonString)
            {
                if (escaping)
                {
                    escaping = false;
                    stringBuilder.Append(character);
                }
                else
                {
                    if (character == '\\')
                    {
                        escaping = true;
                        stringBuilder.Append(character);
                    }
                    else if (character == '\"')
                    {
                        inQuotes = !inQuotes;
                        stringBuilder.Append(character);
                    }
                    else if (!inQuotes)
                    {
                        if (character == ',')
                        {
                            stringBuilder.Append(character);
                            stringBuilder.Append("\r\n");
                            stringBuilder.Append('\t', indentation);
                        }
                        else if (character == '[' || character == '{')
                        {
                            stringBuilder.Append(character);
                            stringBuilder.Append("\r\n");
                            stringBuilder.Append('\t', ++indentation);
                        }
                        else if (character == ']' || character == '}')
                        {
                            stringBuilder.Append("\r\n");
                            stringBuilder.Append('\t', --indentation);
                            stringBuilder.Append(character);
                        }
                        else if (character == ':')
                        {
                            stringBuilder.Append(character);
                            stringBuilder.Append('\t');
                        }
                        else
                        {
                            stringBuilder.Append(character);
                        }
                    }
                    else
                    {
                        stringBuilder.Append(character);
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
