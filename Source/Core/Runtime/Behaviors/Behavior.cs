// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System.Runtime.Serialization;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
using VRBuilder.Unity;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Utils.Logging;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Inherit from this abstract class when creating your own behaviors.
    /// </summary>
    /// <typeparam name="TData">The type of the behavior's data.</typeparam>
    [DataContract(IsReference = true)]
    public abstract partial class Behavior<TData> : Entity<TData>, IBehavior where TData : class, IBehaviorData, new()
    {
        /// <inheritdoc />
        IBehaviorData IDataOwner<IBehaviorData>.Data
        {
            get { return Data; }
        }

        protected Behavior()
        {
            if (LifeCycleLoggingConfig.Instance.LogBehaviors)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogFormat("{0}<b>Behavior</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
#elif GODOT
                    GD.PrintRich($"\t\t[b]Behavior[/b] [i]'{Data.Name} ({GetType().Name})'[/i] is [b]{LifeCycle.Stage}[/b].\n");
#endif
                };
            }
        }

        /// <inheritdoc />
        public virtual IBehavior Clone()
        {
            return MemberwiseClone() as IBehavior;
        }
    }
}
