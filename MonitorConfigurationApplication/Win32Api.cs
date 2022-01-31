using System.Runtime.InteropServices;

namespace MonitorConfigurationApplication
{
    // A collection of win32 api functions related to monitor configuration
    // Docs: https://docs.microsoft.com/en-us/windows/win32/api/_monitor/
    public static class Win32Api
    {
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("Dxva2.dll")]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, 
            out uint pdwNumberOfPhysicalMonitors);

        [DllImport("Dxva2.dll")]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, 
            uint dwPhysicalMonitorArraySize, PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("Dxva2.dll")]
        public static extern bool SetMonitorBrightness(IntPtr hMonitor, int brightness);

        [DllImport("Dxva2.dll")]
        public static extern bool GetMonitorCapabilities(IntPtr hMonitor, 
            out uint pdwMonitorCapabilities, out uint pdwSupportedColorTemperatures);

        [DllImport("Dxva2.dll")]
        public static extern bool GetMonitorBrightness(IntPtr hMonitor,
            out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

        [DllImport("Dxva2.dll")]
        public static extern bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string? lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, uint dwFlags);
    }
}
