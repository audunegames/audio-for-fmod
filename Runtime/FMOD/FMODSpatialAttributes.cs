using UnityEngine;

namespace Audune.Audio
{
  // Class that respresents a position, velocity and orientation for 3D attenuation
  // Listed at https://fmod.com/docs/2.02/api/core-api-common.html#fmod_3d_attributes
  public sealed class FMODSpatialAttributes
  {
    // The native handle of the 3D attributes
    private FMOD.ATTRIBUTES_3D _native3DAttributes;

    #region Constructors
    // Constructor from native 3D attributes
    public FMODSpatialAttributes(FMOD.ATTRIBUTES_3D native3DAttributes)
    {
      _native3DAttributes = native3DAttributes;
    }
    #endregion

    #region Properties
    // Return the native handle of the 3D attributes
    internal FMOD.ATTRIBUTES_3D native => _native3DAttributes;


    // Return the position of the 3D attributes
    public Vector3 position {
      get {
        var position = _native3DAttributes.position;
        return new Vector3(position.x, position.y, position.z);
      }
      set {
        _native3DAttributes.position = new FMOD.VECTOR { x = value.x, y = value.y, z = value.z };
      }
    }

    // Return the velocity of the 3D attributes
    public Vector3 velocity {
      get {
        var velocity = _native3DAttributes.velocity;
        return new Vector3(velocity.x, velocity.y, velocity.z);
      }
      set {
        _native3DAttributes.velocity = new FMOD.VECTOR { x = value.x, y = value.y, z = value.z };
      }
    }

    // Return the forward orientation of the 3D attributes
    public Vector3 forward {
      get {
        return new Vector3(forward.x, forward.y, forward.z);
      }
      set {
        _native3DAttributes.forward = new FMOD.VECTOR { x = value.x, y = value.y, z = value.z };
      }
    }

    // Return the up orientation of the 3D attributes
    public Vector3 up {
      get {
        var up = _native3DAttributes.up;
        return new Vector3(up.x, up.y, up.z);
      }
      set {
        _native3DAttributes.up = new FMOD.VECTOR { x = value.x, y = value.y, z = value.z };
      }
    }

    // Return the rotation of the 3D attributes
    public Quaternion rotation {
      get {
        return Quaternion.LookRotation(forward, up);
      }
    }
    #endregion
  }
}
