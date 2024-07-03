// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH

#if UNITY_5_3_OR_NEWER
using VRBuilder.Unity;
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core
{
    /// <summary>
    /// Factory implementation for <see cref="ITransition"/> objects.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    internal class TransitionFactory : Singleton<TransitionFactory>
    {
#elif GODOT
    [GlobalClass]
    internal partial class TransitionFactory : GodotObject
    {
        private static TransitionFactory instance = new TransitionFactory();
        public static TransitionFactory Instance => instance;
#endif

        /// <summary>
        /// Creates a new <see cref="ITransition"/>.
        /// </summary>
        public ITransition Create()
        {
            return new Transition();
        }
    }
}
