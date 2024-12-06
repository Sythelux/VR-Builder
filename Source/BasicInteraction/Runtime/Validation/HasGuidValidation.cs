using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using Godot.Collections;
#endif
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.BasicInteraction.Validation
{
    /// <summary>
    /// Validator that checks if the object has one of the required guids either as
    /// its object ID or as a group.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class HasGuidValidation : Validator, IGuidContainer
#elif GODOT
    public partial class HasGuidValidation : Validator, IGuidContainer
#endif
    
    {
#if UNITY_5_3_OR_NEWER
        [SerializeField]
        private List<string> guids = new List<string>();
#elif GODOT
        [Export]
        private Array<string> guids = new Array<string>();
#endif

        /// <inheritdoc/>
        public IEnumerable<Guid> Guids => guids.Select(tag => Guid.Parse(tag));

        public event EventHandler<GuidContainerEventArgs> GuidAdded;
        public event EventHandler<GuidContainerEventArgs> GuidRemoved;

        /// <inheritdoc/>
        public void AddGuid(Guid guid)
        {
            if (HasGuid(guid) == false)
            {
                guids.Add(guid.ToString());
                GuidAdded?.Invoke(this, new GuidContainerEventArgs(guid));
            }
        }

        /// <inheritdoc/>
        public bool HasGuid(Guid guid)
        {
            return Guids.Contains(guid);
        }

        /// <inheritdoc/>
        public bool RemoveGuid(Guid guid)
        {
            bool removed = false;

            if (HasGuid(guid))
            {
                removed = guids.Remove(guid.ToString());
                GuidRemoved?.Invoke(this, new GuidContainerEventArgs(guid));
            }

            return removed;
        }

        /// <inheritdoc/>
#if UNITY_5_3_OR_NEWER
        public override bool Validate(GameObject obj)
#elif GODOT
        public override bool Validate(Node obj)
#endif
        {
            ProcessSceneObject processSceneObject = obj.GetComponent<ProcessSceneObject>();

            if (processSceneObject == null)
            {
                return false;
            }

            return Guids.Any(guid => processSceneObject.Guid == guid || processSceneObject.HasGuid(guid));
        }
    }
}