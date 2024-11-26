// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core.Serialization
{
    /// <summary>
    /// Converts Vector2 into json and back.
    /// </summary>
    [NewtonsoftConverter]
    internal class Vector2Converter : JsonConverter
    {
        /// <inheritDoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 vec = (Vector2) value;
            JObject data = new JObject();

#if UNITY_5_3_OR_NEWER
            data.Add("x", vec.x);
            data.Add("y", vec.y);
#elif GODOT
            data.Add("x", vec.X);
            data.Add("y", vec.Y);
#endif

            data.WriteTo(writer);
        }

        /// <inheritDoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject data = (JObject)JToken.ReadFrom(reader);
                return new Vector2(data["x"].Value<float>(), data["y"].Value<float>());
            }

#if UNITY_5_3_OR_NEWER
            return Vector2.zero;
#elif GODOT
            return Vector2.Zero;
#endif
        }

        /// <inheritDoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector2) == objectType;
        }
    }
}
