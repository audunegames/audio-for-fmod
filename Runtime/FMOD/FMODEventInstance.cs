using System;
using System.Collections.Generic;

namespace Audune.Audio
{
  // Class that represents an instance in a studio system
  // Listed at https://fmod.com/docs/2.02/api/studio-api-eventdescription.html
  public sealed class FMODEventInstance : FMODStudioSystemComponent, IDisposable, IEquatable<FMODEventInstance>
  {
    // Dictionary of all instances
    private static readonly Dictionary<IntPtr, FMODEventInstance> _instances = new Dictionary<IntPtr, FMODEventInstance>();


    // The native handle of the instance
    private readonly FMOD.Studio.EventInstance _nativeEventInstance;

    // The audio table sound used by the instance
    private FMOD.Sound? _nativeAudioTableSound = null;


    // Event handler when an instance has been created
    public event FMODCallback OnCreated;

    // Event handler when an instance is about to be destroyed
    public event FMODCallback OnDestroyed;

    // Event handler when start has been called on an instance which was not already playing
    public event FMODCallback OnStarting;

    // Event handler when an instance has started
    public event FMODCallback OnStarted;

    // Event handler when start has been called on an instance which was already playing
    public event FMODCallback OnRestarted;

    // Event handler when an instance has stopped
    public event FMODCallback OnStopped;

    // Event handler when start has been called but the polyphony settings did not allow the event to start
    public event FMODCallback OnStartFailed;

    // Event handler when an programmer sound is about to play
    public event FMODReferenceCallback<FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES> OnProgrammerSoundCreated;

    // Event handler when a programmer sound and has stopped playing
    public event FMODReferenceCallback<FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES> OnProgrammerSoundDestroyed;

    // Event handler when a DSP plugin instance has been created
    public event FMODReferenceCallback<FMOD.Studio.PLUGIN_INSTANCE_PROPERTIES> OnPluginCreated;

    // Event handler when a DSP plugin instance is about to be destroyed
    public event FMODReferenceCallback<FMOD.Studio.PLUGIN_INSTANCE_PROPERTIES> OnPluginDestroyed;

    // Event handler when the timeline passes a named marker
    public event FMODReferenceCallback<FMOD.Studio.TIMELINE_MARKER_PROPERTIES> OnTimelineMarker;

    // Event handler when the timeline hits a beat in a tempo section
    public event FMODReferenceCallback<FMOD.Studio.TIMELINE_BEAT_PROPERTIES> OnTimelineBeat;

    // Event handler when the instance plays a sound
    public event FMODReferenceCallback<FMOD.Sound> OnSoundPlayed;

    // Event handler when the instance finishes playing a sound
    public event FMODReferenceCallback<FMOD.Sound> OnSoundStopped;

    // Event handler when an instance becomes virtual
    public event FMODCallback OnBecameVirtual;

    // Event handler when an instance becomes real
    public event FMODCallback OnBecameReal;

    // Event handler when an instance is started by a start event command
    public event FMODCallback<FMODEventInstance> OnEventCommandStarted;

    // Event handler when the timeline hits a beat in a tempo section of a nested event
    public event FMODReferenceCallback<FMOD.Studio.TIMELINE_NESTED_BEAT_PROPERTIES> OnNestedTimelineBeat;


    // Create a new wrapper or get a cached one
    internal static FMODEventInstance Of(FMODStudioSystem system, FMOD.Studio.EventInstance nativeEventInstance)
    {
      if (!nativeEventInstance.isValid())
        return null;

      if (_instances.TryGetValue(nativeEventInstance.handle, out var instance))
        return instance;
      
      instance = new FMODEventInstance(system, nativeEventInstance);
      _instances.Add(nativeEventInstance.handle, instance);
      return instance;
    }

    // Constructor from a native event instance
    private FMODEventInstance(FMODStudioSystem system, FMOD.Studio.EventInstance nativeEventInstance) : base(system)
    {
      if (!nativeEventInstance.isValid())
        throw new ArgumentException(nameof(nativeEventInstance), "The specified instance is not valid");

      _nativeEventInstance = nativeEventInstance;
      _nativeEventInstance.setCallback(new FMOD.Studio.EVENT_CALLBACK(CallbackHandler), FMOD.Studio.EVENT_CALLBACK_TYPE.ALL).Check();
    }

    // Destructor
    public void Dispose()
    {
      _nativeEventInstance.release().Check();
    }


    // Return if the instance equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODEventInstance);
    }

    // Return if the instance equals another instance
    public bool Equals(FMODEventInstance other)
    {
      return other is not null && _nativeEventInstance.handle.Equals(other._nativeEventInstance.handle);
    }

    // Return the hash code of the instance
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeEventInstance.handle);
    }


    // Return the native handle of the instance
    internal FMOD.Studio.EventInstance native => _nativeEventInstance;

    // Return if the instance is valid
    public bool isValid => _nativeEventInstance.isValid();


    // Return the event description of the instance
    public FMODEventDescription description {
      get {
        _nativeEventInstance.getDescription(out var eventDescription).Check();
        return FMODEventDescription.Of(_system, eventDescription);
      }
    }

    // Return the channel group of the instance
    public FMOD.ChannelGroup channelGroup {
      get {
        _nativeEventInstance.getChannelGroup(out var channelGroup).Check();
        return channelGroup;
      }
    }

    // Return the playback state of the instance
    public FMOD.Studio.PLAYBACK_STATE playbackState {
      get {
        _nativeEventInstance.getPlaybackState(out var state).Check();
        return state;
      }
    }

    // Return and set the paused state of the instance
    public bool paused {
      get {
        _nativeEventInstance.getPaused(out var paused).Check();
        return paused;
      }
      set {
        _nativeEventInstance.setPaused(value).Check();
      }
    }

    // Return and set the pitch of the instance
    public float pitch {
      get {
        _nativeEventInstance.getPitch(out var pitch, out var _).Check();
        return pitch;
      }
      set {
        _nativeEventInstance.setPitch(value).Check();
      }
    }

    // Return the final pitch of the instance
    public float finalPitch {
      get {
        _nativeEventInstance.getPitch(out var _, out var finalPitch).Check();
        return finalPitch;
      }
    }

    // Return and set the volume of the instance
    public float volume {
      get {
        _nativeEventInstance.getVolume(out var volume, out var _).Check();
        return volume;
      }
      set {
        _nativeEventInstance.setVolume(value).Check();
      }
    }

    // Return the final volume of the instance
    public float finalVolume {
      get {
        _nativeEventInstance.getVolume(out var _, out var finalVolume).Check();
        return finalVolume;
      }
    }

    // Return and set the timeline position of the instance
    public int timelinePosition {
      get {
        _nativeEventInstance.getTimelinePosition(out var position).Check();
        return position;
      }
      set {
        _nativeEventInstance.setTimelinePosition(value).Check();
      }
    }

    // Return the virtualization state of the instance
    public bool isVirtual {
      get {
        _nativeEventInstance.isVirtual(out var state).Check();
        return state;
      }
    }

    // Return and set the channel priority of the instance
    public float channelPriority {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.CHANNELPRIORITY, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.CHANNELPRIORITY, value).Check();
      }
    }

    // Return and set the schedule delay in DSP clocks of the instance
    public float scheduleDelay {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.SCHEDULE_DELAY, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.SCHEDULE_DELAY, value).Check();
      }
    }

    // Return and set the schedule look-ahead in DSP clocks of the instance
    public float scheduleLookahead {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, value).Check();
      }
    }

    // Return and set the cooldown of the instance
    public float cooldown {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.COOLDOWN, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.COOLDOWN, value).Check();
      }
    }

    // Return and set the spatial attributes of the instance for 3D attenuation
    public FMODSpatialAttributes spatialAttributes {
      get {
        _nativeEventInstance.get3DAttributes(out var attributes).Check();
        return new FMODSpatialAttributes(attributes);
      }
      set {
        _nativeEventInstance.set3DAttributes(value.native).Check();
      }
    }

    // Return the minimum distance of the instance for 3D attenuation
    public float spatialMinDistance {
      get {
        _nativeEventInstance.getMinMaxDistance(out var min, out var _).Check();
        return min;
      }
    }

    // Return the maximum distance of the instance for 3D attenuation
    public float spatialMaxDistance {
      get {
        _nativeEventInstance.getMinMaxDistance(out var _, out var max).Check();
        return max;
      }
    }

    // Return and set the minimum distance override of the instance for 3D attenuation
    public float spatialMinDistanceOverride {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, value).Check();
      }
    }

    // Return and set the minimum distance override of the instance for 3D attenuation
    public float spatialMaxDistanceOverride {
      get {
        _nativeEventInstance.getProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, out var value).Check();
        return value;
      }
      set {
        _nativeEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, value).Check();
      }
    }

    // Return and set the listener mask of the instance
    public uint listenerMask {
      get {
        _nativeEventInstance.getListenerMask(out var mask).Check();
        return mask;
      }
      set {
        _nativeEventInstance.setListenerMask(value).Check();
      }
    }


    // Start playback of the instance
    public void Start()
    {
      _nativeEventInstance.start().Check();
    }

    // Stop playback of the instance
    public void Stop(bool allowFadeout = true)
    {
      _nativeEventInstance.stop(allowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE).Check();
    }

    // Allow the instance to continue past a sustain point
    public void KeyOff()
    {
      _nativeEventInstance.keyOff().Check();
    }

    // Return a parameter value of the instance by its name
    public float GetParameter(string name)
    {
      _nativeEventInstance.getParameterByName(name, out var value).Check();
      return value;
    }
    public float GetParameter(string name, out float finalValue)
    {
      _nativeEventInstance.getParameterByName(name, out var value, out finalValue).Check();
      return value;
    }

    // Return a parameter value of the instance by its unique identifier
    public float GetParameter(FMOD.Studio.PARAMETER_ID id)
    {
      _nativeEventInstance.getParameterByID(id, out var value).Check();
      return value;
    }
    public float GetParameter(FMOD.Studio.PARAMETER_ID id, out float finalValue)
    {
      _nativeEventInstance.getParameterByID(id, out var value, out finalValue).Check();
      return value;
    }

    // Set a parameter value of the instance by its name
    public void SetParameter(string name, float value, bool ignoreSeekSpeed = false)
    {
      _nativeEventInstance.setParameterByName(name, value, ignoreSeekSpeed).Check();
    }

    // Set a parameter value of the instance by its name, looking up the value label
    public void SetParameter(string name, string label, bool ignoreSeekSpeed = false)
    {
      _nativeEventInstance.setParameterByNameWithLabel(name, label, ignoreSeekSpeed).Check();
    }

    // Set a parameter value of the instance by its unique identifier
    public void SetParameter(FMOD.Studio.PARAMETER_ID id, float value, bool ignoreSeekSpeed = false)
    {
      _nativeEventInstance.setParameterByID(id, value, ignoreSeekSpeed).Check();
    }

    // Set a parameter value of the instance by its unique identifier, looking up the value label
    public void SetParameter(FMOD.Studio.PARAMETER_ID id, string label, bool ignoreSeekSpeed = false)
    {
      _nativeEventInstance.setParameterByIDWithLabel(id, label, ignoreSeekSpeed).Check();
    }

    // Return the reverb send level of the instance
    public float GetReverbLevel(int index)
    {
      _nativeEventInstance.getReverbLevel(index, out var level).Check();
      return level;
    }

    // Set the reverb send level of the instance
    public void SetReverbLevel(int index, float level)
    {
      _nativeEventInstance.setReverbLevel(index, level).Check();
    }


    // Event handler for when an audio table instance has been created
    internal void AudioTableInstanceCreatedCallback(ref FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES programmerSound, string key)
    {
      if (_nativeAudioTableSound.HasValue)
        _nativeAudioTableSound.Value.release().Check();
      _nativeAudioTableSound = _system.CreateAudioTableSound(key, ref programmerSound);
    }

    // Event handler for when an audio table instance has been destroyed
    internal void AudioTableInstanceDestroyedCallback(ref FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES programmerSound)
    {
      if (_nativeAudioTableSound.HasValue)
        _nativeAudioTableSound.Value.release().Check();
      _nativeAudioTableSound = null;
    }


    // Callback handler for the instance
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private static FMOD.RESULT CallbackHandler(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
      if (!_instances.TryGetValue(instancePtr, out var instance))
        return FMOD.RESULT.OK;
      if (!instance._nativeEventInstance.isValid())
        return FMOD.RESULT.OK;

      try
      {
        switch (type)
        {
          case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATED:
            instance.OnCreated?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
            instance.OnDestroyed?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.STARTING:
            instance.OnStarting?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.STARTED:
            instance.OnStarted?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.RESTARTED:
            instance.OnRestarted?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED:
            instance.OnStopped?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.START_FAILED:
            instance.OnStartFailed?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
            instance.OnProgrammerSoundCreated?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
            instance.OnProgrammerSoundDestroyed?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.PLUGIN_CREATED:
            instance.OnPluginCreated?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.PLUGIN_DESTROYED:
            instance.OnPluginDestroyed?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
            instance.OnTimelineMarker?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
            instance.OnTimelineBeat?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.SOUND_PLAYED:
            instance.OnSoundPlayed?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.SOUND_STOPPED:
            instance.OnSoundStopped?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.REAL_TO_VIRTUAL:
            instance.OnBecameVirtual?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.VIRTUAL_TO_REAL:
            instance.OnBecameReal?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.START_EVENT_COMMAND:
            if (_instances.TryGetValue(parameterPtr, out var commandInstance))
              instance.OnEventCommandStarted?.Invoke(commandInstance);
            return FMOD.RESULT.OK;

          case FMOD.Studio.EVENT_CALLBACK_TYPE.NESTED_TIMELINE_BEAT:
            instance.OnNestedTimelineBeat?.WithParameter(parameterPtr).Invoke();
            return FMOD.RESULT.OK;

          default:
            return FMOD.RESULT.OK;
        }
      }
      catch (FMODException ex)
      {
        return ex.Result;
      }
    }
  }
}
