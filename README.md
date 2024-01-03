# FanSet

Someone could say I'm obsessed with controlling fans.

## Build

Prerequisites:
 - Windows 10 or 11
 - elevated (administrator) user privileges
   - LibreHardwareMonitor's internal driver requires it.
 - `dotnet` (.NET 8.0)
 - `git`

NOTE : This utility depends [LibreHardwareMonitorLib](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor).  But it may be [blocked by anti-virus softwares](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor/issues/984).

```
cmd.exe
pushd "%USERPROFILE%\Documents"
git clone https://github.com/t-mat/FanSet.git
cd FanSet
dotnet build --output bin -property:Configuration=Release
```

Continue the following commands with elevated (administrator) privileges.

```
net.exe session 1>nul 2>nul || (echo This script requires elevated rights.)
.\bin\FanSet.exe
```

## Usage

```
C:\Users\Michele\Documents\FanSet\FanSet\bin>fanset.exe

Fan Control #1 == 80%
Fan Control #2 == 40%
Fan Control #3 == 65%

C:\Users\Michele\Documents\FanSet\FanSet\bin>fanset.exe "Fan Control #1" 85

Fan Control #1 = 85%

```

## Why
I couldn't see an example on how to do this online so here we go.

## The catch
This program will not re-enable automatic fan control, so use it only if you want to manage your fans manually until next reboot.

## Credits
This project uses [LibreHardwareMonitorLib](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor). Huge thanks to them.
