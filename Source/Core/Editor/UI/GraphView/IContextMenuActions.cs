#if UNITY_6000_0_OR_NEWER
using UnityEngine.UIElements;
#elif GODOT
using Godot;
#endif


namespace VRBuilder.Core.Editor.UI.GraphView
{
    /// <summary>
    /// Interface for a process graph object having custom context menu actions.
    /// </summary>
    public interface IContextMenuActions
    {
        /// <summary>
        /// Adds custom actions to the context menu.
        /// </summary>        
#if UNITY_6000_0_OR_NEWER
        void AddContextMenuActions(DropdownMenu menu);
#elif GODOT
        void AddContextMenuActions(PopupMenu menu);
#endif

    }
}
