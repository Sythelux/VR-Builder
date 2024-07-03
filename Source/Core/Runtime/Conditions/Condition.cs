// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System.Collections.Generic;
using System.Runtime.Serialization;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
using VRBuilder.Unity;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.RestrictiveEnvironment;
using VRBuilder.Core.Utils.Logging;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// An implementation of <see cref="ICondition"/>. Use it as the base class for your custom conditions.
    /// </summary>
    [DataContract(IsReference = true)]
#if UNITY_5_3_OR_NEWER
    public abstract class Condition<TData> : CompletableEntity<TData>, ICondition, ILockablePropertiesProvider where TData : class, IConditionData, new()
#elif GODOT
    public abstract partial class Condition<TData> : CompletableEntity<TData>, ICondition, ILockablePropertiesProvider where TData : class, IConditionData, new()
#endif

    {
        protected Condition()
        {
            if (LifeCycleLoggingConfig.Instance.LogConditions)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogFormat("{0}<b>Condition</b> <i>'{1} ({2})'</i> is <b>{3}</b>.\n", ConsoleUtils.GetTabs(2), Data.Name, GetType().Name, LifeCycle.Stage);
#elif GODOT
                    GD.PrintRich("{0}[b]Condition[/b] [i]'{1} ({2})'[/i] is [b]{3}[/b].\n", "\t\t", Data.Name, GetType().Name, LifeCycle.Stage);
#endif
                };
            }
        }

        /// <inheritdoc />
        IConditionData IDataOwner<IConditionData>.Data
        {
            get
            {
                return Data;
            }
        }

        /// <inheritdoc />
        public virtual ICondition Clone()
        {
            return MemberwiseClone() as ICondition;
        }

        /// <inheritdoc />
        public virtual IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            return PropertyReflectionHelper.ExtractLockablePropertiesFromCondition(Data);
        }
    }
}
