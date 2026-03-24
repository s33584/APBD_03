using APBD_03;
using APBD_03.Main;

namespace APBD_03.Services;

public class RentalService
{
    private readonly Repository repository;

    public RentalService(Repository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException("Repository should be non null");
    }

    public void AddUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user, "User should not be null");

        if (repository.UserExists(user.Id))
        {
            throw new InvalidOperationException("User with this id already exists");
        }

        repository.AddUser(user);
    }

    public void AddEquipment(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment, "Equipment should not be null");

        if (repository.EquipmentExists(equipment.Id))
        {
            throw new InvalidOperationException("Equipment with this id already exists.");
        }

        repository.AddEquipment(equipment);
    }

    public List<Equipment> GetAllEquipment() => repository.Equipment.ToList();

    public List<Equipment> GetAvailableEquipment() =>
        repository.Equipment.Where(item => item.Status == EquipmentStatus.Available).ToList();

    public Rental RentEquipment(Guid userId, Guid equipmentId, int rentalDays, DateTime? rentedDateTime = null)
    {
        var user = repository.GetUserById(userId)
            ?? throw new KeyNotFoundException("User not found.");
        var equipment = repository.GetEquipmentById(equipmentId)
            ?? throw new KeyNotFoundException("Equipment not found.");

        if (equipment.Status != EquipmentStatus.Available)
        {
            throw new InvalidOperationException("Equipment is not available for rental.");
        }

        var activeRentalsCount = repository.Rentals.Count(rental => !rental.IsReturned && rental.User.Id == userId);
        var userLimit = GetActiveRentalLimit(user.UserType);
        if (activeRentalsCount >= userLimit)
        {
            throw new InvalidOperationException("User reached active rental limit.");
        }

        var rental = new Rental(user, equipment, rentedDateTime ?? DateTime.UtcNow, rentalDays);
        equipment.MarkAsRented();
        repository.AddRental(rental);

        return rental;
    }

    public Rental ReturnEquipment(Guid rentalId, DateTime? returnedDateTime = null)
    {
        var rental = repository.GetRentalById(rentalId)
            ?? throw new KeyNotFoundException("Rental not found.");

        rental.Return(returnedDateTime ?? DateTime.UtcNow);
        rental.Equipment.MarkAsReturned();

        return rental;
    }

    public void MarkEquipmentUnavailable(Guid equipmentId)
    {
        var equipment = repository.GetEquipmentById(equipmentId)
            ?? throw new KeyNotFoundException("Equipment not found.");

        if (equipment.Status == EquipmentStatus.Rented)
        {
            throw new InvalidOperationException("Cannot mark rented equipment as unavailable.");
        }

        equipment.MarkAsUnavailable();
    }

    public List<Rental> GetActiveRentalsForUser(Guid userId) =>
        repository.Rentals.Where(rental => rental.User.Id == userId && !rental.IsReturned).ToList();

    public List<Rental> GetOverdueRentals(DateTime? returnedDateTime = null)
    {
        var actual = returnedDateTime ?? DateTime.UtcNow;
        return repository.Rentals.Where(rental => rental.IsOverdue(actual)).ToList();
    }

    public RentalServiceSummary GetSummary(DateTime? actualDateTime = null)
    {
        var actual = actualDateTime ?? DateTime.UtcNow;
        var equipment = repository.Equipment;

        return new RentalServiceSummary(
            TotalUsers: repository.Users.Count,
            TotalEquipment: equipment.Count,
            AvailableEquipment: equipment.Count(item => item.Status == EquipmentStatus.Available),
            UnavailableEquipment: equipment.Count(item => item.Status == EquipmentStatus.Unavailable),
            ActiveRentals: repository.Rentals.Count(rental => !rental.IsReturned),
            OverdueRentals: repository.Rentals.Count(rental => rental.IsOverdue(actual)),
            TotalPenalties: repository.Rentals.Sum(rental => rental.PenaltyAmount));
    }

    private static int GetActiveRentalLimit(UserType userType) =>
        userType switch
        {
            UserType.Student => 2,
            UserType.Employee => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(userType), userType, "This user type is not supported.")
        };
}

public record RentalServiceSummary(
    int TotalUsers,
    int TotalEquipment,
    int AvailableEquipment,
    int UnavailableEquipment,
    int ActiveRentals,
    int OverdueRentals,
    float TotalPenalties);
