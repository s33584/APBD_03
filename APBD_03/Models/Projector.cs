using Rental.Equipment;

namespace Rental.Models;

public sealed class Projector : Equipment
{
    public Projector(string name, string description, int voltage, int brightnessLumens)
        : base(name, description)
    {
        if (voltage <= 0)
        {
            throw new ArgumentException("Projector voltage must be greater than zero.");
        }

        if (brightnessLumens <= 0)
        {
            throw new ArgumentException("Projector brightness must be greater than zero.");
        }

        Voltage = voltage;
        BrightnessLumens = brightnessLumens;
    }

    public int Voltage { get; }

    public int BrightnessLumens { get; }
}
