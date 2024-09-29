using FMODUnity;
using UnityEngine;

namespace Audune.Audio
{
  // Class that defines a state vector for an FMOD event instance
  public abstract class StateVector
  {
    // Return the position of the state vector
    public abstract Vector3 position { get; }

    // Return the rotation of the state vector
    public abstract Quaternion rotation { get; }

    // Return the velocity of the state vector
    public abstract Vector3 velocity { get; }


    // Apply the state vector to an event instance
    internal abstract void ApplyTo(FMODEventInstance instance);


    


    // Return the velocity of a transform
    public static Vector3 GetVelocity(Transform transform)
    {
      if (transform.TryGetComponent<Rigidbody>(out var rigidbody))
        return rigidbody.velocity;
      else if (transform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
        return rigidbody2D.velocity;
      else
        return Vector3.zero;
    }

    // Create a state vector backed by a vector
    public static StateVector Of(Vector3? position = null, Vector3? velocity = null)
    {
      return new SimpleStateVector(position, velocity);
    }

    // Create a state vector backed by a transform
    public static StateVector Of(Transform transform)
    {
      return new TransformStateVector(transform);
    }

    // Create a state vector backed by FMOD spatial attributes
    public static StateVector Of(FMODSpatialAttributes spatialAttributes)
    {
      return new SpatialAttributesStateVector(spatialAttributes);
    }


    // Convert a vector to a state vector
    public static implicit operator StateVector(Vector3 position)
    {
      return Of(position);
    }

    // Convert a transform to a state vector
    public static implicit operator StateVector(Transform transform)
    {
      return Of(transform);
    }

    // Convert FMOD spatial attributes to a state vector
    public static implicit operator StateVector(FMODSpatialAttributes spatialAttributes)
    {
      return Of(spatialAttributes);
    }
  }


  // Class that defines a state vector backed by variables
  internal sealed class SimpleStateVector : StateVector
  {
    // The position of the state vector
    public readonly Vector3 _position;

    // Return the rotation of the state vector
    public readonly Quaternion _rotation;

    // The velocity of the state vector
    public readonly Vector3 _velocity;


    // Return the position of the state vector
    public override Vector3 position => _position;

    // Return the rotation of the state vector
    public override Quaternion rotation => _rotation;

    // Return the velocity of the state vector
    public override Vector3 velocity => _velocity;


    // Constructor
    internal SimpleStateVector(Vector3? position = null, Vector3? velocity = null)
    {
      _position = position ?? Vector3.zero;
      _velocity = velocity ?? Vector3.zero;
    }

    // Apply the state vector to an event instance
    internal override void ApplyTo(FMODEventInstance instance)
    {
      instance.spatialAttributes.position = position;
      instance.spatialAttributes.velocity = velocity;
    }
  }


  // Cass that defines a state vector backed by a transform
  internal sealed class TransformStateVector : StateVector
  {
    // The transform of the state vector
    public readonly Transform transform;


    // Return the position of the state vector
    public override Vector3 position => transform.position;

    // Return the rotation of the state vector
    public override Quaternion rotation => transform.rotation;

    // Return the velocity of the state vector
    public override Vector3 velocity => GetVelocity(transform);
    

    // Constructor
    internal TransformStateVector(Transform transform)
    {
      this.transform = transform;
    }

    // Apply the state vector to an event instance
    internal override void ApplyTo(FMODEventInstance instance)
    {
      if (transform.TryGetComponent<Rigidbody>(out var rigidbody))
        RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody);
      else if (transform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
        RuntimeManager.AttachInstanceToGameObject(instance.native, transform, rigidbody2D);
      else
        RuntimeManager.AttachInstanceToGameObject(instance.native, transform);
    }
  }


  // Cass that defines a state vector backed by FMOD spatial attributes
  internal sealed class SpatialAttributesStateVector : StateVector
  {
    // The spatial attributes of the state vector
    public readonly FMODSpatialAttributes spatialAttributes;


    // Return the position of the state vector
    public override Vector3 position => spatialAttributes.position;

    // Return the rotation of the state vector
    public override Quaternion rotation => spatialAttributes.rotation;

    // Return the velocity of the state vector
    public override Vector3 velocity => spatialAttributes.velocity;
    

    // Constructor
    internal SpatialAttributesStateVector(FMODSpatialAttributes spatialAttributes)
    {
      this.spatialAttributes = spatialAttributes;
    }

    // Apply the state vector to an event instance
    internal override void ApplyTo(FMODEventInstance instance)
    {
      instance.spatialAttributes = spatialAttributes;
    }
  }
}