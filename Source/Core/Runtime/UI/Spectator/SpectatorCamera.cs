using System;
using VRBuilder.Core.Configuration;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.UI.Spectator
{
    /// <summary>
    /// Spectator camera which sets its viewpoint to the one of the user.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    [RequireComponent(typeof(Camera))]
    public class SpectatorCamera : MonoBehaviour
    {
        private GameObject user;

        protected virtual void Start()
        {
            user = RuntimeConfigurator.Configuration.LocalUser.Head.gameObject;
        }

        protected virtual void Update()
        {
            UpdateCameraPositionAndRotation();
        }

        /// <summary>
        /// Sets the position and rotation of the spectator camera to the one of the user.
        /// </summary>
        protected virtual void UpdateCameraPositionAndRotation()
        {
            if (user == null)
            {
                try
                {
                    user = RuntimeConfigurator.Configuration.LocalUser.Head.gameObject;
                }
                catch (NullReferenceException)
                {
                    return;
                }
            }

            transform.SetPositionAndRotation(user.transform.position, user.transform.rotation);
        }
    }
#elif GODOT
    public partial class SpectatorCamera : Node
    {
        private Node user;
    }
#endif
}