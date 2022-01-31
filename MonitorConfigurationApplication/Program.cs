using MonitorConfigurationApplication;
using System.ComponentModel;
using System.Runtime.InteropServices;

try
{
    #region variables and constants
    const uint MONITOR_DEFAULTTONULL = 0u;
    const uint MONITOR_DEFAULTTOPRIMARY = 1u;
    const uint MONITOR_DEFAULTTONEAREST = 2u;

    const int DISP_CHANGE_SUCCESSFUL = 0;   // Indicates that the function succeeded.
    const int DISP_CHANGE_BADMODE = -2;     // The graphics mode is not supported.
    const int DISP_CHANGE_FAILED = -1;      // The display driver failed the specified graphics mode.
    const int DISP_CHANGE_RESTART = 1;      // The computer must be restarted for the graphics mode to work.

    uint dwNumberOfPhysicalMonitors = 0u;
    uint pdwMonitorCapabilities = 0u;
    uint pdwSupportedColorTemperatures = 0u;

    uint pdwMinimumBrightness = 0u;
    uint pdwCurrentBrightness = 0u;
    uint pdwMaximumBrightness = 0u;
    uint pdwMinimumWidthOrHeight = 0u;
    uint pdwCurrentWidthOrHeight = 0u;
    uint pdwMaximumWidthOrHeight = 0u;
    #endregion

    IntPtr handle = Win32Api.MonitorFromWindow(Win32Api.GetDesktopWindow(), MONITOR_DEFAULTTONULL);

    // Throw error if we cannot retrieve a number of physical monitors
    if (!Win32Api.GetNumberOfPhysicalMonitorsFromHMONITOR(handle, out dwNumberOfPhysicalMonitors))
        throw new Win32Exception(Marshal.GetLastWin32Error());

    PHYSICAL_MONITOR[] pPhysicalMonitorArray = new PHYSICAL_MONITOR[dwNumberOfPhysicalMonitors];

    // Throw error if we cannot retrieve physical monitors
    if (!Win32Api.GetPhysicalMonitorsFromHMONITOR(handle, dwNumberOfPhysicalMonitors, pPhysicalMonitorArray))
        throw new Win32Exception(Marshal.GetLastWin32Error());

    // Throw error if monitor does not support the DDC/CI protocol: https://en.wikipedia.org/wiki/Display_Data_Channel
    if (!Win32Api.GetMonitorCapabilities(pPhysicalMonitorArray[0].hPhysicalMonitor,
    out pdwMonitorCapabilities, out pdwSupportedColorTemperatures))
        throw new Win32Exception(Marshal.GetLastWin32Error());

    IntPtr hPhysicalMonitor = pPhysicalMonitorArray[0].hPhysicalMonitor;

    // If we can hit this part of the program, the monitor supports DDC/CI and the monitor can be configured programmatically.
    Win32Api.GetMonitorBrightness(hPhysicalMonitor, out pdwMinimumBrightness, out pdwCurrentBrightness, out pdwMaximumBrightness);

    #region main program logic
    Console.WriteLine("This console application allows configuration of monitor settings. Type \"help\" for more information.");
    while (true)
    {
        string? input = Console.ReadLine();

        if (input?.Split("=")[0].ToLower() == "brightness")
        {
            if (input?.Split("=")[1].ToLower() == null) return;

            uint brightnessValue = Convert.ToUInt32(input?.Split("=")[1].ToLower());

            if (brightnessValue > pdwMaximumBrightness)
                throw new Exception($"Value {brightnessValue} exceeds maximum brightness value {pdwMaximumBrightness}");

            if (brightnessValue < pdwMinimumBrightness)
                throw new Exception($"Value {brightnessValue} is less than minimum brightness value {pdwMinimumBrightness}");

            bool result = Win32Api.SetMonitorBrightness(hPhysicalMonitor, brightnessValue);
            if (result) Console.WriteLine("Brightness changed successfully.");
        }
        else if (input?.Split("=")[0].ToLower() == "display")
        {
            if (input?.Split("=")[1].Split("x")[0] == null || input?.Split("=")[1].Split("x")[1] == null) return;

            DEVMODE mode = new DEVMODE();
            mode.dmSize = (ushort)Marshal.SizeOf(mode);

            if (!Win32Api.EnumDisplaySettings(null, -1, ref mode))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            DEVMODE newMode = mode; // copy the settings of current screen configuration

            newMode.dmPelsWidth = Convert.ToUInt32(input?.Split("=")[1].Split("x")[0]);
            newMode.dmPelsHeight = Convert.ToUInt32(input?.Split("=")[1].Split("x")[1]);

            int result = Win32Api.ChangeDisplaySettings(ref newMode, 0);
            if (result == DISP_CHANGE_SUCCESSFUL) Console.WriteLine("Display settings changed successfully.");
        }
        else if (input?.ToLower() == "clear") Console.Clear(); 
        else if (input?.ToLower() == "help")
        {
            Console.WriteLine("Showing help here...");
        }
        else return;
    }
    #endregion
}
catch (Win32Exception win32Ex)
{
    Console.WriteLine(win32Ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
 


    



