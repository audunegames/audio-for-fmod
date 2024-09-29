namespace Audune.Audio
{
  // Class that defines extension methods for FMOD
  public static class FMODExtensions
  {
    #region Getting display names
    // Prefixes for paths of FMOD components
    public static readonly string BankPrefix = "bank:/";
    public static readonly string EventDescriptionPrefix = "event:/";
    public static readonly string MixerBusPrefix = "bus:/";
    public static readonly string MixerVCAPrefix = "vca:/";


    // Return the display name of a bank
    public static string GetDisplayName(this FMODBank bank)
    {
      var name = bank.path;
      return name.StartsWith(BankPrefix) ? name.Substring(BankPrefix.Length) : name;
    }

    // Return the display name of an event description
    public static string GetDisplayName(this FMODEventDescription eventDescription)
    {
      var name = eventDescription.path;
      return name.StartsWith(EventDescriptionPrefix) ? name.Substring(EventDescriptionPrefix.Length) : name;
    }

    // Return the display name of a mixer bus
    public static string GetDisplayName(this FMODMixerBus bus)
    {
      var name = bus.path;
      return name.StartsWith(MixerBusPrefix) ? name.Substring(MixerBusPrefix.Length) : name;
    }

    // Return the display name of an event description
    public static string GetDisplayName(this FMODMixerVCA vca)
    {
      var name = vca.path;
      return name.StartsWith(MixerVCAPrefix) ? name.Substring(MixerVCAPrefix.Length) : name;
    }
    #endregion
  }
}
