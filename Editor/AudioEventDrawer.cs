using FMODUnity;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for an audio event
  public class AudioEventDrawer : OdinValueDrawer<AudioEvent>
  {
    // Properties of the audio event
    private InspectorProperty _event;
    private InspectorProperty _type;
    private InspectorProperty _audioTableMode;
    private InspectorProperty _audioTableKeys;


    // Return if the event is not null
    public bool IsEventNotNull => !((EventReference)_event.ValueEntry.WeakSmartValue).IsNull;

    // Return if the type is an one shot audio table
    public bool IsOneShotAudioTable => (AudioEventType)_type.ValueEntry.WeakSmartValue == AudioEventType.OneShotAudioTable;


    // Initialize the drawer
    protected override void Initialize()
    {
      _event = Property.Children["_event"];
      _type = Property.Children["_type"];
      _audioTableMode = Property.Children["_audioTableMode"];
      _audioTableKeys = Property.Children["_audioTableKeys"];
    }

    // Draw the property layout
    protected override void DrawPropertyLayout(GUIContent label)
    {
      // Draw the label and event property
      SirenixEditorGUI.BeginVerticalPropertyLayout(new GUIContent(" "), out var labelPosition);
      Property.State.Expanded = EditorGUI.Foldout(labelPosition, Property.State.Expanded, label, true);
      if (!IsEventNotNull)
        Property.State.Expanded = false;

      _event.Draw(GUIContent.none);

      SirenixEditorGUI.EndVerticalPropertyLayout();

      // Draw the foldout
      if (Property.State.Expanded)
      {
        using (new EditorGUI.IndentLevelScope())
        {
          // Draw the type property
          _type.State.Visible = IsEventNotNull;
          _type.Draw();

          // Draw the audio table keys property
          _audioTableMode.State.Visible = IsEventNotNull && IsOneShotAudioTable;
          _audioTableMode.Draw();

          // Draw the audio table keys property
          _audioTableKeys.State.Visible = IsEventNotNull && IsOneShotAudioTable;
          _audioTableKeys.Draw();
        }
      }
    }
  }
}