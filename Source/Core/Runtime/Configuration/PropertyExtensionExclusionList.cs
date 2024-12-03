using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using Godot.Collections;
using VRBuilder.Core.Godot.Attributes;
#endif

namespace VRBuilder.Core.Configuration
{
#if UNITY_5_3_OR_NEWER
    public class PropertyExtensionExclusionList : MonoBehaviour
#elif GODOT
    public partial class PropertyExtensionExclusionList : Node
#endif
    {
#if UNITY_5_3_OR_NEWER
        [SerializeField]
#elif GODOT
        [Export]
#endif
        [Tooltip("Full name of the assembly we want to exclude the types from.")]
        private string assemblyFullName = string.Empty;

        [Tooltip("List of excluded extension type names, including namespaces.")]
#if UNITY_5_3_OR_NEWER
        [SerializeField]
        private readonly List<string> disallowedExtensionTypeNames = new();
#elif GODOT
        [Export]
        private Array<string> disallowedExtensionTypeNames = new();
#endif

        /// <summary>
        /// Full name of the assembly we want to exclude the types from.
        /// </summary>
        public string AssemblyFullName => assemblyFullName;

        /// <summary>
        /// List of excluded extension types.
        /// </summary>
        public IEnumerable<Type> DisallowedExtensionTypes
        {
            get
            {
                IEnumerable<string> assemblyQualifiedNames = disallowedExtensionTypeNames.Select(typeName => $"{typeName}, {assemblyFullName}");
                List<Type> excludedTypes = new List<Type>();

                foreach (string typeName in assemblyQualifiedNames)
                {
                    Type excludedType = Type.GetType(typeName);

                    if (excludedType == null)
                    {
#if UNITY_5_3_OR_NEWER
                        Debug.LogWarning($"Property extension exclusion list for assembly '{assemblyFullName}' contains invalid extension type: '{typeName}'.");
#elif GODOT
                        GD.PushWarning($"Property extension exclusion list for assembly '{assemblyFullName}' contains invalid extension type: '{typeName}'.");
#endif
                    }
                    else
                    {
                        excludedTypes.Add(excludedType);
                    }
                }

                return excludedTypes;
            }
        }
    }
}
