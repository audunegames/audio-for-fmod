using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to a bus
  public sealed class FMODMixerBusAttributeDrawer : OdinAttributeDrawer<FMODMixerBusAttribute, string>
  {
    // Draw the property layout
    protected override void DrawPropertyLayout(GUIContent label)
    {
      var selected = ValueEntry.SmartValue != null ? FMODEditorGUI.MixerBusDropdown(label, FMODStudio.GetMixerBus(ValueEntry.SmartValue)) : null;
      ValueEntry.SmartValue = selected?.path ?? string.Empty;
    }
  }
}