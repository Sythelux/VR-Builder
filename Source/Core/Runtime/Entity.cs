// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

using System;
using System.Linq;
using System.Runtime.Serialization;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.EntityOwners;

namespace VRBuilder.Core
{
    /// <summary>
    /// Abstract helper class that can be used for instances that implement <see cref="IEntity"/>. Provides implementation of the events and properties, and also
    /// offers member functions to trigger state changes.
    /// </summary>
    [DataContract(IsReference = true)]
#if UNITY_5_3_OR_NEWER
    public abstract class Entity<TData> : IEntity, IDataOwner<TData> where TData : class, IData, new()
#elif GODOT
    public abstract partial class Entity<TData> : GodotObject, IEntity, IDataOwner<TData> where TData : class, IData, new()
#endif
    {
        /// <inheritdoc />
        [DataMember]
        public TData Data { get; private set; }

        /// <inheritdoc />
        IData IDataOwner.Data => ((IDataOwner<TData>)this).Data;

        /// <inheritdoc />
        [IgnoreDataMember]
        public ILifeCycle LifeCycle { get; }

        /// <inheritdoc />
        [IgnoreDataMember]
        public IEntity Parent { get; set; }

        protected Entity()
    {
        LifeCycle = new LifeCycle(this);
        Data = new TData();
    }

        /// <inheritdoc />
        public virtual IStageProcess GetActivatingProcess()
    {
        return new EmptyProcess();
    }

        /// <inheritdoc />
        public virtual IStageProcess GetActiveProcess()
    {
        return new EmptyProcess();
    }

        /// <inheritdoc />
        public virtual IStageProcess GetDeactivatingProcess()
    {
        return new EmptyProcess();
    }

        /// <inheritdoc />
        public virtual IStageProcess GetAbortingProcess()
        {
            return new EmptyProcess();
        }

        /// <summary>
        /// Override this method if your behavior or condition supports changing between process modes (<see cref="IMode"/>).
        /// By default returns an empty configurator that does nothing.
        /// </summary>
        protected virtual IConfigurator GetConfigurator()
    {
        return new EmptyConfigurator();
    }

        /// <inheritdoc />
        public virtual void Configure(IMode mode)
    {
        if (Data is IEntityCollectionData collectionData)
            foreach (IEntity child in collectionData.GetChildren().Distinct())
            {
                child.Parent = this;
                child.Configure(mode);
            }

        GetConfigurator().Configure(mode, LifeCycle.Stage);

        if (Data is IModeData modeData) modeData.Mode = mode;
    }

        /// <inheritdoc />
        public void Update()
        {
            try
            {
	            LifeCycle.Update();

	            // IStepData implements IEntitySequenceData despite not being a sequence,
	            // so we have to manually exclude it.
	            if (Data is IEntitySequenceData sequenceData && Data is IStepData == false)
	            {
	                sequenceData.Current?.Update();
	            }
	            else if (Data is IEntityCollectionData collectionData)
	            {
	                foreach (IEntity child in collectionData.GetChildren().Distinct())
	                {
	                    child.Update();
	                }
	            }
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"Exception in Step: {(Data as Step.EntityData)?.Name}. In LifeCycle: {LifeCycle.Stage}");
                Debug.LogException(e);
#endif
            }
        }
    }
}
