using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class Vector3IntConverter : JsonConverter<Vector3Int>
{
    public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("x");
        writer.WriteValue(value.x);

        writer.WritePropertyName("y");
        writer.WriteValue(value.y);

        writer.WritePropertyName("z");
        writer.WriteValue(value.z);

        writer.WriteEndObject();
    }

    public override Vector3Int ReadJson(JsonReader reader, Type objectType, Vector3Int existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        return new Vector3Int(jo["x"]!.Value<int>(), jo["y"]!.Value<int>(), jo["z"]!.Value<int>());
    }
}
