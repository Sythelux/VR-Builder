// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Properties;
using System;
using System.Collections.Generic;

namespace VRBuilder.Core.SceneObjects
{
    [Obsolete("These args belong to a unused event and will be removed in the next major release.")]
#if UNITY_5_3_OR_NEWER
    public class SceneObjectNameChanged : EventArgs
#elif GODOT
    public partial class SceneObjectNameChanged : Resource
#endif

    {
        public readonly string NewName;
        public readonly string PreviousName;

        public SceneObjectNameChanged(string newName, string previousName)
        {
            NewName = newName;
            PreviousName = previousName;
        }
    }

    /// <summary>
    /// Arguments for UniqueIdChanged event.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class UniqueIdChangedEventArgs : EventArgs
#elif GODOT
    public partial class UniqueIdChangedEventArgs : Resource
#endif
    {
        public readonly Guid NewId;
        public readonly Guid PreviousId;

        public UniqueIdChangedEventArgs(Guid previousId, Guid newId)
        {
            NewId = newId;
            PreviousId = previousId;
        }
    }

    /// <summary>
    /// Gives the possibility to easily identify targets for Conditions, Behaviors and so on.
    /// </summary>
    public interface ISceneObject : ILockable, IGuidContainer
    {
        [Obsolete("This event is no longer used and will be removed in the next major release. Use UniqueIdChanged instead.")]
#if UNITY_5_3_OR_NEWER
        event EventHandler<SceneObjectNameChanged> UniqueNameChanged;
#elif GODOT
        delegate void UniqueNameChangedEventHandler(SceneObjectNameChanged changed);
#endif

        /// <summary>
        /// Called when the object's object id has been changed.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        event EventHandler<UniqueIdChangedEventArgs> ObjectIdChanged;
#elif GODOT
        delegate void ObjectIdChangedEventHandler(UniqueIdChangedEventArgs eventArgs);
#endif

        /// <summary>
        /// Unique Guid for each entity, which is required
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Unique name which is not required
        /// </summary>
        [Obsolete("Use Guid instead.")]
        string UniqueName { get; }

        /// <summary>
        /// Target GameObject, used for applying stuff.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        GameObject GameObject { get; }
#elif GODOT
        Node GameObject { get; } //was GameObject
#endif

        /// <summary>
        /// Properties on the scene object.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        ICollection<ISceneObjectProperty> Properties { get; }
#elif GODOT
        IEnumerable<ISceneObjectProperty> Properties { get; }
#endif


        /// <summary>
        /// True if the scene object has a property of the specified type.
        /// </summary>
        bool CheckHasProperty<T>() where T : ISceneObjectProperty;

        /// <summary>
        /// True if the scene object has a property of the specified type.
        /// </summary>
        bool CheckHasProperty(Type type);

        /// <summary>
        /// Validates properties on the scene object.
        /// </summary>
        void ValidateProperties(IEnumerable<Type> properties);

        /// <summary>
        /// Returns a property of the specified type.
        /// </summary>
        T GetProperty<T>() where T : ISceneObjectProperty;

        /// <summary>
        /// Attempts to change the scene object's unique name to the specified name.
        /// </summary>
        [Obsolete("Use ChangeUniqueId instead.")]
        void ChangeUniqueName(string newName);

        /// <summary>
        /// Gives the object a new specified unique ID.
        /// </summary>
        void SetObjectId(Guid guid);
    }
}
