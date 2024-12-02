// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using System.Linq;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
#if UNITY_5_3_OR_NEWER
    [RequireComponent(typeof(ProcessSceneObject))]
    public abstract class ProcessSceneObjectProperty : MonoBehaviour, ISceneObjectProperty
#elif GODOT
    public abstract partial class ProcessSceneObjectProperty : Node, ISceneObjectProperty
#endif
    {
        private ISceneObject sceneObject;

        public ISceneObject SceneObject
        {
            get
        {
            if (sceneObject == null)
            {
#if UNITY_5_3_OR_NEWER
                    sceneObject = GetComponent<ISceneObject>();
#elif GODOT
            sceneObject = FindChildren("*", recursive: true)?.OfType<ISceneObject>().FirstOrDefault();
#endif

}
            return sceneObject;
        }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void Reset()
        {
            this.AddProcessPropertyExtensions();
        }

        public override string ToString()
        {
#if UNITY_5_3_OR_NEWER
            return SceneObject.GameObject.name;
#elif GODOT
            return SceneObject.GameObject.Name;
#endif
        }
    }
}
