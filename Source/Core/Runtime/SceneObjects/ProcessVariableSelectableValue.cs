#if UNITY_5_3_OR_NEWER
using System.Runtime.Serialization;
using VRBuilder.Core.Properties;

namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Selectable value implementation for process variables.
    /// </summary>    
    [DataContract(IsReference = true)]
    public class ProcessVariableSelectableValue<T> : SelectableValue<T, SingleScenePropertyReference<IDataProperty<T>>>
    {
        public override string FirstValueLabel => "Constant";

        public override string SecondValueLabel => "Data Property";
    }
}

#elif GODOT
using Godot;
//TODO
#endif
