using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#elif GODOT
using Godot;
#endif
using Newtonsoft.Json;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// A condition that completes when a certain amount of time has passed.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://www.mindport.co/vr-builder/manual/default-conditions/timeout-condition")]
    public partial class TimeoutCondition : Condition<TimeoutCondition.EntityData>
    {
        /// <summary>
        /// The data for timeout condition.
        /// </summary>
        [DisplayName("Timeout")]
        public class EntityData : IConditionData
        {
            /// <summary>
            /// The delay before the condition completes.
            /// </summary>
            [DataMember]
            [DisplayName("Wait (in seconds)")]
            public float Timeout { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [IgnoreDataMember]
            [HideInProcessInspector]
            public string Name
            {
                get
                {
                    return $"Complete after {Timeout.ToString()} seconds";
                }
            }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            private float timeStarted;

            /// <inheritdoc />
            protected override bool CheckIfCompleted()
            {
#if UNITY_6000_0_OR_NEWER
                return Time.time - timeStarted >= Data.Timeout;
#elif GODOT
                return Time.GetTicksMsec() - timeStarted >= Data.Timeout;
#endif
            }

            /// <inheritdoc />
            public override void Start()
            {
#if UNITY_6000_0_OR_NEWER
                timeStarted = Time.time;
#elif GODOT
                timeStarted = Time.GetTicksMsec();
#endif
                base.Start();
            }
        }

        [JsonConstructor, Preserve]
        public TimeoutCondition() : this(0)
        {
        }

        public TimeoutCondition(float timeout)
        {
            Data.Timeout = timeout;
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}