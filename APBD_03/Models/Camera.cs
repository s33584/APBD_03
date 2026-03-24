using APBD_03.Main;

namespace APBD_03.Models;

public class Camera : Equipment
{
    public Camera(string name, string description, float aperture, int focalLengthMm)
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

    public float Aperture { get; }

    public int FocalLengthMm { get; }
}
