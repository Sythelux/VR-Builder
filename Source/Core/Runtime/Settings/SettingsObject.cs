// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
#elif GODOT
using Godot;
using Godot.Collections;
#endif
using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace VRBuilder.Core.Settings
{
    /// <summary>
    /// ScriptableObject with additional load and save mechanic to make it a singleton.
    /// </summary>
    /// <typeparam name="T">The class itself</typeparam>
#if UNITY_5_3_OR_NEWER
    public class SettingsObject<T> : ScriptableObject where T : ScriptableObject, new()
#elif GODOT
    public partial class SettingsObject<T> : Resource where T : Resource, new()
#endif
    {
        private static T instance;

        public static T Instance
        {
            get
            {

#if UNITY_EDITOR
                if (EditorUtility.IsDirty(instance))
                {
                    instance = null;
                }
#endif
                if (instance == null)
                {
                    instance = Load();
                }

                return instance;
            }
        }

        private static T Load()
#if UNITY_5_3_OR_NEWER
        {
            T settings = Resources.Load<T>(typeof(T).Name);

            if (settings == null)
            {
                // Create an instance
                settings = CreateInstance<T>();
#if UNITY_EDITOR
                if (!Directory.Exists("Assets/MindPort/VR Builder/Resources"))
                {
                    Directory.CreateDirectory("Assets/MindPort/VR Builder/Resources");
                }
                AssetDatabase.CreateAsset(settings, $"Assets/MindPort/VR Builder/Resources/{typeof(T).Name}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
            return settings;
        }
#elif GODOT4
    {
        var settings = new T();
        foreach (Dictionary property in settings.GetPropertyList())
        {
            if (!property["usage"].Equals(Variant.From(4102))) continue;
            StringName propertyName = property["name"].AsStringName();
            var key = $"TinkerFlow/{typeof(T).Name}/{propertyName}";
            if (ProjectSettings.Singleton.HasSetting(key))
                settings.Set(propertyName, ProjectSettings.Singleton.GetSetting(key));
            else
                ProjectSettings.Singleton.SetSetting(key, default);
        }

        return settings;
    }

        public override bool _Set(StringName property, Variant value)
    {
        ProjectSettings.Singleton.SetSetting(property, default);
        return base._Set(property, value);
    }
#endif

        /// <summary>
        /// Saves the VR Builder settings, only works in editor.
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#elif GODOT4
            // ProjectSettings.Singleton.SetSetting($"TinkerFlow/{typeof(T).Name}", this);
            // EditorInterface.Singleton.Set($"TinkerFlow/{typeof(T).Name}", this);
#endif
        }

        ~SettingsObject()
        {
#if UNITY_EDITOR
            if (EditorUtility.IsDirty(this))
            {
                Save();
            }
#endif
        }
    }
}
