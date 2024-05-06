#if UNITY_5_3_OR_NEWER
﻿using System;

namespace VRBuilder.TextToSpeech
{
    public class UnableToParseAudioFormatException : Exception
    {
        public UnableToParseAudioFormatException(string msg) : base(msg) { }
    }
}

#elif GODOT
using Godot;
//TODO
#endif
