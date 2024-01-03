using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to a VCA
  public sealed class FMODMixerVCAAttributeDrawer : OdinAttributeDrawer<FMODMixerVCAAttribute, string>
  {
    // Draw the property layout
    protected override void DrawPropertyLayout(GUIContent label)
    {
      var selected = ValueEntry.SmartValue != null ? FMODEditorGUI.MixerVCADropdown(label, FMODStudio.GetMixerVCA(ValueEntry.SmartValue)) : null;
      ValueEntry.SmartValue = selected?.path ?? string.Empty;
    }
  }
}