using MonitorConfigurationApplication;
using System.ComponentModel;
using System.Runtime.InteropServices;

try
{
    const uint MONITOR_DEFAULTTONULL = 0u;
    const uint MONITOR_DEFAULTTOPRIMARY = 1u;
    const uint MONITOR_DEFAULTTONEAREST = 2u;
    uint dwNumberOfPhysicalMonitors = 0u;
    uint pdwMonitorCapabilities = 0u;
    uint pdwSupportedColorTemperatures = 0u;

    uint pdwMinimumBrightness = 0u;
    uint pdwCurrentBrightness = 0u;
    uint pdwMaximumBrightness = 0u;
    uint pdwMinimumWidthOrHeight = 0u;
    uint pdwCurrentWidthOrHeight = 0u;
    uint pdwMaximumWidthOrHeight = 0u;

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

    while (true)
    {
        string? input = Console.ReadLine();
        
        if (input?.Split("=")[0].ToLower() == "brightness" && input?.Split("=")[1].ToLower() != null)
        {
            uint brightnessValue = Convert.ToUInt32(input?.Split("=")[1].ToLower());

            if (brightnessValue > pdwMaximumBrightness)
                throw new Exception($"Value {brightnessValue} exceeds maximum brightness value {pdwMaximumBrightness}");

            if (brightnessValue < pdwMinimumBrightness)
                throw new Exception($"Value {brightnessValue} is less than minimum brightness value {pdwMinimumBrightness}");

            Win32Api.SetMonitorBrightness(hPhysicalMonitor, brightnessValue);
        }
        else return;
    }  
}
catch (Win32Exception win32Ex)
{
    Console.WriteLine(win32Ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
 


    



