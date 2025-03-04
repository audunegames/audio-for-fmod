using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Audio
{
  // Class that defines an audio event
  [Serializable]
  public sealed class AudioEvent : IEquatable<AudioEvent>
  {
    // Delegate that defines a callback that is invoked when an audio event instance is created
    public delegate void InstanceCreatedCallback(FMODEventInstance instance);


    // Dictionary of global audio event instances
    private readonly Dictionary<AudioEvent, FMODEventInstance> _globalInstances = new Dictionary<AudioEvent, FMODEventInstance>();


    // Audio event settings
    [SerializeField, Tooltip("The audio event to play")]
    private EventReference _event;

    [SerializeField, Tooltip("The type of the audio event")]
    private AudioEventType _type = AudioEventType.OneShot;

    [SerializeField, Tooltip("The mode of choosing an audio table key")]
    private AudioTableMode _audioTableMode = AudioTableMode.Shuffle;

    [SerializeField, Tooltip("The audio table keys of which to pick one")]
    private List<string> _audioTableKeys;


    // Internal state of the audio event emitter
    private int _lastPickedIndex = -1;
    

    #region Playing and stopping instances
    // Play the audio event
    public void Play(StateVector vector = null, params InstanceCreatedCallback[] callbacks)
    {
      // Create the instance for the audio event and start it
      using var instance = CreateInstance(vector, callbacks);
      instance.Start();
    }

    // Stop all instances of the audio event
    public void StopAll()
    {
      // Stop all instances of the audio event
      _event.StopAllInstances();
    }
    #endregion

    #region Playing and stopping global instances
    // Return if the audio event has a global instance
    public bool TryGetGlobalInstance(out FMODEventInstance globalInstance)
    {
      // Return the existing global instance if any
      return _globalInstances.TryGetValue(this, out globalInstance);
    }

    // Play the audio event on its global instance
    public void PlayGlobalInstance(params InstanceCreatedCallback[] callbacks)
    {
      // Return the existing global instance if any
      if (!TryGetGlobalInstance(out var globalInstance))
      {
        // Create a new global instance and store it
        globalInstance = CreateInstance(null, callbacks);
        if (globalInstance != null)
          _globalInstances.Add(this, globalInstance);
      }

      // Start the global instance for the audio event
      globalInstance?.Start();
    }

    // Pause the audio event on its global instance
    public void PauseGlobalInstance(bool isPaused)
    {
      // Start the global instance for the audio event
      if (TryGetGlobalInstance(out var globalInstance))
        globalInstance.paused = isPaused;
    }

    // Stop the audio event on its global instance
    public void StopGlobalInstance()
    {
      // Start the global instance for the audio event
      if (TryGetGlobalInstance(out var globalInstance))
      {
        globalInstance.Stop();
        globalInstance.Dispose();
      }
    }
    #endregion

    #region Creating instances
    // Create an instance for the audio event
    private FMODEventInstance CreateInstance(StateVector vector = null, params InstanceCreatedCallback[] callbacks)
    {
      // If the type is none or the event is null, then do nothing
      if (_event.IsNull)
        return null;

      // Check the type of the event and create an instance
      FMODEventInstance instance;
      if (_type == AudioEventType.OneShot)
        instance = _event.CreateInstance(vector);
      else if (_type == AudioEventType.OneShotAudioTable && TryPickTableKey(out var key))
        instance = _event.CreateAudioTableInstance(key, vector);
      else
        throw new InvalidOperationException($"{_type} is not a valid audio event type");

      // Invoke the callbacks on the instance
      foreach (var callback in callbacks)
        callback(instance);

      // Return the instance
      return instance;
    }
    #endregion

    #region Picking audio table keys
    // Pick a key from the audio table
    private bool TryPickTableKey(out string key)
    {
      key = null;
      if (_audioTableKeys == null || _audioTableKeys.Count == 0)
        return false;

      if (_audioTableMode == AudioTableMode.Sequential)
      {
        var index = _lastPickedIndex > -1 ? ((_lastPickedIndex + 1) % _audioTableKeys.Count) : 0;
        _lastPickedIndex = index;
        key = _audioTableKeys[index];
      }
      else if (_audioTableMode == AudioTableMode.Shuffle)
      {
        var index = UnityEngine.Random.Range(0, _audioTableKeys.Count);
        while (index == _lastPickedIndex)
          index = UnityEngine.Random.Range(0, _audioTableKeys.Count);
        _lastPickedIndex = index;
        key = _audioTableKeys[index];
      }
      else
      {
        key = _audioTableKeys[UnityEngine.Random.Range(0, _audioTableKeys.Count)];
      }

      return !string.IsNullOrEmpty(key);
    }
    #endregion

    #region Equatable implementation
    // Return if the audio event equals another object
    public override bool Equals(object obj)
    {
      return obj is AudioEvent other && Equals(other);
    }

    // Return if the audio event equals another event
    public bool Equals(AudioEvent other)
    {
      return _event.Guid == other._event.Guid;
    }

    // Return the hash code of the audio event
    public override int GetHashCode()
    {
      return HashCode.Combine(_event.Guid);
    }
    #endregion

    #region Returning callbacks
    // Set the pitch to the specified value
    public static InstanceCreatedCallback Pitch(float value)
      => instance => instance.pitch = value;

    // Set the volume to the specified value
    public static InstanceCreatedCallback Volume(float value)
      => instance => instance.volume = value;

    // Set the parameter with the specified name to the specified value
    public static InstanceCreatedCallback Parameter(string name, float value, bool ignoreSeekSpeed = false)
      => instance => instance.SetParameter(name, value, ignoreSeekSpeed);

    // Set the parameter with the specified name to the specified value
    public static InstanceCreatedCallback Parameter(string name, string label, bool ignoreSeekSpeed = false)
      => instance => instance.SetParameter(name, label, ignoreSeekSpeed);
    #endregion
  }
}