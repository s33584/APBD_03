using Rental.Equipment;

namespace Rental.Models;

public sealed class Camera : Equipment
{
    public Camera(string name, string description, decimal aperture, int focalLengthMm)
        : base(name, description)
    {
        if (aperture <= 0)
        {
            throw new ArgumentException("Camera aperture must be greater than zero.");
        }

        if (focalLengthMm <= 0)
        {
            throw new ArgumentException("Camera focal length must be greater than zero.");
        }

        Aperture = aperture;
        FocalLengthMm = focalLengthMm;
    }

    public decimal Aperture { get; }

    public int FocalLengthMm { get; }
}
