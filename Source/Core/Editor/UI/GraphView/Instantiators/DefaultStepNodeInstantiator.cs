#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;
#endif
using VRBuilder.Core.Editor.UI.GraphView.Nodes;

namespace VRBuilder.Core.Editor.UI.GraphView.Instantiators
{
    /// <summary>
    /// Instantiator for a default <see cref="IStep"/> node.
    /// </summary>
    public class DefaultStepNodeInstantiator : IStepNodeInstantiator
    {
        /// <inheritdoc/>
        public string Name => "Step";

        /// <inheritdoc/>
        public bool IsInNodeMenu => true;

        /// <inheritdoc/>
        public string StepType => "default";

        /// <inheritdoc/>
        public int Priority => 100;

#if UNITY_6000_0_OR_NEWER
        /// <inheritdoc/>
        public DropdownMenuAction.Status GetContextMenuStatus(IEventHandler target, IChapter currentChapter)
        {
            return DropdownMenuAction.Status.Normal;
        }
#endif

        /// <inheritdoc/>
        public ProcessGraphNode InstantiateNode(IStep step)
        {
            return new StepGraphNode(step);
        }
    }
}
