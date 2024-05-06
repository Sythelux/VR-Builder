// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VRBuilder.Editor")]
[assembly: InternalsVisibleTo("VRBuilder.Core.Tests.PlayMode")]
[assembly: InternalsVisibleTo("VRBuilder.Core.Tests.EditMode")]

#elif GODOT
using Godot;
//TODO
#endif
