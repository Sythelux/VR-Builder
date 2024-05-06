#if UNITY_5_3_OR_NEWER
namespace VRBuilder.Core.Utils
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
