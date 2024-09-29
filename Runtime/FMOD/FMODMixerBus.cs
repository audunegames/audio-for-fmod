using System;
using System.Collections.Generic;

namespace Audune.Audio
{
  // Class that represents a global mixer bus in a studio system
  // Listed at https://fmod.com/docs/2.02/api/studio-api-bus.html
  public sealed class FMODMixerBus : FMODStudioSystemComponent, IEquatable<FMODMixerBus>
  {
    // The native handle of the bus
    private readonly FMOD.Studio.Bus _nativeBus;


    #region Constructors
    // Dictionary of all VCAs
    private static readonly Dictionary<IntPtr, FMODMixerBus> _instances = new Dictionary<IntPtr, FMODMixerBus>();


    // Create a new wrapper or get a cached one
    internal static FMODMixerBus Of(FMODStudioSystem system, FMOD.Studio.Bus nativeBus)
    {
      if (!nativeBus.isValid())
        return null;

      if (_instances.TryGetValue(nativeBus.handle, out var instance))
        return instance;

      instance = new FMODMixerBus(system, nativeBus);
      _instances.Add(nativeBus.handle, instance);
      return instance;
    }

    // Constructor from a native bus
    private FMODMixerBus(FMODStudioSystem system, FMOD.Studio.Bus nativeBus) : base(system)
    {
      if (!nativeBus.isValid())
        throw new ArgumentException(nameof(nativeBus), "The specified bus is not valid");

      _nativeBus = nativeBus;
    }
    #endregion

    #region Properties
    // Return the native handle of the bus
    internal FMOD.Studio.Bus native => _nativeBus;


    // Return the unique identifier of the bus
    public FMOD.GUID guid {
      get {
        _nativeBus.getID(out var id).Check();
        return id;
      }
    }

    // Return the path of the bus
    public string path {
      get {
        _nativeBus.getPath(out var path).Check();
        return path;
      }
    }

    // Return the channel group of the bus
    public FMOD.ChannelGroup channelGroup {
      get {
        _nativeBus.getChannelGroup(out var channelGroup).Check();
        return channelGroup;
      }
    }

    // Return and set the paused state of the bus
    public bool paused {
      get {
        _nativeBus.getPaused(out var paused).Check();
        return paused;
      }
      set {
        _nativeBus.setPaused(value).Check();
      }
    }

    // Return and set the volume of the bus
    public float volume {
      get {
        _nativeBus.getVolume(out var volume, out var _).Check();
        return volume;
      }
      set {
        _nativeBus.setVolume(value).Check();
      }
    }

    // Return the final volume of the bus
    public float finalVolume {
      get {
        _nativeBus.getVolume(out var _, out var finalVolume).Check();
        return finalVolume;
      }
    }

    // Return and set the mute state of the bus
    public bool mute {
      get {
        _nativeBus.getMute(out var mute).Check();
        return mute;
      }
      set {
        _nativeBus.setMute(value).Check();
      }
    }

    // Return and set the port index of the bus
    public ulong portIndex {
      get {
        _nativeBus.getPortIndex(out var index).Check();
        return index;
      }
      set {
        _nativeBus.setPortIndex(value).Check();
      }
    }
    #endregion

    #region Methods
    // Stop all event instances that are routed into the bus
    public void StopAllEventInstances(bool allowFadeout = true)
    {
      _nativeBus.stopAllEvents(allowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE).Check();
    }

    // Lock the channel group of the bus
    public void LockChannelGroup()
    {
      _nativeBus.lockChannelGroup().Check();
    }

    // Unlock the channel group of the bus
    public void UnlockChannelGroup()
    {
      _nativeBus.unlockChannelGroup().Check();
    }
    #endregion

    #region Equatable implementation
    // Return if the bus equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODMixerBus);
    }

    // Return if the bus equals another event instance
    public bool Equals(FMODMixerBus other)
    {
      return other is not null && _nativeBus.handle.Equals(other._nativeBus.handle);
    }

    // Return the hash code of the event instance
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeBus.handle);
    }
    #endregion
  }
}
