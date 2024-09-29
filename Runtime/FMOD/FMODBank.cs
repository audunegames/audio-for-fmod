using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Audio
{
  // Class that represents a bank that contains metadata and audio sample data required for runtime mixing and playback
  // Listed at https://fmod.com/docs/2.02/api/studio-api-bank.html
  public sealed class FMODBank : FMODStudioSystemComponent, IEquatable<FMODBank>
  {
    // The native handle of the description
    private readonly FMOD.Studio.Bank _nativeBank;


    #region Constructors
    // Dictionary of all banks
    private static readonly Dictionary<IntPtr, FMODBank> _instances = new Dictionary<IntPtr, FMODBank>();


    // Create a new wrapper or get a cached one
    internal static FMODBank Of(FMODStudioSystem system, FMOD.Studio.Bank nativeBank)
    {
      if (!nativeBank.isValid())
        return null;

      if (_instances.TryGetValue(nativeBank.handle, out var instance))
        return instance;

      instance = new FMODBank(system, nativeBank);
      _instances.Add(nativeBank.handle, instance);
      return instance;
    }

    // Constructor from a native bank
    private FMODBank(FMODStudioSystem system, FMOD.Studio.Bank nativeBank) : base(system)
    {
      if (!nativeBank.isValid())
        throw new ArgumentException(nameof(nativeBank), "The specified bank is not valid");

      _nativeBank = nativeBank;
    }
    #endregion

    #region Properties
    // Return the native handle of the bank
    internal FMOD.Studio.Bank native => _nativeBank;


    // Return the unique identifier of the bank
    public FMOD.GUID guid {
      get {
        _nativeBank.getID(out var id).Check();
        return id;
      }
    }

    // Return the path of the bank
    public string path {
      get {
        _nativeBank.getPath(out var path).Check();
        return path;
      }
    }

    // Return the loading state of the bank
    public FMOD.Studio.LOADING_STATE loadingState {
      get {
        _nativeBank.getLoadingState(out var state).Check();
        return state;
      }
    }

    // Return the sample loading state of the bank
    public FMOD.Studio.LOADING_STATE sampleLoadingState {
      get {
        _nativeBank.getSampleLoadingState(out var state).Check();
        return state;
      }
    }

    // Return the event descriptions of the bank
    public IEnumerable<FMODEventDescription> events {
      get {
        _nativeBank.getEventList(out var array).Check();
        return array.Select(eventDescription => FMODEventDescription.Of(_system, eventDescription));
      }
    }

    // Return the buses of the bank
    public IEnumerable<FMODMixerBus> mixerBuses {
      get {
        _nativeBank.getBusList(out var array).Check();
        return array.Select(bus => FMODMixerBus.Of(_system, bus));
      }
    }

    // Return the mixer VCAs of the bank
    public IEnumerable<FMODMixerVCA> mixerVCAs {
      get {
        _nativeBank.getVCAList(out var array).Check();
        return array.Select(vca => FMODMixerVCA.Of(_system, vca));
      }
    }

    // Return the strings of the bank
    public IDictionary<FMOD.GUID, string> strings {
      get {
        _nativeBank.getStringCount(out var count).Check();

        var dictionary = new Dictionary<FMOD.GUID, string>();
        for (var i = 0; i < count; i ++)
        {
          _nativeBank.getStringInfo(i, out var guid, out var path);
          dictionary[guid] = path;
        }

        return dictionary;
      }
    }
    #endregion

    #region Methods
    // Load the sample data of the bank
    public void LoadSampleData()
    {
      _nativeBank.loadSampleData().Check();
    }

    // Unload the sample data of the bank
    public void UnloadSampleData()
    {
      _nativeBank.unloadSampleData().Check();
    }

    // Unload the bank
    public void Unload()
    {
      _nativeBank.unload().Check();
    }
    #endregion

    #region Equatable implementation
    // Return if the bank equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODBank);
    }

    // Return if the bank equals another bank
    public bool Equals(FMODBank other)
    {
      return other is not null && _nativeBank.handle.Equals(other._nativeBank.handle);
    }

    // Return the hash code of the bank
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeBank.handle);
    }
    #endregion
  }
}
