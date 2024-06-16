using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to a bank
  [CustomPropertyDrawer(typeof(FMODBankAttribute))]
  public sealed class FMODBankAttributeDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
        FMODEditorGUI.BankDropdown(position, label, property);
      else
        EditorGUI.LabelField(position, label, "The [FMODBank] attribute can only be used with string properties");
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}