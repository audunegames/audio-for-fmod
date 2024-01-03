using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Audio
{
  // Class that represents the FMOD Studio system
  // Listed at https://fmod.com/docs/2.02/api/studio-api-system.html
  public class FMODStudioSystem : IDisposable, IEquatable<FMODStudioSystem>
  {
    // Dictionary of all systems
    private static readonly Dictionary<IntPtr, FMODStudioSystem> _instances = new Dictionary<IntPtr, FMODStudioSystem>();


    // The native handle of the system
    private readonly FMOD.Studio.System _nativeSystem;


    // Event handler when a bank has just been unloaded
    public event FMODCallback<FMODBank> OnBankUnloaded;

    // Event handler when a live update connection has been established
    public event FMODCallback OnLiveUpdateConnected;

    // Event handler when a live update session disconnects
    public event FMODCallback OnLiveUpdateDisconnected;


    // Create a new wrapper or get a cached one
    internal static FMODStudioSystem Of(FMOD.Studio.System nativeSystem)
    {
      if (!nativeSystem.isValid())
        return null;

      if (_instances.TryGetValue(nativeSystem.handle, out var instance))
        return instance;

      instance = new FMODStudioSystem(nativeSystem);
      _instances.Add(nativeSystem.handle, instance);
      return instance;
    }

    // Constructor from a native system
    private FMODStudioSystem(FMOD.Studio.System nativeSystem)
    {
      if (!nativeSystem.isValid())
        throw new ArgumentException(nameof(nativeSystem), "The specified system is not valid");

      _nativeSystem = nativeSystem;
      _nativeSystem.setCallback(new FMOD.Studio.SYSTEM_CALLBACK(CallbackHandler), FMOD.Studio.SYSTEM_CALLBACK_TYPE.ALL & ~FMOD.Studio.SYSTEM_CALLBACK_TYPE.PREUPDATE & ~FMOD.Studio.SYSTEM_CALLBACK_TYPE.POSTUPDATE);
    }

    // Destructor
    public void Dispose()
    {
      _nativeSystem.release().Check();
    }


    // Return if the system equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FMODStudioSystem);
    }

    // Return if the system equals another system
    public bool Equals(FMODStudioSystem other)
    {
      return other is not null && _nativeSystem.handle.Equals(other._nativeSystem.handle);
    }

    // Return the hash code of the system
    public override int GetHashCode()
    {
      return HashCode.Combine(_nativeSystem.handle);
    }


    // Return the native handle of the parameter description
    internal FMOD.Studio.System native => _nativeSystem;


    // Return the banks of the system
    public IEnumerable<FMODBank> banks {
      get {
        _nativeSystem.getBankList(out var array).Check();
        return array.Select(bank => FMODBank.Of(this, bank));
      }
    }


    // Return a bank of the system by its path
    public FMODBank GetBank(string path)
    {
      _nativeSystem.getBank(path, out var bank).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODBank.Of(this, bank);
    }

    // Return a bank of the system by its unique identifier
    public FMODBank GetBank(FMOD.GUID guid)
    {
      _nativeSystem.getBankByID(guid, out var bank).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODBank.Of(this, bank);
    }

    // Return an event description of the system by its path
    public FMODEventDescription GetEvent(string path)
    {
      _nativeSystem.getEvent(path, out var eventDescription).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODEventDescription.Of(this, eventDescription);
    }

    // Return an event description of the system by its unique identifier
    public FMODEventDescription GetEvent(FMOD.GUID guid)
    {
      _nativeSystem.getEventByID(guid, out var eventDescription).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODEventDescription.Of(this, eventDescription);
    }

    // Return a bus of the system by its path
    public FMODMixerBus GetMixerBus(string path)
    {
      _nativeSystem.getBus(path, out var bus).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODMixerBus.Of(this, bus);
    }

    // Return a bus of the system by its unique identifier
    public FMODMixerBus GetMixerBus(FMOD.GUID guid)
    {
      _nativeSystem.getBusByID(guid, out var bus).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODMixerBus.Of(this, bus);
    }

    // Return a VCA of the system by its path
    public FMODMixerVCA GetMixerVCA(string path)
    {
      _nativeSystem.getVCA(path, out var vca).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODMixerVCA.Of(this, vca);
    }

    // Return a VCA of the system by its unique identifier
    public FMODMixerVCA GetMixerVCA(FMOD.GUID guid)
    {
      _nativeSystem.getVCAByID(guid, out var vca).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return FMODMixerVCA.Of(this, vca);
    }

    // Return a global parameter description of the system by its path
    public FMODParameterDescription GetGlobalParameterDescription(string path)
    {
      _nativeSystem.getParameterDescriptionByName(path, out var parameterDescription).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return new FMODParameterDescription(this, parameterDescription);
    }

    // Return a global parameter description by its unique identifier
    public FMODParameterDescription GetGlobalParameterDescription(FMOD.Studio.PARAMETER_ID id)
    {
      _nativeSystem.getParameterDescriptionByID(id, out var parameterDescription).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return new FMODParameterDescription(this, parameterDescription);
    }

    // Return a global parameter value of the system by its name
    public float GetGlobalParameter(string name)
    {
      _nativeSystem.getParameterByName(name, out var value).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return value;
    }
    public float GetGlobalParameter(string name, out float finalValue)
    {
      _nativeSystem.getParameterByName(name, out var value, out finalValue).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return value;
    }

    // Return a global parameter value of the system by its unique identifier
    public float GetGlobalParameter(FMOD.Studio.PARAMETER_ID id)
    {
      _nativeSystem.getParameterByID(id, out var value).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return value;
    }
    public float GetGlobalParameter(FMOD.Studio.PARAMETER_ID id, out float finalValue)
    {
      _nativeSystem.getParameterByID(id, out var value, out finalValue).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return value;
    }

    // Set a global parameter value of the system by its name
    public void SetGlobalParameter(string name, float value, bool ignoreSeekSpeed = false)
    {
      _nativeSystem.setParameterByName(name, value, ignoreSeekSpeed).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
    }

    // Set a global parameter value of the system by its name, looking up the value label
    public void SetGlobalParameter(string name, string label, bool ignoreSeekSpeed = false)
    {
      _nativeSystem.setParameterByNameWithLabel(name, label, ignoreSeekSpeed).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
    }

    // Set a global parameter value of the system by its unique identifier
    public void SetGlobalParameter(FMOD.Studio.PARAMETER_ID id, float value, bool ignoreSeekSpeed = false)
    {
      _nativeSystem.setParameterByID(id, value, ignoreSeekSpeed).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
    }

    // Set a global parameter value of the system by its unique identifier, looking up the value label
    public void SetGlobalParameter(FMOD.Studio.PARAMETER_ID id, string label, bool ignoreSeekSpeed = false)
    {
      _nativeSystem.setParameterByIDWithLabel(id, label, ignoreSeekSpeed).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
    }

    // Return the ID for a bank, event, snapshot, bus or VCA of the system
    public FMOD.GUID LookupGUID(string path)
    {
      _nativeSystem.lookupID(path, out var guid).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return guid;
    }

    // Return the path for a bank, event, snapshot, bus or VCA of the system
    public string LookupPath(FMOD.GUID guid)
    {
      _nativeSystem.lookupPath(guid, out var path).Check(FMOD.RESULT.ERR_EVENT_NOTFOUND);
      return path;
    }


    // Return a sound with the specified key from the audio table of the system
    internal FMOD.Sound CreateAudioTableSound(string key, ref FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES programmerSound)
    {
      // Create the sound
      var soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;

      _nativeSystem.getCoreSystem(out var nativeCoreSystem).Check();
      _nativeSystem.getSoundInfo(key, out var nativeSoundInfo).Check();
      nativeCoreSystem.createSound(nativeSoundInfo.name_or_data, nativeSoundInfo.mode | soundMode, ref nativeSoundInfo.exinfo, out var nativeSound);

      // Fill the programmer sound properties
      programmerSound.sound = nativeSound.handle;
      programmerSound.subsoundIndex = nativeSoundInfo.subsoundindex;

      // Return the sound
      return nativeSound;
    }


    // Callback handler for the system
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.SYSTEM_CALLBACK))]
    private static FMOD.RESULT CallbackHandler(IntPtr systemPtr, FMOD.Studio.SYSTEM_CALLBACK_TYPE type, IntPtr commandDataPtr, IntPtr userDataPtr)
    {
      if (!_instances.TryGetValue(systemPtr, out var system))
        return FMOD.RESULT.OK;
      if (!system._nativeSystem.isValid())
        return FMOD.RESULT.OK;

      try
      {
        switch (type)
        {
          case FMOD.Studio.SYSTEM_CALLBACK_TYPE.BANK_UNLOAD:
            system.OnBankUnloaded?.Invoke(FMODBank.Of(system, new FMOD.Studio.Bank(commandDataPtr)));
            return FMOD.RESULT.OK;

          case FMOD.Studio.SYSTEM_CALLBACK_TYPE.LIVEUPDATE_CONNECTED:
            system.OnLiveUpdateConnected?.Invoke();
            return FMOD.RESULT.OK;

          case FMOD.Studio.SYSTEM_CALLBACK_TYPE.LIVEUPDATE_DISCONNECTED:
            system.OnLiveUpdateDisconnected?.Invoke();
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
