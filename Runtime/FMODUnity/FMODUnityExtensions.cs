using FMODUnity;
using UnityEngine;

namespace Audune.Audio
{
  // Class that defines extension methods for FMOD managed by Unity
  public static class FMODUnityExtensions
  {
    #region Creating event instances
    // Create an event instance from the reference
    public static FMODEventInstance CreateInstance(this EventReference reference)
    {
      return FMODStudio.GetEvent(reference).CreateInstance();
    }


    // Create an event instance from the description with an attached transform
    public static FMODEventInstance CreateInstance(this FMODEventDescription description, Transform transform)
    {
      var instance = description.CreateInstance();

      if (transform != null)
      {
        if (transform.TryGetComponent<Rigidbody>(out var rigidbody))
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody);
        else if (transform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody2D);
        else
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform);
      }

      return instance;
    }

    // Create an event instance from the reference with an attached transform
    public static FMODEventInstance CreateInstance(this EventReference reference, Transform transform)
    {
      return FMODStudio.GetEvent(reference).CreateInstance(transform);
    }

    // Create an audio table event instance from the description with an attached transform
    public static FMODEventInstance CreateAudioTableInstance(this FMODEventDescription description, string key, Transform transform)
    {
      var instance = description.CreateAudioTableInstance(key);

      if (transform != null)
      {
        if (transform.TryGetComponent<Rigidbody>(out var rigidbody))
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody);
        else if (transform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody2D);
        else
          RuntimeManager.AttachInstanceToGameObject(instance.native, transform);
      }

      return instance;
    }

    // Create an audio table event instance from the reference with an attached transform
    public static FMODEventInstance CreateAudioTableInstance(this EventReference reference, string key, Transform transform)
    {
      return FMODStudio.GetEvent(reference).CreateAudioTableInstance(key, transform);
    }
    #endregion

    #region Starting event instances
    // Create an event instance from the description and start it
    public static FMODEventInstance StartInstance(this FMODEventDescription description, Transform transform = null)
    {
      var instance = description.CreateInstance(transform);
      instance.Start();
      return instance;
    }

    // Create an event instance from the description and start it
    public static FMODEventInstance StartInstance(this EventReference reference, Transform transform = null)
    {
      return FMODStudio.GetEvent(reference).StartInstance(transform);
    }

    // Create an audio table event instance from the description and start it
    public static FMODEventInstance StartAudioTableInstance(this FMODEventDescription description, string key, Transform transform = null)
    {
      var instance = description.CreateAudioTableInstance(key, transform);
      instance.Start();
      return instance;
    }

    // Create an audio table event instance from the description and start it
    public static FMODEventInstance StartAudioTableInstance(this EventReference reference, string key, Transform transform = null)
    {
      return FMODStudio.GetEvent(reference).StartAudioTableInstance(key, transform);
    }
    #endregion

    #region Starting one shot event instances
    // Create an event instance from the description, start it, and release it immediately
    public static void StartOneShotInstance(this FMODEventDescription description, Transform transform = null)
    {
      var instance = description.CreateInstance(transform);
      instance.Start();
      instance.Dispose();
    }

    // Create an event instance from the description, start it, and release it immediately
    public static void StartOneShotInstance(this EventReference reference, Transform transform = null)
    {
      FMODStudio.GetEvent(reference).StartOneShotInstance(transform);
    }

    // Create an audio table event instance from the description, start it, and release it immediately
    public static void StartOneShotAudioTableInstance(this FMODEventDescription description, string key, Transform transform = null)
    {
      var instance = description.CreateAudioTableInstance(key, transform);
      instance.Start();
      instance.Dispose();
    }

    // Create an audio table event instance from the description, start it, and release it immediately
    public static void StartOneShotAudioTableInstance(this EventReference reference, string key, Transform transform = null)
    {
      FMODStudio.GetEvent(reference).StartOneShotAudioTableInstance(key, transform);
    }
    #endregion

    #region Stopping all instances
    // Stop all instances of the description
    public static void StopAllInstances(this FMODEventDescription description)
    {
      // Stop and release all instances of the description
      foreach (var instance in description.instances)
      {
        instance.Stop();
        instance.Dispose();
      }
    }

    // Stop all instances of the description
    public static void StopAllInstances(this EventReference reference)
    {
      FMODStudio.GetEvent(reference).StopAllInstances();
    }
    #endregion
  }
}