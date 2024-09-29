using System;
using FMODUnity;

namespace Audune.Audio
{
  // Class that defines extension methods for FMOD managed by Unity
  public static class FMODUnityExtensions
  {
    #region Creating event instances
    // Create an event instance from the description with the specified spatial state
    public static FMODEventInstance CreateInstance(this FMODEventDescription description, StateVector vector = null)
    {
      if (description == null)
        throw new ArgumentNullException(nameof(description));

      var instance = description.CreateInstance();
      vector?.ApplyTo(instance);
      return instance;
    }

    // Create an audio table event instance from the description with the specified spatial state
    public static FMODEventInstance CreateAudioTableInstance(this FMODEventDescription description, string key, StateVector vector = null)
    {
      if (description == null)
        throw new ArgumentNullException(nameof(description));

      var instance = description.CreateAudioTableInstance(key);
      vector?.ApplyTo(instance);
      return instance;
    }


    // Create an event instance from the reference with the specified spatial state
    public static FMODEventInstance CreateInstance(this EventReference reference, StateVector vector = null)
    {
      return FMODStudio.GetEvent(reference).CreateInstance(vector);
    }

    // Create an audio table event instance from the reference  with the specified spatial state
    public static FMODEventInstance CreateAudioTableInstance(this EventReference reference, string key, StateVector vector = null)
    {
      return FMODStudio.GetEvent(reference).CreateAudioTableInstance(key, vector);
    }
    #endregion

    #region Starting event instances
    // Create an event instance from the description and start it
    public static FMODEventInstance StartInstance(this FMODEventDescription description, StateVector vector = null)
    {
      var instance = description.CreateInstance(vector);
      instance.Start();
      return instance;
    }

    // Create an audio table event instance from the description and start it
    public static FMODEventInstance StartAudioTableInstance(this FMODEventDescription description, string key, StateVector vector = null)
    {
      var instance = description.CreateAudioTableInstance(key, vector);
      instance.Start();
      return instance;
    }


    // Create an event instance from the description and start it
    public static FMODEventInstance StartInstance(this EventReference reference, StateVector vector = null)
    {
      return FMODStudio.GetEvent(reference).StartInstance(vector);
    }

    // Create an audio table event instance from the description and start it
    public static FMODEventInstance StartAudioTableInstance(this EventReference reference, string key, StateVector vector = null)
    {
      return FMODStudio.GetEvent(reference).StartAudioTableInstance(key, vector);
    }
    #endregion

    #region Starting one shot event instances
    // Create an event instance from the description, start it, and release it immediately
    public static void StartOneShotInstance(this FMODEventDescription description, StateVector vector = null)
    {
      var instance = description.CreateInstance(vector);
      instance.Start();
      instance.Dispose();
    }

    // Create an audio table event instance from the description with an attached transform, start it, and release it immediately
    public static void StartOneShotAudioTableInstance(this FMODEventDescription description, string key, StateVector vector = null)
    {
      var instance = description.CreateAudioTableInstance(key, vector);
      instance.Start();
      instance.Dispose();
    }


    // Create an event instance from the description, start it, and release it immediately
    public static void StartOneShotInstance(this EventReference reference, StateVector vector = null)
    {
      FMODStudio.GetEvent(reference).StartOneShotInstance(vector);
    }

    // Create an audio table event instance from the description with an attached transform, start it, and release it immediately
    public static void StartOneShotAudioTableInstance(this EventReference reference, string key, StateVector vector = null)
    {
      FMODStudio.GetEvent(reference).StartOneShotAudioTableInstance(key, vector);
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