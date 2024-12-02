#if UNITY_5_3_OR_NEWER
using System;

namespace VRBuilder.Core.Properties.Operations
{
    /// <summary>
    /// "And" boolean operation.
    /// </summary>
    public class AndOperation : IOperationCommand<bool, bool>
    {
        /// <inheritdoc/>
        public bool Execute(bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "&&";
        }
    }
}
#elif GODOT
using Godot;
//TODO
#endif
