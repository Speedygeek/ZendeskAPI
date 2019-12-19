using NUnit.Framework;
using Speedygeek.ZendeskAPI.Models.Support;
using Speedygeek.ZendeskAPI.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var json = "{\r\n  \"value\": \"test_1\"\r\n}";
            
            var field = serializer.Serialize(new CustomField { Value = { "test_1" } });

            Assert.That(field, Is.EqualTo(json));
        }
    }
}
