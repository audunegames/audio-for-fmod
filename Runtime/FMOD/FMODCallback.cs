using System;
using System.Runtime.InteropServices;

namespace Audune.Audio
{
  // Delegate that defines a callback
  public delegate void FMODCallback();

  // Delegate that defines a callback with a parameter
  public delegate void FMODCallback<T>(T parameter);

  // Delegate that defines a callback with a referenced parameter
  public delegate void FMODReferenceCallback<T>(ref T parameter) where T : struct;


  // Class that defines extensions for callbacks
  public static class FMODCallbackExtensions
  {
    // Wrap a callback with a parameter
    public static FMODCallback WithParameter<T>(this FMODReferenceCallback<T> callback, IntPtr parameterPtr) where T : struct
    {
      return () => {
        var parameter = Marshal.PtrToStructure<T>(parameterPtr);
        callback(ref parameter);
        Marshal.StructureToPtr(parameter, parameterPtr, false);
      };
    }
  }
}
