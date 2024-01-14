using FMODUnity;
using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for an audio event
  [CustomPropertyDrawer(typeof(AudioEvent))]
  public class AudioEventDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);

      var eventProperty = property.FindPropertyRelative("_event");
      var typeProperty = property.FindPropertyRelative("_type");
      var audioTableModeProperty = property.FindPropertyRelative("_audioTableMode");
      var audioTableKeysProperty = property.FindPropertyRelative("_audioTableKeys");

      var isEventNotNull = !((EventReference)eventProperty.boxedValue).IsNull;
      var isOneShotAudioTable = (AudioEventType)typeProperty.enumValueIndex == AudioEventType.OneShotAudioTable;

      position.Set(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
      var fieldPosition = new Rect(position.x, position.y, position.width - 18, position.height);
      FMODEditorGUI.EventReferenceDropdownField(fieldPosition, label, eventProperty);
      var foldoutPosition = new Rect(position.x + position.width - 16, position.y, 16, position.height);
      property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, GUIContent.none, true);

      if (property.isExpanded)
      {
        using (new EditorGUI.IndentLevelScope())
        {
          if (isEventNotNull)
          {
            position.Set(position.x, position.y + position.height + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUI.GetPropertyHeight(typeProperty));
            EditorGUI.PropertyField(position, typeProperty);
          }

          if (isEventNotNull && isOneShotAudioTable)
          {
            position.Set(position.x, position.y + position.height + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUI.GetPropertyHeight(audioTableModeProperty));
            EditorGUI.PropertyField(position, audioTableModeProperty);

            position.Set(position.x, position.y + position.height + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUI.GetPropertyHeight(audioTableKeysProperty));
            EditorGUI.PropertyField(position, audioTableKeysProperty);
          }
        }
      }

      EditorGUI.EndProperty();
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var eventProperty = property.FindPropertyRelative("_event");
      var typeProperty = property.FindPropertyRelative("_type");
      var audioTableModeProperty = property.FindPropertyRelative("_audioTableMode");
      var audioTableKeysProperty = property.FindPropertyRelative("_audioTableKeys");

      var isEventNotNull = !((EventReference)eventProperty.boxedValue).IsNull;
      var isOneShotAudioTable = (AudioEventType)typeProperty.enumValueIndex == AudioEventType.OneShotAudioTable;

      var height = EditorGUIUtility.singleLineHeight;
      if (property.isExpanded)
      {
        if (isEventNotNull)
          height += EditorGUI.GetPropertyHeight(typeProperty) + EditorGUIUtility.standardVerticalSpacing;
        if (isEventNotNull && isOneShotAudioTable)
          height += EditorGUI.GetPropertyHeight(audioTableModeProperty) + EditorGUI.GetPropertyHeight(audioTableKeysProperty) + EditorGUIUtility.standardVerticalSpacing * 2;
      }
      return height;
    }
  }
}