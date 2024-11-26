#if UNITY_5_3_OR_NEWER
﻿namespace VRBuilder.BasicInteraction.RigSetup
{
    
    /// <summary>
    /// Does not initialize any rig.
    /// </summary>
    public class NoRigSetup : InteractionRigProvider
    {
        /// <inheritdoc/>
        public override string Name { get; } = "<None>";
        
        /// <inheritdoc/>
        public override string PrefabName { get; } = null;
    }
}
#elif GODOT
using Godot;
//TODO
#endif
