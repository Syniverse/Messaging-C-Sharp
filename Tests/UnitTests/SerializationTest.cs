using Newtonsoft.Json;
using ScgApi;
using System;
using Xunit;

namespace UnitTests
{
    public class Data : ModelBase
    {
        public Data()
        {

        }

        public Data(String id = null, Int64? applicationId = null, String firstName = null)
        {
            Id = id;
            ApplicationId = applicationId;
            FirstName = firstName;
        }


        // Read Only
        // Should not be serialized
        public String Id { get; set; }
        public bool ShouldSerializeId() { return false; }
        public Int64? ApplicationId { get; set; }
        public bool ShouldSerializeApplicationId() { return false; }

        // Read Write
        // Should be serialized and deserialized
        public String FirstName { get; set; }
    }

    public class SerializationTest
    {
        [Fact]
        public void TestSerialize()
        {
            var serializer = new ScgApi.Serialize<Data>();
            Data data = new Data("idXXX", 12345, "John");

            string json = serializer.ToJson(data);
            Assert.DoesNotContain("id", json);
            Assert.DoesNotContain("application_id", json);
            Assert.Contains("first_name", json);
        }


        [Fact]
        public void TestDeserialize()
        {
            var serializer = new ScgApi.Serialize<Data>();
            var json = @"{ ""id"":""idXXX"", ""application_id"":12345, ""first_name"":""John""}";

            var data = serializer.ToModel(json);

            Assert.Equal("idXXX", data.Id);
            Assert.Equal(12345, data.ApplicationId.Value);
            Assert.Equal("John", data.FirstName);
        }
    }
}