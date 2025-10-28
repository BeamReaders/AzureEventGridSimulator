using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Converts a string property containing JSON into a raw JSON object/array in
/// the output.
/// </summary>
public class RawJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    public override void WriteJson(JsonWriter writer, object value,
        JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        string jsonString = value as string;

        if (string.IsNullOrWhiteSpace(jsonString))
        {
            writer.WriteNull();
            return;
        }

        try
        {
            // Parse the JSON string and write it as raw JSON
            var token = JToken.Parse(jsonString);
            token.WriteTo(writer);
        }
        catch (JsonReaderException ex)
        {
            throw new JsonSerializationException($"Failed to parse JSON string", ex);
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType,
        object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        // Read the JSON token and convert it back to a string
        var token = JToken.Load(reader);
        return token.ToString(Formatting.None);
    }
}
