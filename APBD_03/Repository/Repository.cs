using APBD_03.Main;

namespace APBD_03;

public class Repository
{
    public readonly List<User> Users = [];
    public readonly List<Equipment> Equipment = [];
    public readonly List<Rental> Rentals = [];

    public void AddUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user, "User cannot be null");
        Users.Add(user);
    }

    public void AddEquipment(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, "Equipment cannot be null");
        this.Equipment.Add(equipment);
    }

    public void AddRental(Rental rental)
    {
        ArgumentNullException.ThrowIfNull(rental, "Rental cannot be null");
        Rentals.Add(rental);
    }

    public User? GetUserById(Guid id) =>
        Users.First(u => u.Id == id);

    public Equipment? GetEquipmentById(Guid id) =>
        Equipment.First(e => e.Id == id);

    public Rental? GetRentalById(Guid id) =>
        Rentals.First(r => r.Id == id);

    public bool UserExists(Guid id) =>
        Users.Exists(u => u.Id == id);

    public bool EquipmentExists(Guid id) =>
        Equipment.Exists(e => e.Id == id);
}
