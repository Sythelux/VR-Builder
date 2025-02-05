using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
#if UNITY_6000_0_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;
using VRBuilder.Core.Utils.ParticleMachines;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// This behavior causes confetti to rain.
    /// </summary>
    [DataContract(IsReference = true)]
    public partial class ConfettiBehavior : Behavior<ConfettiBehavior.EntityData>
    {
        [DisplayName("Spawn Confetti")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData, IBehaviorExecutionStages
        {
            /// <summary>
            /// Bool to check whether the confetti machine should spawn above the user or at the position of the position provider.
            /// </summary>
            [DataMember]
            [DisplayName("Spawn Above User")]
            public bool IsAboveUser { get; set; }

            /// <summary>
            /// Name of the process object where to spawn the confetti machine.
            /// Only needed if "Spawn Above User" is not checked.
            /// </summary>
            [DataMember]
            [DisplayName("Position Provider")]
            public SingleSceneObjectReference ConfettiPosition { get; set; }

            /// <summary>
            /// Path to the desired confetti machine prefab.
            /// </summary>
            [DataMember]
            [DisplayName("Confetti Machine Path")]
            public string ConfettiMachinePrefabPath { get; set; }

            /// <summary>
            /// Radius of the spawning area.
            /// </summary>
            [DataMember]
            [DisplayName("Area Radius")]
            public float AreaRadius { get; set; }

            /// <summary>
            /// Duration of the animation in seconds.
            /// </summary>
            [DataMember]
            [DisplayName("Duration")]
            public float Duration { get; set; }

            /// <inheritdoc />
            [DataMember]
            public BehaviorExecutionStages ExecutionStages { get; set; }

#if UNITY_6000_0_OR_NEWER
            public GameObject ConfettiMachine { get; set; }
#elif GODOT
            public Node ConfettiMachine { get; set; }
#endif

            public Metadata Metadata { get; set; }

            [IgnoreDataMember]
            public string Name
            {
                get
                {
                    string positionProvider = "user";
                    if (IsAboveUser == false)
                    {
#if UNITY_6000_0_OR_NEWER
                        positionProvider = ConfettiPosition.HasValue() ? ConfettiPosition.Value.GameObject.name : "[NULL]";
#elif GODOT
                        positionProvider = ConfettiPosition.HasValue() ? ConfettiPosition.Value.GameObject.Name : "[NULL]";
#endif

                    }

                    return $"Spawn confetti on {positionProvider}";
                }
            }
        }

        private const float defaultDuration = 15f;
        private const float defaultRadius = 1f;
        private const float distanceAboveUser = 3f;

        [JsonConstructor, Preserve]
        public ConfettiBehavior() : this(true, Guid.Empty, "", defaultRadius, defaultDuration, BehaviorExecutionStages.Activation)
        {
        }

        public ConfettiBehavior(bool isAboveUser, ISceneObject positionProvider, string confettiMachinePrefabPath, float radius, float duration, BehaviorExecutionStages executionStages)
            : this(isAboveUser, ProcessReferenceUtils.GetUniqueIdFrom(positionProvider), confettiMachinePrefabPath, radius, duration, executionStages)
        {
        }

        public ConfettiBehavior(bool isAboveUser, Guid positionProviderId, string confettiMachinePrefabPath, float radius, float duration, BehaviorExecutionStages executionStages)
        {
            Data.IsAboveUser = isAboveUser;
            Data.ConfettiPosition = new SingleSceneObjectReference(positionProviderId);
            Data.ConfettiMachinePrefabPath = confettiMachinePrefabPath;
            Data.AreaRadius = radius;
            Data.Duration = duration;
            Data.ExecutionStages = executionStages;

            if (string.IsNullOrEmpty(Data.ConfettiMachinePrefabPath) && RuntimeConfigurator.Exists)
            {
                Data.ConfettiMachinePrefabPath = RuntimeConfigurator.Configuration.SceneConfiguration.DefaultConfettiPrefab;
            }
        }

        private class EmitConfettiProcess : StageProcess<EntityData>
        {
            private readonly BehaviorExecutionStages stages;
            private float timeStarted;
#if UNITY_6000_0_OR_NEWER
            private GameObject confettiPrefab;
            private List<GameObject> confettiMachines = new List<GameObject>();
#elif GODOT
            private PackedScene confettiPrefab;
            private readonly List<Node> confettiMachines = new();
#endif


            public EmitConfettiProcess(EntityData data, BehaviorExecutionStages stages) : base(data)
            {
                this.stages = stages;
            }

            /// <inheritdoc />
            public override void Start()
            {
                if (ShouldExecuteCurrentStage(Data) == false)
                {
                    return;
                }

                // Load the given prefab and stop the coroutine if not possible.
#if UNITY_6000_0_OR_NEWER
                confettiPrefab = Resources.Load<GameObject>(Data.ConfettiMachinePrefabPath);
#elif GODOT
                confettiPrefab = ResourceLoader.Load<PackedScene>(Data.ConfettiMachinePrefabPath);
#endif

                if (confettiPrefab == null)
                {
#if UNITY_6000_0_OR_NEWER
                    Debug.LogWarning("No valid prefab path provided.");
#elif GODOT
                    GD.PushWarning("No valid prefab path provided.");
#endif
                    return;
                }

                if (Data.IsAboveUser)
                {
                    foreach (var user in RuntimeConfigurator.Configuration.UserTransforms)
                    {
                        Vector3 spawnPosition;
#if UNITY_6000_0_OR_NEWER
                        spawnPosition = user.Head.position;
                        spawnPosition.y += distanceAboveUser;
#elif GODOT
                        spawnPosition = user.Head.Position;
                        spawnPosition.Y += distanceAboveUser;
#endif

                        CreateConfettiMachine(spawnPosition);
                    }
                }
                else
                {
#if UNITY_6000_0_OR_NEWER
                    CreateConfettiMachine(Data.ConfettiPosition.Value.GameObject.transform.position);
#elif GODOT
                    if (Data.ConfettiPosition.Value.GameObject is Node3D valueGameObject)
                        CreateConfettiMachine(valueGameObject.Position);
                    else
                        GD.PushWarning($"{Data.ConfettiPosition.Value.GameObject} needs to be a Node3D.");
#endif
                }

                if (Data.Duration > 0f)
                {
#if UNITY_6000_0_OR_NEWER
                    timeStarted = Time.time;
#elif GODOT
                    timeStarted = GetTimeSinceStart();
#endif
                }
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                if (ShouldExecuteCurrentStage(Data) == false)
                {
                    yield break;
                }

                if (confettiMachines.Count == 0)
                {
                    yield break;
                }

                if (Data.Duration > 0)
                {
#if UNITY_6000_0_OR_NEWER
                    while (Time.time - timeStarted < Data.Duration)
                    {
                        yield return null;
                    }
#elif GODOT
                    while (GetTimeSinceStart() - timeStarted < Data.Duration)
                        yield return null;
#endif
                }
            }

            /// <inheritdoc />
            public override void End()
            {
                if (ShouldExecuteCurrentStage(Data))
                {
#if UNITY_6000_0_OR_NEWER
                    foreach (GameObject confettiMachine in confettiMachines)
                    {
                        Object.Destroy(confettiMachine);
                    }
#elif GODOT
                    foreach (Node confettiMachine in confettiMachines)
                        confettiMachine.QueueFree();
#endif

                    confettiMachines.Clear();
                }
            }

            /// <inheritdoc />
            public override void FastForward() { }

            private bool ShouldExecuteCurrentStage(EntityData data)
            {
                return (data.ExecutionStages & stages) > 0;
            }

#if UNITY_6000_0_OR_NEWER
            private void CreateConfettiMachine(Vector3 spawnPosition)
            {
                // Spawn the machine and check if it has the interface IParticleMachine
                GameObject confettiMachine = RuntimeConfigurator.Configuration.SceneObjectManager.InstantiatePrefab(confettiPrefab, spawnPosition, Quaternion.Euler(90, 0, 0));

                if (confettiMachine == null)
                {
                    Debug.LogWarning("The provided prefab is missing.");
                    return;
                }

                if (confettiMachine.GetComponent(typeof(IParticleMachine)) == null)
                {
                    Debug.LogWarning("The provided prefab does not have any component of type \"IParticleMachine\".");
                    return;
                }

                confettiMachines.Add(confettiMachine);

                // Change the settings and activate the machine
                IParticleMachine particleMachine = confettiMachine.GetComponent<IParticleMachine>();
                particleMachine.Activate(Data.AreaRadius, Data.Duration);
            }
#elif GODOT
            private void CreateConfettiMachine(Vector3 spawnPosition)
            {
                // Spawn the machine and check if it has the interface IParticleMachine
                if (RuntimeConfigurator.Configuration.SceneObjectManager.InstantiatePrefab(confettiPrefab, spawnPosition, Quaternion.FromEuler(new Vector3(90, 0, 0))) is not Node confettiMachine)
                {
                    GD.PushWarning($"Couldn't instantiate confetti machine prefab. ({confettiPrefab})");
                    return;
                }

                if (confettiMachine.GetComponent(typeof(IParticleMachine)) == null)
                {
                    GD.PushWarning("The provided prefab does not have any component of type \"IParticleMachine\".");
                    return;
                }

                confettiMachines.Add(confettiMachine);

                // Change the settings and activate the machine
                if (confettiMachine.GetComponent<IParticleMachine>() is IParticleMachine particleMachine)
                    particleMachine.Activate(Data.AreaRadius, Data.Duration);
            }
#endif
        }


        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new EmitConfettiProcess(Data, BehaviorExecutionStages.Activation);
        }

        /// <inheritdoc />
        public override IStageProcess GetDeactivatingProcess()
        {
            return new EmitConfettiProcess(Data, BehaviorExecutionStages.Deactivation);
        }

#if GODOT
        static float GetTimeSinceStart()
        {
            return Time.GetTicksMsec() / 1000f;
        }
#endif
    }
}
