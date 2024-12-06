// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
//TODO
#endif
using System.Linq;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;


namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Used to identify the user within the scene.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class UserSceneObject : MonoBehaviour
    {
        [SerializeField]
        private Transform head, leftHand, rightHand, rigBase;

        /// <summary>
        /// Returns the user's head transform.
        /// </summary>
        public Transform Head
        {
            get
            {
                if (head == null)
                {
                    head = GetComponentInChildren<Camera>().transform;
                    Debug.LogWarning("User head object is not referenced on User Scene Object component. The rig's camera will be used, if available.");
                }

                return head;
            }
        }

        /// <summary>
        /// Returns the user's left hand transform.
        /// </summary>
        public Transform LeftHand => leftHand;

        /// <summary>
        /// Returns the user's right hand transform.
        /// </summary>
        public Transform RightHand => rightHand;

        /// <summary>
        /// Returns the base of the rig.
        /// </summary>
        public Transform Base => rigBase;
    }
    #elif GODOT

    public partial class UserSceneObject : Node
    {
        [SerializeField]
        private Node3D head, leftHand, rightHand;

        /// <summary>
        /// Returns the user's head transform.
        /// </summary>
        public Node3D Head
        {
            get
            {
                if (head == null)
                {
                    head = FindChildren("*", nameof(Camera3D)).OfType<Node3D>().FirstOrDefault();
                    GD.PushWarning("User head object is not referenced on User Scene Object component. The rig's camera will be used, if available.");
                }

                return head;
            }
        }

        /// <summary>
        /// Returns the user's left hand transform.
        /// </summary>
        public Node3D LeftHand => leftHand;

        /// <summary>
        /// Returns the user's right hand transform.
        /// </summary>
        public Node3D RightHand => rightHand;
    }
#endif
}
