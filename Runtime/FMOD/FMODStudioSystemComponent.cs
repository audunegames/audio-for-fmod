namespace Audune.Audio
{
  // Class that defines a component of an FMOD studio system
  public abstract class FMODStudioSystemComponent
  {
    // Reference to the studio system
    protected readonly FMODStudioSystem _system;


    // Constructor
    public FMODStudioSystemComponent(FMODStudioSystem system)
    {
      _system = system;
    }
  }
}
