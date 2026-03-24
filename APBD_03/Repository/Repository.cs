using APBD_03.Domain;

namespace APBD_03;

public class Repository
{
    private readonly List<User> users = [];
    private readonly List<Equipment> equipment = [];
    private readonly List<Rental> rentals = [];

    public void AddUser(User user)
    {
        user ?? throw ArgumentNullException("User should not be null");
        users.Add(user);
    }

    public void AddEquipment(Equipment equipment)
    {
        equipment ?? throw ArgumentNullException("Equipment should not be null");
        this.equipment.Add(equipment);
    }

    public void AddRental(Rental rental)
    {
        rental ?? throw ArgumentNullException("Rental should not be null");
        rentals.Add(rental);
    }

    public User? GetUserById(Guid id) =>
        users.First(u => u.Id == id);

    public Equipment? GetEquipmentById(Guid id) =>
        equipment.First(e => e.Id == id);

    public Rental? GetRentalById(Guid id) =>
        rentals.First(r => r.Id == id);

    public bool UserExists(Guid id) =>
        users.Exists(u => u.Id == id);

    public bool EquipmentExists(Guid id) =>
        equipment.Exists(e => e.Id == id);
}
