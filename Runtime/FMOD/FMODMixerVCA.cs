using System;
using System.Collections.Generic;

namespace Audune.Audio
{
  // Class that represents a global mixer VCA in a studio system
  // Listed at https://fmod.com/docs/2.02/api/studio-api-vca.html
  public class FMODMixerVCA : FMODStudioSystemComponent, IEquatable<FMODMixerVCA>
  {
    // The native handle of the VCA
    private readonly FMOD.Studio.VCA _nativeVCA;


    #region Constructors
    // Dictionary of all VCAs
    private static readonly Dictionary<IntPtr, FMODMixerVCA> _instances = new Dictionary<IntPtr, FMODMixerVCA>();

    // Create a new wrapper or get a cached one
    internal static FMODMixerVCA Of(FMODStudioSystem system, FMOD.Studio.VCA nativeVCA)
    {
      if (!nativeVCA.isValid())
        return null;

      if (_instances.TryGetValue(nativeVCA.handle, out var instance))
        return instance;

      instance = new FMODMixerVCA(system, nativeVCA);
      _instances.Add(nativeVCA.handle, instance);
      return instance;
    }

    // Constructor from a native VCA
    private FMODMixerVCA(FMODStudioSystem system, FMOD.Studio.VCA nativeVCA) : base(system)
    {
      if (!nativeVCA.isValid())
        throw new ArgumentException(nameof(nativeVCA), "The specified VCA is not valid");

      _nativeVCA = nativeVCA;
    }
    #endregion

    #region Properties
    // Return the native handle of the VCA
    internal FMOD.Studio.VCA native => _nativeVCA;


    // Return the unique identifier of the VCA
    public FMOD.GUID guid {
      get {
        _nativeVCA.getID(out var id).Check();
        return id;
      }
    }

    // Return the path of the VCA
    public string path {
      get {
        _nativeVCA.getPath(out var path).Check();
        return path;
      }
    }

    // Return and set the volume of the VCA
    public float volume {
      get {
        _nativeVCA.getVolume(out var volume, out var _).Check();
        return volume;
      }
      set {
        _nativeVCA.setVolume(value).Check();
      }
    }

    // Return the final volume of the VCA
    public float finalVolume {
      get {
        _nativeVCA.getVolume(out var _, out var finalVolume).Check();
        return finalVolume;
      }
    }
    #endregion

    #region Equatable implementation
    // Return if the VCA equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODMixerVCA);
    }

    // Return if the VCA equals another VCA
    public bool Equals(FMODMixerVCA other)
    {
      return other is not null && _nativeVCA.handle.Equals(other._nativeVCA.handle);
    }

    // Return the hash code of the VCA
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeVCA.handle);
    }
    #endregion
  }
}
