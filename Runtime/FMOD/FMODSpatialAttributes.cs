using FMODUnity;
using UnityEngine;

namespace Audune.Audio
{
  // Class that respresents a position, velocity and orientation for 3D attenuation
  // Listed at https://fmod.com/docs/2.02/api/core-api-common.html#fmod_3d_attributes
  public sealed class FMODSpatialAttributes : FMODEventInstanceComponent
  {
    #region Constructors
    // Constructor from native 3D attributes
    public FMODSpatialAttributes(FMODStudioSystem system, FMODEventInstance instance) : base(system, instance)
    {
    }
    #endregion

    #region Properties
    // Return the native handle of the 3D attributes
    internal FMOD.ATTRIBUTES_3D native {
      get {
        _instance.native.get3DAttributes(out var attributes).Check();
        return attributes;
      }
      set {
        _instance.native.set3DAttributes(value).Check();
      }
    }


    // Return the position of the 3D attributes
    public Vector3 position {
      get {
        var position = native.position;
        return new Vector3(position.x, position.y, position.z);
      }
      set {
        var _lastNative = native;
        native = new FMOD.ATTRIBUTES_3D {
          position = value.ToFMODVector(),
          velocity = _lastNative.velocity,
          forward = _lastNative.forward,
          up = _lastNative.up,
        };
      }
    }

    // Return the velocity of the 3D attributes
    public Vector3 velocity {
      get {
        var velocity = native.velocity;
        return new Vector3(velocity.x, velocity.y, velocity.z);
      }
      set {
        var _lastNative = native;
        native = new FMOD.ATTRIBUTES_3D {
          position = _lastNative.position,
          velocity = value.ToFMODVector(),
          forward = _lastNative.forward,
          up = _lastNative.up,
        };
      }
    }

    // Return the forward orientation of the 3D attributes
    public Vector3 forward {
      get {
        var forward = native.forward;
        return new Vector3(forward.x, forward.y, forward.z);
      }
      set {
        var _lastNative = native;
        native = new FMOD.ATTRIBUTES_3D {
          position = _lastNative.position,
          velocity = _lastNative.velocity,
          forward = value.ToFMODVector(),
          up = _lastNative.up,
        };
      }
    }

    // Return the up orientation of the 3D attributes
    public Vector3 up {
      get {
        var up = native.up;
        return new Vector3(up.x, up.y, up.z);
      }
      set {
        var _lastNative = native;
        native = new FMOD.ATTRIBUTES_3D {
          position = _lastNative.position,
          velocity = _lastNative.velocity,
          forward = _lastNative.forward,
          up = value.ToFMODVector(),
        };
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
