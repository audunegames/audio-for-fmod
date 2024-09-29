namespace Audune.Audio
{
  // Class that defines a component of an FMOD event instance
  public abstract class FMODEventInstanceComponent : FMODStudioSystemComponent
  {
    // Reference to the event instance
    protected readonly FMODEventInstance _instance;


    #region Constructors
    // Constructor
    public FMODEventInstanceComponent(FMODStudioSystem system, FMODEventInstance instance) : base(system)
    {
      _instance = instance;
    }
    #endregion
  }
}
