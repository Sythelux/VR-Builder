// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER

using System;
using System.Reflection;

namespace VRBuilder.Core.Attributes
{
    /// <summary>
    /// Declare that this element has to be separated with thin gray lines.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SeparatedAttribute : MetadataAttribute
    {
        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool IsMetadataValid(object metadata)
        {
            return metadata == null;
        }
    }
}

#elif GODOT
using Godot;
//TODO
#endif
