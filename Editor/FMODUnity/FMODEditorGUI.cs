using FMODUnity;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines GUI utility methods for FMOD in the Unity editor
  public static class FMODEditorGUI
  {
    // Icon textures
    public static readonly Texture2D studioIcon = EditorUtils.LoadImage("StudioIcon.png");
    public static readonly Texture2D folderIcon = EditorUtils.LoadImage("FolderIconClosed.png");
    public static readonly Texture2D bankIcon = EditorUtils.LoadImage("BankIcon.png");
    public static readonly Texture2D eventIcon = EditorUtils.LoadImage("EventIcon.png");
    public static readonly Texture2D snapshotIcon = EditorUtils.LoadImage("SnapshotIcon.png");


    // Menu style for the selector tree
    public static readonly OdinMenuStyle selectorMenuStyle = OdinMenuStyle.TreeViewStyle
      .SetBorders(false)
      .SetHeight(18)
      .SetIconPadding(4.0f);


    // Create a bank selector
    private static BankSelector CreateBankSelector(Rect rect, FMODBank value)
    {
      var selector = new BankSelector();
      selector.SetSelection(value);
      selector.ShowInPopup(rect);
      return selector;
    }

    // Draw a dropdown for FMOD banks
    public static FMODBank BankDropdown(GUIContent label, FMODBank value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = BankSelector.DrawSelectorDropdown(label, valueLabel, rect => CreateBankSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }
    public static FMODBank BankDropdown(Rect position, FMODBank value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = BankSelector.DrawSelectorDropdown(position, valueLabel, rect => CreateBankSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }


    // Create an event reference selector
    private static EventReferenceSelector CreateEventReferenceSelector(Rect rect, EventReference value)
    {
      var selector = new EventReferenceSelector();
      selector.SetSelection(value);
      selector.ShowInPopup(rect);
      return selector;
    }

    // Draw a dropdown for FMOD event references
    public static EventReference EventReferenceDropdown(GUIContent label, EventReference value)
    {
      var valueLabel = !value.IsNull ? value.Path : "None";
      var selected = EventReferenceSelector.DrawSelectorDropdown(label, valueLabel, rect => CreateEventReferenceSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }
    public static EventReference EventReferenceDropdown(Rect position, EventReference value)
    {
      var valueLabel = !value.IsNull ? value.Path : "None";
      var selected = EventReferenceSelector.DrawSelectorDropdown(position, valueLabel, rect => CreateEventReferenceSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }


    // Create a mixer bus selector
    private static MixerBusSelector CreateMixerBusSelector(Rect rect, FMODMixerBus value)
    {
      var selector = new MixerBusSelector();
      selector.SetSelection(value);
      selector.ShowInPopup(rect);
      return selector;
    }

    // Draw a dropdown for FMOD mixer buses
    public static FMODMixerBus MixerBusDropdown(GUIContent label, FMODMixerBus value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = MixerBusSelector.DrawSelectorDropdown(label, valueLabel, rect => CreateMixerBusSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }
    public static FMODMixerBus MixerBusDropdown(Rect position, FMODMixerBus value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = MixerBusSelector.DrawSelectorDropdown(position, valueLabel, rect => CreateMixerBusSelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }


    // Create a mixer VCA selector
    private static MixerVCASelector CreateMixerVCASelector(Rect rect, FMODMixerVCA value)
    {
      var selector = new MixerVCASelector();
      selector.SetSelection(value);
      selector.ShowInPopup(rect);  
      return selector;
    }

    // Draw a dropdown for FMOD mixer VCAs
    public static FMODMixerVCA MixerVCADropdown(GUIContent label, FMODMixerVCA value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = MixerVCASelector.DrawSelectorDropdown(label, valueLabel, rect => CreateMixerVCASelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }
    public static FMODMixerVCA MixerVCADropdown(Rect position, FMODMixerVCA value)
    {
      var valueLabel = value != null ? value.path : "None";
      var selected = MixerVCASelector.DrawSelectorDropdown(position, valueLabel, rect => CreateMixerVCASelector(rect, value));
      return selected != null ? selected.FirstOrDefault() : value;
    }


    // Base class that defines a value selector
    private abstract class AbstractBankSelector<TValue> : OdinSelector<TValue>
    {
      // Build the selection tree of a bank
      protected abstract void BuildBankSelectionTree(OdinMenuTree tree, FMODBank bank);

      // Build the selection tree
      protected override void BuildSelectionTree(OdinMenuTree tree)
      {
        tree.Config.DrawSearchToolbar = true;
        tree.Config.DefaultMenuStyle = selectorMenuStyle;

        // Add the none value to the tree
        tree.Add("None", default(TValue));

        // Iterate over the banks and build their selection trees
        foreach (var bank in FMODStudio.banks)
          BuildBankSelectionTree(tree, bank);
      }
    }

    // Class that defines a selector for items in FMOD studio banks
    private abstract class AbstractBankItemSelector<TValue> : AbstractBankSelector<TValue>
    {
      // Class that defines an item in a selector for FMOD studio components
      public sealed class Item
      {
        // The value of the item
        public readonly TValue value;

        // The path of the item
        public readonly string path;

        // The icon of the item
        public readonly Texture icon;


        // Constructor
        public Item(TValue value, string path, Texture icon)
        {
          this.value = value;
          this.path = path;
          this.icon = icon;
        }
      }


      // Return the items in the specified bank
      protected abstract IEnumerable<Item> BuildBankItems(FMODBank bank);

      // Build the selection tree of a bank
      protected override void BuildBankSelectionTree(OdinMenuTree tree, FMODBank bank)
      {
        // Get the items and return if there are no items
        var items = BuildBankItems(bank);
        if (items.Count() == 0)
          return;

        // Add the bank to the tree
        tree.Add(bank.GetDisplayName(), null, bankIcon);

        // Iterate over the items in the bank
        foreach (var item in items)
        {
          // Add the folders of the description to the tree
          var components = item.path.Split("/");
          for (var i = 0; i < components.Length - 1; i++)
            tree.Add($"{bank.GetDisplayName()}/{string.Join("/", components.Take(i + 1))}", null, folderIcon);

          // Add the description to the tree
          tree.Add($"{bank.GetDisplayName()}/{item.path}", item.value, item.icon);
        }
      }
    }

    // Class that defines a selector for FMOD studio banks
    private class BankSelector : AbstractBankSelector<FMODBank>
    {
      // Build the selection tree of a bank
      protected override void BuildBankSelectionTree(OdinMenuTree tree, FMODBank bank)
      {
        // Add the bank to the tree
        tree.Add(bank.GetDisplayName(), bank, bankIcon);
      }
    }

    // Class that defines a selector for a reference to an event description
    private class EventReferenceSelector : AbstractBankItemSelector<EventReference>
    {
      // Return the items in the specified bank
      protected override IEnumerable<Item> BuildBankItems(FMODBank bank)
      {
        return bank.events.Select(description => new Item(description.GetEventReference(), description.GetDisplayName(), eventIcon));
      }

      // Draw info about the selected item
      [OnInspectorGUI]
      public void DrawInfoAboutSelectedItem()
      {
        var reference = GetCurrentSelection().FirstOrDefault();
        if (!reference.IsNull)
        {
          var description = FMODStudio.GetEvent(reference);

          EditorGUILayout.LabelField(new GUIContent(description.path), SirenixGUIStyles.BoldLabel);
          EditorGUILayout.LabelField(new GUIContent("Panning"), new GUIContent(description.is3D ? "3D" : "2D"));
          EditorGUILayout.LabelField(new GUIContent("Streaming"), new GUIContent(description.isStream.ToString()));
          EditorGUILayout.LabelField(new GUIContent("Oneshot"), new GUIContent(description.isOneshot.ToString()));

          GUILayout.Space(4.0f);

          using (new EditorGUIUtility.IconSizeScope(new Vector2(12, 12)))
          {
            if (GUILayout.Button(new GUIContent(" Show in FMOD Studio", FMODEditorGUI.studioIcon), SirenixGUIStyles.MiniButton))
              EditorUtils.SendScriptCommand($"studio.window.navigateTo(studio.project.lookup(\"{description.guid}\"))");
            GUILayout.Space(2.0f);
          }
        }
      }
    }

    // Class that defines a selector for a reference to an event description
    private class MixerBusSelector : AbstractBankItemSelector<FMODMixerBus>
    {
      // Return the items in specified the bank
      protected override IEnumerable<Item> BuildBankItems(FMODBank bank)
      {
        return bank.mixerBuses.Select(mixerBus => new Item(mixerBus, $"{bank.GetDisplayName()}/{mixerBus.GetDisplayName()}", snapshotIcon));
      }

      // Draw info about the selected item
      [OnInspectorGUI]
      public void DrawInfoAboutSelectedItem()
      {
        var mixerBus = GetCurrentSelection().FirstOrDefault();
        if (mixerBus != null)
        {
          EditorGUILayout.LabelField(new GUIContent(mixerBus.path), SirenixGUIStyles.BoldLabel);

          GUILayout.Space(4.0f);

          using (new EditorGUIUtility.IconSizeScope(new Vector2(12, 12)))
          {
            if (GUILayout.Button(new GUIContent(" Show in FMOD Studio", FMODEditorGUI.studioIcon), SirenixGUIStyles.MiniButton))
              EditorUtils.SendScriptCommand($"studio.window.navigateTo(studio.project.lookup(\"{mixerBus.guid}\"))");
            GUILayout.Space(2.0f);
          }
        }
      }
    }

    // Class that defines a selector for a reference to an event description
    private class MixerVCASelector : AbstractBankItemSelector<FMODMixerVCA>
    {
      // Return the items in the specified bank
      protected override IEnumerable<Item> BuildBankItems(FMODBank bank)
      {
        return bank.mixerVCAs.Select(mixerVCA => new Item(mixerVCA, mixerVCA.GetDisplayName(), snapshotIcon));
      }

      // Draw info about the selected item
      [OnInspectorGUI]
      public void DrawInfoAboutSelectedItem()
      {
        var mixerVCA = GetCurrentSelection().FirstOrDefault();
        if (mixerVCA != null)
        {
          EditorGUILayout.LabelField(new GUIContent(mixerVCA.path), SirenixGUIStyles.BoldLabel);

          GUILayout.Space(4.0f);

          using (new EditorGUIUtility.IconSizeScope(new Vector2(12, 12)))
          {
            if (GUILayout.Button(new GUIContent(" Show in FMOD Studio", FMODEditorGUI.studioIcon), SirenixGUIStyles.MiniButton))
              EditorUtils.SendScriptCommand($"studio.window.navigateTo(studio.project.lookup(\"{mixerVCA.guid}\"))");
            GUILayout.Space(2.0f);
          }
        }
      }
    }
  }
}