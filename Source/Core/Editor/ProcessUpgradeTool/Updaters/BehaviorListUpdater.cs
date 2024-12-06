#if UNITY_6000_0_OR_NEWER
using VRBuilder.Core.Behaviors;

namespace VRBuilder.Core.Editor.ProcessUpgradeTool.Updaters
{
    /// <summary>
    /// Concrete implementation of <see cref="ListUpdater{T}"/>.
    /// </summary>
    public class BehaviorListUpdater : ListUpdater<IBehavior>
    {
    }
}

#endif
