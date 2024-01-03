using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines a property drawer for a reference to a bank
  public sealed class FMODBankAttributeDrawer : OdinAttributeDrawer<FMODBankAttribute, string>
  {
    // Draw the property layout
    protected override void DrawPropertyLayout(GUIContent label)
    {
      var selected = ValueEntry.SmartValue != null ? FMODEditorGUI.BankDropdown(label, FMODStudio.GetBank(ValueEntry.SmartValue)) : null;
      ValueEntry.SmartValue = selected?.path ?? string.Empty;
    }
  }
}