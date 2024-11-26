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
    /// Converts Unity color into json and back.
    /// </summary>
    [NewtonsoftConverter]
    internal class UnityColorConverter : JsonConverter //TODO: make a GodotColorConverter.cs and solve references to it instead
    {
        /// <inheritDoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color color = (Color) value;
            JObject data = new JObject();

#if UNITY_5_3_OR_NEWER
            data.Add("r",color.r);
            data.Add("g",color.g);
            data.Add("b",color.b);
            data.Add("a",color.a);
#elif GODOT
            data.Add("r",color.R);
            data.Add("g",color.G);
            data.Add("b",color.B);
            data.Add("a",color.A);
#endif

            data.WriteTo(writer);
        }

        /// <inheritDoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {
                    JObject data = (JObject) JToken.ReadFrom(reader);

                    float r = data["r"].Value<float>();
                    float g = data["g"].Value<float>();
                    float b = data["b"].Value<float>();
                    float a = 1.0f;
                    if (data.Count == 4)
                    {
                        a = data["a"].Value<float>();
                    }

                    return new Color(r, g, b, a);
                }
                catch (Exception ex)
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogErrorFormat("Exception occured while trying to parse a color.\n{0}", ex.Message);
                    return Color.magenta;
#elif GODOT
                    GD.PushError("Exception occured while trying to parse a color.\n{0}", ex.Message);
                    return Colors.Magenta;
#endif
                }
            }
#if UNITY_5_3_OR_NEWER
            Debug.LogWarning("Can't read/parse color from JSON.");
            return Color.magenta;
#elif GODOT
            GD.PushWarning("Can't read/parse color from JSON.");
            return Colors.Magenta;
#endif
        }


        /// <inheritDoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Color) == objectType;
        }
    }
}
