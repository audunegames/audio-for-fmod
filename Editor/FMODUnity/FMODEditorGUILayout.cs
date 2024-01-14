using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines GUI layout methods for FMOD in the Unity editor
  public static class FMODEditorGUILayout
  {
    // Draw a dropdown for FMOD banks
    public static void BankDropdownField(GUIContent label, SerializedProperty property)
    {
      FMODEditorGUI.BankDropdownField(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD event references
    public static void EventReferenceDropdownField(GUIContent label, SerializedProperty property)
    {
      FMODEditorGUI.EventReferenceDropdownField(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD mixer buses
    public static void MixerBusDropdownField(GUIContent label, SerializedProperty property)
    {
      FMODEditorGUI.MixerBusDropdownField(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD mixer VCAs
    public static void MixerVCADropdownField(GUIContent label, SerializedProperty property)
    {
      FMODEditorGUI.MixerVCADropdownField(EditorGUILayout.GetControlRect(), label, property);
    }
  }
}