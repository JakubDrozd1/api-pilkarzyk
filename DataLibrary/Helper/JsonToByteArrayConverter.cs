using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLibrary.Helper
{
    internal sealed class JsonToByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            string? textToConvert = root.GetString() ?? throw new Exception("Convert text is null");
            byte[] byteArray = Encoding.UTF8.GetBytes(textToConvert);
            return byteArray;
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var val in value)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
        }
    }

}
