// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2024 MindPort GmbH
#if UNITY_5_3_OR_NEWER

namespace VRBuilder.Unity
{
    /// <summary>
    /// Auxiliary class that allows starting UnityCoroutines from a context that is not
    /// itself a MonoBehaviour.
    /// </summary>
    public class CoroutineDispatcher : UnitySingleton<CoroutineDispatcher>
    {
    }
}
#endif
