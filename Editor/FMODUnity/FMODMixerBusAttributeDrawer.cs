using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to a bus
  [CustomPropertyDrawer(typeof(FMODMixerBusAttribute))]
  public sealed class FMODMixerBusAttributeDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
        FMODEditorGUI.MixerBusDropdown(position, label, property);
      else
        EditorGUI.LabelField(position, label, "The [FMODMixerBus] attribute can only be used with string properties");
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}