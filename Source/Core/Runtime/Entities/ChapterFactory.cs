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
    /// Factory implementation for <see cref="IChapter"/> objects.
    /// </summary>
#if UNITY_5_3_OR_NEWER
    internal class ChapterFactory : Singleton<ChapterFactory>
    {
#elif GODOT
    [GlobalClass]
    internal partial class ChapterFactory : GodotObject
    {
        private static ChapterFactory instance = new ChapterFactory();
        public static ChapterFactory Instance => instance;
#endif
        /// <summary>
        /// Creates a new <see cref="IChapter"/>.
        /// </summary>
        /// <param name="name"><see cref="IChapter"/>'s name.</param>
        public IChapter Create(string name)
        {
            return new Chapter(name, null);
        }
    }
}
