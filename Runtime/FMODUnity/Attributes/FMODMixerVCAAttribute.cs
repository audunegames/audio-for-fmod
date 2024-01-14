using System;
using UnityEngine;

namespace Audune.Audio
{
  // Attribute that specifies that a string should be interpreted as a path to a VCA
  [AttributeUsage(AttributeTargets.Field)]
  public class FMODMixerVCAAttribute : PropertyAttribute
  {
  }
}