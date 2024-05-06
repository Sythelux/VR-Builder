// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using VRBuilder.Core.Utils;
#endif
using System;
using System.Collections.Generic;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Properties;
using System.Linq;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Process runtime configuration which is used if no other was implemented.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    public class DefaultRuntimeConfiguration : BaseRuntimeConfiguration
#elif GODOT
    [Tool]
    public partial class DefaultRuntimeConfiguration : BaseRuntimeConfiguration
#endif
    {
        private IProcessAudioPlayer processAudioPlayer;
        private ISceneObjectManager sceneObjectManager;

        /// <summary>
        /// Default mode which white lists everything.
        /// </summary>
        public static readonly IMode DefaultMode = new Mode("Default", new WhitelistTypeRule<IOptional>());

        public DefaultRuntimeConfiguration()
        {
            Modes = new BaseModeHandler(new List<IMode> {DefaultMode});
        }

        /// <inheritdoc />
        public override ProcessSceneObject User => LocalUser;

        /// <inheritdoc />
        public override UserSceneObject LocalUser
        {
            get
            {
                UserSceneObject user = Users.FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("Could not find a UserSceneObject in the scene.");
                }

                return user;
            }
        }

        /// <inheritdoc />
#if UNITY_5_3_OR_NEWER
        public override AudioSource InstructionPlayer
#elif GODOT
        public override AudioStreamPlayer /*AudioSource*/ InstructionPlayer
#endif
        {
            get
            {
                return ProcessAudioPlayer.FallbackAudioSource;
            }
        }

        /// <inheritdoc />
        public override IProcessAudioPlayer ProcessAudioPlayer
        {
            get
            {
                if (processAudioPlayer == null)
                {
                    processAudioPlayer = new DefaultAudioPlayer();
                }

                return processAudioPlayer;
            }
        }

        /// <inheritdoc />
#if UNITY_5_3_OR_NEWER
        public override IEnumerable<UserSceneObject> Users => GameObject.FindObjectsOfType<UserSceneObject>();
#elif GODOT
        public override IEnumerable<UserSceneObject> Users => NodeExtensions.FindObjectsOfType<UserSceneObject>();
#endif

        /// <inheritdoc />
        public override ISceneObjectManager SceneObjectManager
        {
            get
            {
                if (sceneObjectManager == null)
                {
                    sceneObjectManager = new DefaultSceneObjectManager();
                }

                return sceneObjectManager;
            }
        }

// #if GODOT
//         /// <inheritdoc />
//         public override ISceneObjectRegistry SceneObjectRegistry => sceneObjectRegistry ??= new GodotSceneObjectRegistry();
// #endif
    }
}
