using System.Runtime.InteropServices;

namespace MonitorConfigurationApplication
{
    // A structure of a physical model used for monitor configuration
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]  public string szPhysicalMonitorDescription;
    }
    
}
