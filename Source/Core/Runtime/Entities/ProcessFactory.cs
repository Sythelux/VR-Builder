// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER
#elif GODOT
using Godot;
#endif

namespace VRBuilder.Core
{
    /// <summary>
    /// Factory implementation for <see cref="IProcess"/> objects.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    internal class ProcessFactory : Singleton<ProcessFactory>
    {
#elif GODOT
    [GlobalClass]
    internal partial class ProcessFactory : GodotObject
    {
        private static ProcessFactory instance = new ProcessFactory();
        public static ProcessFactory Instance => instance;
#endif

        /// <summary>
        /// Creates a new <see cref="IProcess"/>.
        /// </summary>
        /// <param name="name"><see cref="IProcess"/>'s name.</param>
        /// <param name="firstStep">Initial <see cref="IStep"/> for this <see cref="IProcess"/>.</param>
        public IProcess Create(string name, IStep firstStep = null)
        {
            return new Process(name, new Chapter("Chapter 1", firstStep));
        }
    }
}
