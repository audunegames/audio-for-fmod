using FMODUnity;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to an event description
  public sealed class FMODEventReferenceDrawer : OdinValueDrawer<EventReference>
  {
    // Draw the property layout
    protected override void DrawPropertyLayout(GUIContent label)
    {
      var position = EditorGUILayout.GetControlRect();

      // Draw the label
      if (label != null)
        position = EditorGUI.PrefixLabel(position, label);

      // Draw the selector
      ValueEntry.SmartValue = FMODEditorGUI.EventReferenceDropdown(position, ValueEntry.SmartValue);
    }
  }
}