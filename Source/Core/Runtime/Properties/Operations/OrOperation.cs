#if UNITY_5_3_OR_NEWER
using System;

namespace VRBuilder.Core.Properties.Operations
{
    /// <summary>
    /// "Or" boolean operation.
    /// </summary>
    public class OrOperation : IOperationCommand<bool, bool>
    {
        /// <inheritdoc/>
        public bool Execute(bool leftOperand, bool rightOperand)
        {
            return leftOperand || rightOperand;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "||";
        }
    }
}
#elif GODOT
using Godot;
//TODO
#endif
