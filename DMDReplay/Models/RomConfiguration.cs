namespace DMDReplay.Models
{
    public class RomConfiguration
    {
        public string RomName { get; set; } = string.Empty;
        public List<int> DipSwitches { get; set; } = new();
        public bool ShowExternalDmd { get; set; }
        public bool ShowNativePinmameDmd { get; set; }
        public List<SwitchConfig> SwitchInitialization { get; set; } = new();    
    }
}
