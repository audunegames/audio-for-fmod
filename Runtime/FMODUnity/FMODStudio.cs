using System.Collections.Generic;
using UnityEngine;

namespace Audune.Audio
{
  // Class that provides access to the FMOD Studio system managed by Unity
  public static class FMODStudio
  {
    // Return the studio system
    public static FMODStudioSystem studioSystem {
      get {
#if UNITY_EDITOR
        if (Application.isPlaying)
          return FMODStudioSystem.Of(FMODUnity.RuntimeManager.StudioSystem);

        FMODUnity.EditorUtils.LoadPreviewBanks();
        return FMODStudioSystem.Of(FMODUnity.EditorUtils.System);
#else
        return FMODStudioSystem.Of(FMODUnity.RuntimeManager.StudioSystem);
#endif
      }
    }


    // Return the banks of the system
    public static IEnumerable<FMODBank> banks
      => studioSystem.banks;


    // Return a bank of the system by its path
    public static FMODBank GetBank(string path)
     => studioSystem.GetBank(path);

    // Return a bank of the system by its unique identifier
    public static FMODBank GetBank(FMOD.GUID guid)
      => studioSystem.GetBank(guid);

    // Return an event description of the system by its path
    public static FMODEventDescription GetEvent(string path)
      => studioSystem.GetEvent(path);

    // Return an event description of the system by its unique identifier
    public static FMODEventDescription GetEvent(FMOD.GUID guid)
      => studioSystem.GetEvent(guid);

    // Return an event description of the system from an event reference
    public static FMODEventDescription GetEvent(FMODUnity.EventReference reference)
      => studioSystem.GetEvent(reference);

    // Return a bus of the system by its path
    public static FMODMixerBus GetMixerBus(string path)
      => studioSystem.GetMixerBus(path);

    // Return a bus of the system by its unique identifier
    public static FMODMixerBus GetMixerBus(FMOD.GUID guid)
      => studioSystem.GetMixerBus(guid);

    // Return a VCA of the system by its path
    public static FMODMixerVCA GetMixerVCA(string path)
      => studioSystem.GetMixerVCA(path);

    // Return a VCA of the system by its unique identifier
    public static FMODMixerVCA GetMixerVCA(FMOD.GUID guid)
      => studioSystem.GetMixerVCA(guid);

    // Return a global parameter description of the system by its path
    public static FMODParameterDescription GetGlobalParameterDescription(string path)
      => studioSystem.GetGlobalParameterDescription(path);

    // Return a global parameter description by its unique identifier
    public static FMODParameterDescription GetGlobalParameterDescription(FMOD.Studio.PARAMETER_ID guid)
      => studioSystem.GetGlobalParameterDescription(guid);

    // Return a global parameter value of the system by its name
    public static float GetGlobalParameter(string name)
      => studioSystem.GetGlobalParameter(name);
    public static float GetGlobalParameter(string name, out float finalValue)
      => studioSystem.GetGlobalParameter(name, out finalValue);

    // Return a global parameter value of the system by its unique identifier
    public static float GetGlobalParameter(FMOD.Studio.PARAMETER_ID id)
      => studioSystem.GetGlobalParameter(id);
    public static float GetGlobalParameter(FMOD.Studio.PARAMETER_ID id, out float finalValue)
      => studioSystem.GetGlobalParameter(id, out finalValue);

    // Set a global parameter value of the system by its name
    public static void SetGlobalParameter(string name, float value, bool ignoreSeekSpeed = false)
      => studioSystem.SetGlobalParameter(name, value, ignoreSeekSpeed);

    // Set a global parameter value of the system by its name, looking up the value label
    public static void SetGlobalParameter(string name, string label, bool ignoreSeekSpeed = false)
      => studioSystem.SetGlobalParameter(name, label, ignoreSeekSpeed);

    // Set a global parameter value of the system by its unique identifier
    public static void SetGlobalParameter(FMOD.Studio.PARAMETER_ID id, float value, bool ignoreSeekSpeed = false)
      => studioSystem.SetGlobalParameter(id, value, ignoreSeekSpeed);

    // Set a global parameter value of the system by its unique identifier, looking up the value label
    public static void SetGlobalParameter(FMOD.Studio.PARAMETER_ID id, string label, bool ignoreSeekSpeed = false)
      => studioSystem.SetGlobalParameter(id, label, ignoreSeekSpeed);

    // Return the ID for a bank, event, snapshot, bus or VCA of the system
    public static FMOD.GUID LookupGUID(string path)
      => studioSystem.LookupGUID(path);

    // Return the path for a bank, event, snapshot, bus or VCA of the system
    public static string LookupPath(FMOD.GUID guid)
      => studioSystem.LookupPath(guid);
  }
}