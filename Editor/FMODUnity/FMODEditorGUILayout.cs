using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines GUI layout methods for FMOD in the Unity editor
  public static class FMODEditorGUILayout
  {
    // Draw a dropdown for FMOD banks
    public static void BankDropdown(GUIContent label, SerializedProperty property, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      FMODEditorGUI.BankDropdown(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD event references
    public static void EventReferenceDropdown(GUIContent label, SerializedProperty property, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      FMODEditorGUI.EventReferenceDropdown(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD mixer buses
    public static void MixerBusDropdown(GUIContent label, SerializedProperty property, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      FMODEditorGUI.MixerBusDropdown(EditorGUILayout.GetControlRect(), label, property);
    }

    // Draw a dropdown for FMOD mixer VCAs
    public static void MixerVCADropdown(GUIContent label, SerializedProperty property, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      FMODEditorGUI.MixerVCADropdown(EditorGUILayout.GetControlRect(), label, property);
    }
  }
}