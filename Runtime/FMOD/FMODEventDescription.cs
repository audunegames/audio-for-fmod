using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Audio
{
  // Class that represents an event description in a studio system
  // Listed at https://fmod.com/docs/2.02/api/studio-api-eventdescription.html
  public sealed class FMODEventDescription : FMODStudioSystemComponent, IEquatable<FMODEventDescription>
  {
    // The native handle of the description
    private readonly FMOD.Studio.EventDescription _nativeEventDescription;


    #region Constructors
    // Dictionary of all descriptions
    private static readonly Dictionary<IntPtr, FMODEventDescription> _instances = new Dictionary<IntPtr, FMODEventDescription>();


    // Create a new wrapper or get a cached one
    internal static FMODEventDescription Of(FMODStudioSystem system, FMOD.Studio.EventDescription nativeEventDescription)
    {
      if (!nativeEventDescription.isValid())
        return null;

      if (_instances.TryGetValue(nativeEventDescription.handle, out var instance))
        return instance;

      instance = new FMODEventDescription(system, nativeEventDescription);
      _instances.Add(nativeEventDescription.handle, instance);
      return instance;
    }

    // Constructor from a native event description
    private FMODEventDescription(FMODStudioSystem system, FMOD.Studio.EventDescription nativeEventDescription) : base(system)
    {
      if (!nativeEventDescription.isValid())
        throw new ArgumentException(nameof(nativeEventDescription), "The specified event description is not valid");

      _nativeEventDescription = nativeEventDescription;
    }
    #endregion

    #region Properties
    // Return the native handle of the description
    internal FMOD.Studio.EventDescription native => _nativeEventDescription;


    // Return the unique identifier of the description
    public FMOD.GUID guid {
      get {
        _nativeEventDescription.getID(out var id).Check();
        return id;
      }
    }

    // Return the path of the description
    public string path {
      get {
        _nativeEventDescription.getPath(out var path).Check();
        return path;
      }
    }

    // Return the event instances of the description
    public IEnumerable<FMODEventInstance> instances {
      get {
        _nativeEventDescription.getInstanceList(out var array).Check();
        return array.Select(eventInstance => FMODEventInstance.Of(_system, eventInstance));
      }
    }

    // Return the sample data loading state of the description
    public FMOD.Studio.LOADING_STATE sampleLoadingState {
      get {
        _nativeEventDescription.getSampleLoadingState(out var state).Check();
        return state;
      }
    }

    // Return the 3D status of the description
    public bool is3D {
      get {
        _nativeEventDescription.is3D(out var state).Check();
        return state;
      }
    }

    // Return the doppler status of the description
    public bool isDopplerEnabled {
      get {
        _nativeEventDescription.isDopplerEnabled(out var state).Check();
        return state;
      }
    }

    // Return the oneshot status of the description
    public bool isOneshot {
      get {
        _nativeEventDescription.isOneshot(out var state).Check();
        return state;
      }
    }

    // Return the snapshot status of the description
    public bool isSnapshot {
      get {
        _nativeEventDescription.isOneshot(out var state).Check();
        return state;
      }
    }

    // Return the stream status of the description
    public bool isStream {
      get {
        _nativeEventDescription.isStream(out var state).Check();
        return state;
      }
    }

    // Return whether the description has any sustain points
    public bool hasSustainPoint {
      get {
        _nativeEventDescription.hasSustainPoint(out var state).Check();
        return state;
      }
    }

    // Return the minimum distance of the description for 3D attenuation
    public float spatialMinDistance {
      get {
        _nativeEventDescription.getMinMaxDistance(out var min, out var _).Check();
        return min;
      }
    }

    // Return the maximum distance of the description for 3D attenuation
    public float spatialMaxDistance {
      get {
        _nativeEventDescription.getMinMaxDistance(out var _, out var max).Check();
        return max;
      }
    }

    // Return the sound size of the description for 3D panning
    public float spatialSoundSize {
      get {
        _nativeEventDescription.getSoundSize(out var size).Check();
        return size;
      }
    }

    // Return the length of the timeline of the description
    public int timelineLength {
      get {
        _nativeEventDescription.getLength(out var length).Check();
        return length;
      }
    }
    #endregion

    #region Methods
    // Create an event instance from the description
    public FMODEventInstance CreateInstance()
    {
      _nativeEventDescription.createInstance(out var instance).Check();
      return FMODEventInstance.Of(_system, instance);
    }

    // Create an audio table event instance from the description
    public FMODEventInstance CreateAudioTableInstance(string key)
    {
      var instance = CreateInstance();

      instance.onProgrammerSoundCreated += (ref FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES programmerSound) => instance.AudioTableInstanceCreatedCallback(ref programmerSound, key);
      instance.onProgrammerSoundDestroyed += (ref FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES programmerSound) => instance.AudioTableInstanceDestroyedCallback(ref programmerSound);

      return instance;
    }

    // Release all instances of the description
    public void ReleaseAllInstances()
    {
      _nativeEventDescription.releaseAllInstances().Check();
    }

    // Load the sample data of the description
    public void LoadSampleData()
    {
      _nativeEventDescription.loadSampleData().Check();
    }

    // Unload the sample data of the description
    public void UnloadSampleData()
    {
      _nativeEventDescription.unloadSampleData().Check();
    }

    // Return a parameter description of the description by its name
    public FMODParameterDescription GetParameterDescription(string name)
    {
      _nativeEventDescription.getParameterDescriptionByName(name, out var parameter).Check();
      return new FMODParameterDescription(_system, parameter);
    }

    // Return a parameter description of the description by unique its identifier
    public FMODParameterDescription GetParameterDescription(FMOD.Studio.PARAMETER_ID id)
    {
      _nativeEventDescription.getParameterDescriptionByID(id, out var parameter).Check();
      return new FMODParameterDescription(_system, parameter);
    }

    // Return a parameter description of the description by its index
    public FMODParameterDescription GetParameterDescription(int index)
    {
      _nativeEventDescription.getParameterDescriptionByIndex(index, out var parameter).Check();
      return new FMODParameterDescription(_system, parameter);
    }

    // Return a parameter label of the description by its name
    public string GetParameterLabel(string name, int labelIndex)
    {
      _nativeEventDescription.getParameterLabelByName(name, labelIndex, out var label).Check();
      return label;
    }

    // Return a parameter label of the description by its unique identifier
    public string GetParameterLabel(FMOD.Studio.PARAMETER_ID id, int labelIndex)
    {
      _nativeEventDescription.getParameterLabelByID(id, labelIndex, out var label).Check();
      return label;
    }

    // Return a parameter label of the description by its index
    public string GetParameterLabel(int index, int labelIndex)
    {
      _nativeEventDescription.getParameterLabelByIndex(index, labelIndex, out var label).Check();
      return label;
    }
    #endregion

    #region Equatable implementation
    // Return if the description equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODEventDescription);
    }

    // Return if the description equals another description
    public bool Equals(FMODEventDescription other)
    {
      return other is not null && _nativeEventDescription.handle.Equals(other._nativeEventDescription.handle);
    }

    // Return the hash code of the description
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeEventDescription.handle);
    }
    #endregion

    #region Implicit operators
    // Convert an event description to an event reference
    public static implicit operator FMODUnity.EventReference(FMODEventDescription description)
    {
#if UNITY_EDITOR
      return new FMODUnity.EventReference { Guid = description.guid, Path = description.path };
#else
      return new FMODUnity.EventReference { Guid = description.guid };
#endif
    }
    #endregion
  }
}
