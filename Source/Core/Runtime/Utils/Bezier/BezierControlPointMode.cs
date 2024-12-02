#if UNITY_5_6_OR_NEWER
namespace VRBuilder.Core.Utils.Bezier
{
    /// <summary>
    /// Control point modes for <see cref="BezierSpline"/>.
    /// </summary>
	public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }
}

#elif GODOT
using Godot;
//TODO
#endif
