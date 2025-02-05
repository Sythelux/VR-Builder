#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;
#endif
using VRBuilder.Core.Editor.UI.GraphView.Nodes;

namespace VRBuilder.Core.Editor.UI.GraphView.Instantiators
{
    /// <summary>
    /// Instantiator for the End Chapter node.
    /// </summary>
    public class EndChapterNodeInstantiator : IStepNodeInstantiator
    {
        /// <inheritdoc/>
        public string Name => "End Chapter";

        /// <inheritdoc/>
        public bool IsInNodeMenu => true;

        /// <inheritdoc/>
        public int Priority => 150;

        /// <inheritdoc/>
        public string StepType => "endChapter";

        /// <inheritdoc/>
#if UNITY_5_3_OR_NEWER
        public DropdownMenuAction.Status GetContextMenuStatus(IEventHandler target, IChapter currentChapter)
        {
            return DropdownMenuAction.Status.Normal;
        }
#endif

        /// <inheritdoc/>
        public ProcessGraphNode InstantiateNode(IStep step)
        {
            return null; //TODO: return new EndChapterNode(step);
        }
    }
}
