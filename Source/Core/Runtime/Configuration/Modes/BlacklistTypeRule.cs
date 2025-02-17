// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER

using System;

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// All listed types will be invalid.
    /// </summary>
    /// <typeparam name="TBase">Type which can be filtered.</typeparam>
    public class BlacklistTypeRule<TBase> : ListTypeRule<BlacklistTypeRule<TBase>, TBase>
    {
        /// <inheritdoc />
        protected override bool IsQualifiedByPredicate(Type type)
        {
            return StoredTypes.Contains(type) == false;
        }
    }
}

#elif GODOT
using Godot;
//TODO
#endif
