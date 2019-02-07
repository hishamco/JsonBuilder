using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonBuilder.Tests
{
    [TestClass]
    public class JsonBuilderTests
    {
        [TestMethod]
        public void AddProperties()
        {
            // Arrange
            JsonBuilder obj = new JsonBuilder();
            string jsonResult;

            // Act
            obj.AppendProperty("name", "Jon");
            obj.AppendProperty("age", 22);
            jsonResult = obj.Stringify(Formatting.Minify);

            // Assert
            Assert.AreEqual(jsonResult, "{\"name\":\"Jon\",\"age\":22}");
        }

        [TestMethod]
        [DataRow(Formatting.Indented, "{\r\n\t\"name\":\t\"Jon\"\r\n}")]
        [DataRow(Formatting.Minify, "{\"name\":\"Jon\"}")]
        public void FormatJson(Formatting format, string expectedResult)
        {
            // Arrange
            JsonBuilder obj = new JsonBuilder();
            string jsonResult;

            // Act
            obj.AppendProperty("name", "Jon");
            jsonResult = obj.Stringify(format);

            // Assert
            Assert.AreEqual(jsonResult, expectedResult);
        }

        [TestMethod]
        [DataRow("{{", false)]
        [DataRow("{name: \"Jon\"}", true)]
        public void ValidateJsonString(string value, bool expectedResult)
        {
            // Arrange
           bool isValid;

            // Act
            isValid = JsonBuilder.IsValidJson(value);

            // Assert
            Assert.AreEqual(expectedResult, isValid);
        }

        [TestMethod]
        public void MergeMultipleJsonObjects()
        {
            // Arrange
            var json1 = "{\"name\":\"Jon\"}";
            var json2 = "{\"name\":\"Doe\"}";
            string jsonResult;

            // Act
            jsonResult = JsonBuilder.MargeJsonObjects(Formatting.Minify, json1, json2);

            // Assert
            Assert.AreEqual(jsonResult, "[{\"name\":\"Jon\"},{\"name\":\"Doe\"}]");
        }
    }
}
