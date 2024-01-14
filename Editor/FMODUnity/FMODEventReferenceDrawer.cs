using FMODUnity;
using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to an event description
  [CustomPropertyDrawer(typeof(EventReference))]
  public sealed class FMODEventReferenceDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      FMODEditorGUI.EventReferenceDropdownField(position, label, property);
    }
  }
}