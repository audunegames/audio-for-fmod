using FMODUnity;
using UnityEngine;

namespace Audune.Audio.Editor
{
  // Class that defines GUI utility methods for FMOD in the Unity editor
  public static class FMODEditorGUIUtils
  {
    // Icon textures
    public static readonly Texture2D studioIcon = EditorUtils.LoadImage("StudioIcon.png");
    public static readonly Texture2D folderIcon = EditorUtils.LoadImage("FolderIconClosed.png");
    public static readonly Texture2D bankIcon = EditorUtils.LoadImage("BankIcon.png");
    public static readonly Texture2D eventIcon = EditorUtils.LoadImage("EventIcon.png");
    public static readonly Texture2D snapshotIcon = EditorUtils.LoadImage("SnapshotIcon.png");
  }
}