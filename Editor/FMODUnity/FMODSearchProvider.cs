using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Base class that defines a search provider
  internal abstract class FMODSearchProvider<TValue> : ScriptableObject, ISearchWindowProvider
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

    // The title of the search provider
    public string title;

    // The items of the search provider
    public List<Item> items;

    // The callback of the search provider when an entry is selected
    public Action<TValue> onSelectCallback;


    // Create the search tree
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
      var list = new List<SearchTreeEntry>();
      var groups = new List<string>();

      // Add the root and the none value to the tree
      list.Add(new SearchTreeGroupEntry(new GUIContent(title), 0));
      list.Add(new SearchTreeEntry(new GUIContent("None")) { level = 1, userData = default(TValue) });

      // Iterate over the items
      foreach (var item in items.OrderBy(item => item.path))
      {
        var components = item.path.Split("/");

        // Add the groups to the tree
        var group = "";
        for (var i = 0; i < components.Length - 1; i++)
        {
          group += components[i];
          if (!groups.Contains(group))
          {
            list.Add(new SearchTreeGroupEntry(new GUIContent(components[i]), i + 1));
            groups.Add(group);
          }
          group += "/";
        }

        // Add the item to the tree
        list.Add(new SearchTreeEntry(new GUIContent(components[^1], item.icon)) { level = components.Length, userData = item.value });
      }

      return list;
    }

    // Event handler for selecting an entry
    public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
    {
      onSelectCallback?.Invoke((TValue)entry.userData);
      return true;
    }
  }

  // Class that defines a search provider for FMOD studio banks
  internal class FMODBankSearchProvider : FMODSearchProvider<FMODBank>
  {
    // Create a search provider
    public static FMODSearchProvider<FMODBank> Create(IEnumerable<FMODBank> banks, Action<FMODBank> onSelectCallback)
    {
      var provider = CreateInstance<FMODBankSearchProvider>();
      provider.title = "Banks";
      provider.items = banks.Select(bank => new Item(bank, bank.GetDisplayName(), FMODEditorGUIUtils.bankIcon)).ToList();
      provider.onSelectCallback = onSelectCallback;
      return provider;
    }
  }

  // Class that defines a search provider for a reference to an FMOD event description
  internal class FMODEventReferenceSearchProvider : FMODSearchProvider<EventReference>
  {
    // Create a search provider
    public static FMODEventReferenceSearchProvider Create(IEnumerable<FMODBank> banks, Action<EventReference> onSelectCallback)
    {
      var provider = CreateInstance<FMODEventReferenceSearchProvider>();
      provider.title = "Events";
      provider.items = banks.SelectMany(bank => bank.events.Select(description => new Item((EventReference)description, $"{bank.GetDisplayName()}/{description.GetDisplayName()}", FMODEditorGUIUtils.eventIcon))).ToList();
      provider.onSelectCallback = onSelectCallback;
      return provider;
    }
  }

  // Class that defines a search provider for a reference to an FMOD mixer bus
  internal class FMODMixerBusSearchProvider : FMODSearchProvider<FMODMixerBus>
  {
    // Create a search provider
    public static FMODMixerBusSearchProvider Create(IEnumerable<FMODBank> banks, Action<FMODMixerBus> onSelectCallback)
    {
      var provider = CreateInstance<FMODMixerBusSearchProvider>();
      provider.title = "Mixer Buses";
      provider.items = banks.SelectMany(bank => bank.mixerBuses.Select(mixerBus => new Item(mixerBus, $"{bank.GetDisplayName()}/{mixerBus.GetDisplayName()}", FMODEditorGUIUtils.snapshotIcon))).ToList();
      provider.onSelectCallback = onSelectCallback;
      return provider;
    }
  }

  // Class that defines a search provider for a reference to an FMOD mixer VCA
  internal class FMODMixerVCASearchProvider : FMODSearchProvider<FMODMixerVCA>
  {
    // Create a search provider
    public static FMODMixerVCASearchProvider Create(IEnumerable<FMODBank> banks, Action<FMODMixerVCA> onSelectCallback)
    {
      var provider = CreateInstance<FMODMixerVCASearchProvider>();
      provider.title = "Mixer VCAs";
      provider.items = banks.SelectMany(bank => bank.mixerVCAs.Select(mixerVCA => new Item(mixerVCA, $"{bank.GetDisplayName()}/{mixerVCA.GetDisplayName()}", FMODEditorGUIUtils.snapshotIcon))).ToList();
      provider.onSelectCallback = onSelectCallback;
      return provider;
    }
  }
}