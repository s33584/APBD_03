using APBD_03.Main;

namespace APBD_03.Models;

public class Laptop : Equipment
{
    public Laptop(string name, string description, string resolution, int ramGb)
        : base(name, description)
    {
        if (string.IsNullOrWhiteSpace(resolution))
        {
            throw new ArgumentException("Laptop resolution is required.");
        }

        if (ramGb <= 0)
        {
            throw new ArgumentException("Laptop RAM must be greater than zero.");
        }

        Resolution = resolution.Trim();
        RamGb = ramGb;
    }

    public string Resolution { get; }

    public int RamGb { get; }
}
