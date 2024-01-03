using System;

namespace Audune.Audio
{
  // Class that defines an exception that wraps an FMOD result
  public class FMODException : Exception
  {
    // The result wrapped by this exception
    public FMOD.RESULT Result { get; private set; }


    // Constructor
    public FMODException(FMOD.RESULT result) : base()
    {
      Result = result;
    }

    // Constructor with message
    public FMODException(FMOD.RESULT result, string message) : base(message)
    {
      Result = result;
    }
  }
}
