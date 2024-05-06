// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if GODOT
using Godot;
#endif
using VRBuilder.Unity;

namespace VRBuilder.Core
{
    /// <summary>
    /// Factory implementation for <see cref="IStep"/> objects.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    internal class StepFactory : Singleton<StepFactory>
    {
#elif GODOT
    [GlobalClass]
    internal partial class StepFactory : GodotObject
    {
        private static StepFactory instance = new StepFactory();
        public static StepFactory Instance => instance;
#endif

        /// <summary>
        /// Creates a new <see cref="IStep"/>.
        /// </summary>
        /// <param name="name"><see cref="IStep"/>'s name.</param>
        public IStep Create(string name)
        {
            return new Step(name);
        }
    }
}
