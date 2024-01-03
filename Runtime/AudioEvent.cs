using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Audio
{
  // Class that defines an audio event
  [Serializable]
  public sealed class AudioEvent
  {
    // Audio event emitter settings
    [SerializeField, Tooltip("The audio event to play")]
    private EventReference _event;

    [SerializeField, Tooltip("The type of the audio action")]
    private AudioEventType _type = AudioEventType.OneShot;

    [SerializeField, Tooltip("The mode of choosing an audio table key")]
    private AudioTableMode _audioTableMode = AudioTableMode.Shuffle;

    [SerializeField, Tooltip("The audio table keys of which to pick one")]
    private List<string> _audioTableKeys;


    // Internal state of the audio event emitter
    private int _lastPickedIndex = -1;


    // Play the audio event
    public void Play(Transform transform = null)
    {
      // If the type is none or the event is null, then do nothing
      if (_event.IsNull)
        return;

      // Check the type of the event
      if (_type == AudioEventType.OneShot)
        _event.StartOneShotInstance(transform);
      else if (_type == AudioEventType.OneShotAudioTable && TryPickTableKey(out var key))
        _event.StartOneShotAudioTableInstance(key, transform);
    }


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
  }
}