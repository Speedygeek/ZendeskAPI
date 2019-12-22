using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Serialization;
using System.IO;
using System.Text;
using Speedygeek.ZendeskAPI.Utilities;

namespace Speedygeek.ZendeskAPI.UnitTests.Serialization
{
    [TestFixture]
    public class SingleOrListConverterTest
    {
        [Test]
        public void ConvertList()
        {
            var serializer = new JsonDotNetSerializer();
            var json = @"{""value"": [ ""test_1"",""test_2""] }";

            var field = serializer.Deserialize<CustomField>(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            Assert.That(field.Value.Count, Is.EqualTo(2));
        }

        [Test]
        public void Single()
        {
            var serializer = new JsonDotNetSerializer();
            var json = @"{""value"": ""test_1"" }";

            var field = serializer.Deserialize<CustomField>(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            Assert.That(field.Value.Count, Is.EqualTo(1));
            Assert.That(field.Value[0], Is.EqualTo("test_1"));
        }

        [Test]
        public void WriteSingle()
        {
            var serializer = new JsonDotNetSerializer();
            var json = @"{""value"":""test_1""}";
            
            var field = serializer.Serialize(new CustomField { Value = { "test_1" } });

            Assert.That(field.RemoveWhitespace(), Is.EqualTo(json));
        }
    }
}
