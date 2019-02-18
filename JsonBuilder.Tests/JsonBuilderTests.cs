using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonBuilder.Tests
{
    [TestClass]
    public class JsonBuilderTests
    {
        [TestMethod]
        public void NewJsonBuilderObject_ShouldBeEmpty()
        {
            // Arrange
            JsonBuilder obj;

            // Act
            obj = new JsonBuilder();

            // Assert
            Assert.AreEqual(obj.ToString(JsonFormat.Indent), JsonBuilder.EmptyJsonString);
        }

        [TestMethod]
        public void AddProperties()
        {
            // Arrange
            JsonBuilder obj = new JsonBuilder();
            string jsonResult;

            // Act
            obj.AppendProperty("name", "Jon");
            obj.AppendProperty("age", 22);
            jsonResult = obj.ToString(JsonFormat.Minify);

            // Assert
            Assert.AreEqual(jsonResult, "{\"name\":\"Jon\",\"age\":22}");
        }

        [TestMethod]
        [DataRow(JsonFormat.Indent, "{\r\n\t\"name\":\t\"Jon\"\r\n}")]
        [DataRow(JsonFormat.Minify, "{\"name\":\"Jon\"}")]
        public void FormatJson(JsonFormat format, string expectedResult)
        {
            // Arrange
            JsonBuilder obj = new JsonBuilder();
            string jsonResult;

            // Act
            obj.AppendProperty("name", "Jon");
            jsonResult = obj.ToString(format);

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
            jsonResult = JsonBuilder.MargeJsonObjects(JsonFormat.Minify, json1, json2);

            // Assert
            Assert.AreEqual(jsonResult, "[{\"name\":\"Jon\"},{\"name\":\"Doe\"}]");
        }
    }
}
