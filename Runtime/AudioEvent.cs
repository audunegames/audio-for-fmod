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
    // Dictionary of global audio event instances
    private readonly Dictionary<AudioEvent, FMODEventInstance> _globalInstances = new Dictionary<AudioEvent, FMODEventInstance>();


    // Audio event emitter settings
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


    // Play the audio event
    public void Play(StateVector vector = null)
    {
      // Start a one shot instance for the audio event
      StartOneShotInstance(vector);
    }

    // Play the audio event and return the instance
    public FMODEventInstance PlayInstance(StateVector vector = null)
    {
      // Start a instance for the audio event
      var instance = CreateInstance(vector);
      instance.Start();
      return instance;
    }

    // Stop all instances of the audio event
    public void StopAll()
    {
      // Stop all instances of the audio event
      _event.StopAllInstances();
    }

    // Return if the audio event has a global instance
    public bool TryGetGlobalInstance(out FMODEventInstance globalInstance)
    {
      // Return the existing global instance if any
      return _globalInstances.TryGetValue(this, out globalInstance);
    }

    // Play the audio event on its global instance
    public void PlayGlobalInstance()
    {
      // Return the existing global instance if any
      if (!TryGetGlobalInstance(out var globalInstance))
      {
        // Create a new global instance and store it
        globalInstance = CreateInstance();
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


    #region Creating and starting instances
    // Create an instance for the audio event
    private FMODEventInstance CreateInstance(StateVector vector = null)
    {
      // If the type is none or the event is null, then do nothing
      if (_event.IsNull)
        return null;

      // Check the type of the event and create an instance
      if (_type == AudioEventType.OneShot)
        return _event.CreateInstance(vector);
      else if (_type == AudioEventType.OneShotAudioTable && TryPickTableKey(out var key))
        return _event.CreateAudioTableInstance(key, vector);
      else
        throw new InvalidOperationException($"{_type} is not a valid audio event type");
    }

    // Start a one shot instance for the audio event
    private void StartOneShotInstance(StateVector vector = null)
    {
      // If the type is none or the event is null, then do nothing
      if (_event.IsNull)
        return;

      // Check the type of the event and start a one shot instance
      if (_type == AudioEventType.OneShot)
        _event.StartOneShotInstance(vector);
      else if (_type == AudioEventType.OneShotAudioTable && TryPickTableKey(out var key))
        _event.StartOneShotAudioTableInstance(key, vector);
      else
        throw new InvalidOperationException($"{_type} is not a valid audio event type");
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
  }
}