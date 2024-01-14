using FMODUnity;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines GUI methods for FMOD in the Unity editor
  public static class FMODEditorGUI
  {
    // Create a search window context for a rect
    private static SearchWindowContext CreateSearchWindowContext(Rect position)
    {
      return new SearchWindowContext(GUIUtility.GUIToScreenPoint(position.max + new Vector2(position.width * -0.5f, position.height - EditorGUIUtility.standardVerticalSpacing - 1)), position.width);
    }


    // Draw a dropdown for FMOD banks at the specified position
    public static void BankDropdownField(Rect position, GUIContent label, SerializedProperty property)
    {
      EditorGUI.BeginProperty(position, label, property);

      position = EditorGUI.PrefixLabel(position, label);

      var buttonLabel = !string.IsNullOrEmpty(property.stringValue) ? property.stringValue : "None";
      if (GUI.Button(position, buttonLabel, EditorStyles.popup))
        SearchWindow.Open(CreateSearchWindowContext(position), FMODBankSearchProvider.Create(FMODStudio.banks, (selected) => {
          property.serializedObject.Update();
          property.stringValue = selected?.path ?? null;
          property.serializedObject.ApplyModifiedProperties();
        }));

      EditorGUI.EndProperty();
    }

    // Draw a dropdown for FMOD event references at the specified position
    public static void EventReferenceDropdownField(Rect position, GUIContent label, SerializedProperty property)
    {
      EditorGUI.BeginProperty(position, label, property);

      position = EditorGUI.PrefixLabel(position, label);

      var value = (EventReference)property.boxedValue;
      var buttonLabel = !value.IsNull ? value.Path : "None";
      if (GUI.Button(position, buttonLabel, EditorStyles.popup))
      {
        SearchWindow.Open(CreateSearchWindowContext(position), FMODEventReferenceSearchProvider.Create(FMODStudio.banks, (selected) => {
          property.serializedObject.Update();
          property.boxedValue = selected;
          property.serializedObject.ApplyModifiedProperties();
        }));
      }

      EditorGUI.EndProperty();
    }

    // Draw a dropdown for FMOD mixer buses at the specified position
    public static void MixerBusDropdownField(Rect position, GUIContent label, SerializedProperty property)
    {
      EditorGUI.BeginProperty(position, label, property);

      position = EditorGUI.PrefixLabel(position, label);

      var buttonLabel = !string.IsNullOrEmpty(property.stringValue) ? property.stringValue : "None";
      if (GUI.Button(position, buttonLabel, EditorStyles.popup))
      {
        SearchWindow.Open(CreateSearchWindowContext(position), FMODMixerBusSearchProvider.Create(FMODStudio.banks, (selected) => {
          property.serializedObject.Update();
          property.stringValue = selected?.path ?? null;
          property.serializedObject.ApplyModifiedProperties();
        }));
      }

      EditorGUI.EndProperty();
    }

    // Draw a dropdown for FMOD mixer VCAs at the specified position
    public static void MixerVCADropdownField(Rect position, GUIContent label, SerializedProperty property)
    {
      EditorGUI.BeginProperty(position, label, property);

      position = EditorGUI.PrefixLabel(position, label);

      var buttonLabel = !string.IsNullOrEmpty(property.stringValue) ? property.stringValue : "None";
      if (GUI.Button(position, buttonLabel, EditorStyles.popup))
      {
        SearchWindow.Open(CreateSearchWindowContext(position), FMODMixerVCASearchProvider.Create(FMODStudio.banks, (selected) => {
          property.serializedObject.Update();
          property.stringValue = selected?.path ?? null;
          property.serializedObject.ApplyModifiedProperties();
        }));
      }

      EditorGUI.EndProperty();
    }
  }
}