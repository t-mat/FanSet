using LibreHardwareMonitor.Hardware;

namespace FanSet;

public static class Program {
    private static void Main(string[] args) {
        bool reportOnly = true;

        Dictionary<string, string> dictionary = new();

        string CanonicalKey(string key) => key.ToLower();

        for (var i = 0; i < args.Length - 1; i += 2) {
            string key = CanonicalKey(args[i]);
            dictionary[key] = args[i + 1];
        }

        if (dictionary.Count != 0) {
            reportOnly = false;
        }

        ForEachSensor(
            dontClose: true,
            action: (ISensor sensor) =>
            {
                if (sensor.SensorType != SensorType.Control) {
                    return;
                }

                if (reportOnly) {
                    Console.WriteLine($"READ: {sensor.Name} == {Math.Round(sensor.Value ?? 0)}%");
                    return;
                }

                string key = CanonicalKey(sensor.Name);
                if (! dictionary.TryGetValue(key, out string? value)) {
                    return;
                }
                if (! int.TryParse(value, out int intValue)) {
                    return;
                }
                if (intValue is < 0 or > 100) {
                    return;
                }

                Console.WriteLine($"SET: {sensor.Name} := {intValue}%");
                sensor.Control.SetSoftware(intValue);
            });
    }

    private static void ForEachSensor(bool dontClose, Action<ISensor> action) {
        Computer computer = new()
        {
            IsCpuEnabled         = true,
            IsGpuEnabled         = true,
            IsMemoryEnabled      = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled  = true,
            IsNetworkEnabled     = true,
            IsStorageEnabled     = true
        };

        computer.Open();
        computer.Accept(new UpdateVisitor());

        foreach (IHardware hardware in computer.Hardware) {
            // Console.WriteLine($"Hardware: {hardware.Name}");
            foreach (IHardware subHardware in hardware.SubHardware) {
                // Console.WriteLine($"\tSubHardware: {subHardware.Name}");
                foreach (ISensor sensor in subHardware.Sensors) {
                    action(sensor);
                    // Console.WriteLine($"\t\tSensor: {sensor.Name}, sensorType:{sensor.SensorType}, value: {sensor.Value}");
                }
            }

            foreach (ISensor sensor in hardware.Sensors) {
                action(sensor);
                // Console.WriteLine($"\tSensor: {sensor.Name}, value: {sensor.Value}");
            }
        }

        if (dontClose) {
            Type.GetType("OpenHardwareMonitor.Hardware.Ring0, OpenHardwareMonitorLib")
                ?.GetMethod("Close")
                ?.Invoke(null, null);

            Type.GetType("OpenHardwareMonitor.Hardware.Opcode, OpenHardwareMonitorLib")
                ?.GetMethod("Close")
                ?.Invoke(null, null);
        } else {
            computer.Close();
        }
    }

    private class UpdateVisitor : IVisitor {
        public void VisitComputer(IComputer computer) {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware) {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) {
                subHardware.Accept(this);
            }
        }

        public void VisitSensor(ISensor sensor) {
        }

        public void VisitParameter(IParameter parameter) {
        }
    }
}
