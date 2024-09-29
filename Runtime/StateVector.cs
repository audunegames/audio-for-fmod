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
        return UnityEngine.Vector3.zero;
    }

    // Create a state vector backed by the specified object
    public static StateVector Vector3(Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null)
      => new SimpleStateVector(position, rotation, velocity);
    public static StateVector Vector2(Vector2? position = null, Quaternion? rotation = null, Vector2? velocity = null)
      => new SimpleStateVector(position, rotation, velocity);
    public static StateVector Transform(Transform transform)
      => new TransformStateVector(transform);
    public static StateVector SpatialAttributes(FMODSpatialAttributes spatialAttributes)
      => new SpatialAttributesStateVector(spatialAttributes);

    // Convert objects to state vectors
    public static implicit operator StateVector(Vector3 position)
      => Vector2(position);
    public static implicit operator StateVector(Vector2 position)
      => Vector2(position);
    public static implicit operator StateVector(Transform transform)
      => Transform(transform);
    public static implicit operator StateVector(FMODSpatialAttributes spatialAttributes)
      => SpatialAttributes(spatialAttributes);
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
    internal SimpleStateVector(Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null)
    {
      _position = position ?? UnityEngine.Vector3.zero;
      _rotation = rotation ?? Quaternion.identity;
      _velocity = velocity ?? UnityEngine.Vector3.zero;
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
    private readonly Transform _transform;


    // Return the position of the state vector
    public override Vector3 position => _transform.position;

    // Return the rotation of the state vector
    public override Quaternion rotation => _transform.rotation;

    // Return the velocity of the state vector
    public override Vector3 velocity => GetVelocity(_transform);
    

    // Constructor
    internal TransformStateVector(Transform transform)
    {
      _transform = transform;
    }

    // Apply the state vector to an event instance
    internal override void ApplyTo(FMODEventInstance instance)
    {
      if (_transform.TryGetComponent<Rigidbody>(out var rigidbody))
        RuntimeManager.AttachInstanceToGameObject(instance.native, _transform, rigidbody);
      else if (_transform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
        RuntimeManager.AttachInstanceToGameObject(instance.native, _transform, rigidbody2D);
      else
        RuntimeManager.AttachInstanceToGameObject(instance.native, _transform);
    }
  }


  // Cass that defines a state vector backed by FMOD spatial attributes
  internal sealed class SpatialAttributesStateVector : StateVector
  {
    // The spatial attributes of the state vector
    private readonly FMODSpatialAttributes _spatialAttributes;


    // Return the position of the state vector
    public override Vector3 position => _spatialAttributes.position;

    // Return the rotation of the state vector
    public override Quaternion rotation => _spatialAttributes.rotation;

    // Return the velocity of the state vector
    public override Vector3 velocity => _spatialAttributes.velocity;
    

    // Constructor
    internal SpatialAttributesStateVector(FMODSpatialAttributes spatialAttributes)
    {
      _spatialAttributes = spatialAttributes;
    }

    // Apply the state vector to an event instance
    internal override void ApplyTo(FMODEventInstance instance)
    {
      instance.spatialAttributes.native = _spatialAttributes.native;
    }
  }
}