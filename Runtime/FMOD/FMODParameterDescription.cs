using System;

namespace Audune.Audio
{
  // Class that respresents an event parameter description
  // Listed at https://fmod.com/docs/2.02/api/studio-api-common.html#fmod_studio_parameter_description
  public sealed class FMODParameterDescription : FMODStudioSystemComponent
  {
    // The native handle of the parameter description
    private readonly FMOD.Studio.PARAMETER_DESCRIPTION _nativeParameterDescription;


    #region Constructors
    // Constructor from a native parameter description
    public FMODParameterDescription(FMODStudioSystem system, FMOD.Studio.PARAMETER_DESCRIPTION nativeParameterDescription) : base(system)
    {
      _nativeParameterDescription = nativeParameterDescription;
    }
    #endregion

    #region Properties
    // Return the native handle of the parameter description
    internal FMOD.Studio.PARAMETER_DESCRIPTION native => _nativeParameterDescription;


    // Return the unique identifier of the parameter description
    public FMOD.GUID guid => _nativeParameterDescription.guid;

    // Return the name of the parameter description
    public string name => _nativeParameterDescription.name;

    // Return the ID of the parameter description
    public FMOD.Studio.PARAMETER_ID id => _nativeParameterDescription.id;

    // Return the minimum value of the parameter description
    public float minimum => _nativeParameterDescription.minimum;

    // Return the maximum value of the parameter description
    public float maximum => _nativeParameterDescription.maximum;

    // Return the default value of the parameter description
    public float defaultValue => _nativeParameterDescription.defaultvalue;

    // Return the type of the parameter description
    public FMOD.Studio.PARAMETER_TYPE yype => _nativeParameterDescription.type;

    // Return the flags of the parameter description
    public FMOD.Studio.PARAMETER_FLAGS flags => _nativeParameterDescription.flags;
    #endregion

    #region Methods
    // Return the global parameter value for the description
    public float GetGlobalParameter()
    {
      return _system.GetGlobalParameter(id);
    }
    public float GetGlobalParameter(out float finalValue)
    {
      return _system.GetGlobalParameter(id, out finalValue);
    }

    // Set the global parameter value for the description
    public void SetGlobalParameter(float value, bool ignoreSeekSpeed = false)
    {
      _system.SetGlobalParameter(id, value, ignoreSeekSpeed);
    }
    public void SetGlobalParameter(string label, bool ignoreSeekSpeed = false)
    {
      _system.SetGlobalParameter(id, label, ignoreSeekSpeed);
    }

    // Return a parameter value in an instance for the description
    public float GetParameter(FMODEventInstance instance)
    {
      return instance.GetParameter(id);
    }
    public float GetParameter(FMODEventInstance instance, out float finalValue)
    {
      return instance.GetParameter(id, out finalValue);
    }

    // Set a parameter value in an instance for the description
    public void SetParameter(FMODEventInstance instance, float value, bool ignoreSeekSpeed = false)
    {
      instance.SetParameter(id, value, ignoreSeekSpeed);
    }
    public void SetParameter(FMODEventInstance instance, string label, bool ignoreSeekSpeed = false)
    {
      instance.SetParameter(id, label, ignoreSeekSpeed);
    }
    #endregion
  }
}
