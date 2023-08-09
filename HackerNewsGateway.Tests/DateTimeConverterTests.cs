using HackerNewsGateway.Model;
using NUnit.Framework;
using System.Text.Json;

namespace HackerNewsGateway.Tests
{
    [TestFixture]
    public class DateTimeConverterTests
    {
        [Test]
        public void ConvertDateTimeToIntendedFormat()
        {
            //arrange
            DateTime testTime = new DateTime(2019, 10, 12, 13, 43, 1, DateTimeKind.Utc);

            //act
            var sampleItem = System.Text.Json.JsonSerializer.Serialize(testTime, JsonSerializerOptionsProvider.JsonOptions.SerializerOptions);
            var expectedJsonDoc = JsonDocument.Parse("\"2019-10-12T13:43:01+00:00\"");
            var resultJsonDoc = JsonDocument.Parse(sampleItem);

            //assert
            Assert.IsTrue(expectedJsonDoc.RootElement.ValueEquals(resultJsonDoc.RootElement.ToString()));
        }
    }
}