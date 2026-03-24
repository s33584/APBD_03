namespace APBD_03.Domain;

public enum EquipmentStatus
{
    Available = 1,
    Rented = 2,
    Unavailable = 3
}

public abstract class Equipment
{
    protected Equipment(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Equipment name is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Equipment description is required.");
        }

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        Status = EquipmentStatus.Available;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string Description { get; }

    public EquipmentStatus Status { get; private set; }

    public void MarkAsUnavailable()
    {
        Status = EquipmentStatus.Unavailable;
    }

    public void MarkAsAvailable()
    {
        Status = EquipmentStatus.Available;
    }

    public void MarkAsRented()
    {
        if (Status != EquipmentStatus.Available)
        {
            throw new InvalidOperationException("Equipment is not available for rental.");
        }

        Status = EquipmentStatus.Rented;
    }

    public void MarkAsReturned()
    {
        if (Status == EquipmentStatus.Unavailable)
        {
            throw new InvalidOperationException("Equipment is marked unavailable and cannot be returned.");
        }

        Status = EquipmentStatus.Available;
    }
}
