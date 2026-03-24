namespace APBD_03.Domain;

public class Rental
{
    private const float PenalryPerDay = 10.0f;

    public Rental(User user, Equipment equipment, DateTime rentedDateTime, int rentalDays)
    {
        if (rentalDays <= 0)
        {
            throw new ArgumentException("Rental days must be greater than zero.");
        }

        User = user ?? throw new ArgumentNullException("User must be non null");
        Equipment = equipment ?? throw new ArgumentNullException("Equipment must be non null");
        RentedDateTime = rentedDateTime;
        DueDateTime = rentedDateTime.AddDays(rentalDays);
        ReturnedDateTime = null;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public User User { get; }

    public Equipment Equipment { get; }

    public DateTime RentedDateTime { get; }

    public DateTime DueDateTime { get; }

    public DateTime? ReturnedDateTime { get; private set; }

    public float PenaltyAmount { get; private set; }

    public bool IsReturned => ReturnedDateTime.HasValue;

    public bool IsOverdue(DateTime actualReturnDateTime) => !IsReturned && actualReturnDateTime > DueDateTime;

    public void Return(DateTime returnedDateTime)
    {
        if (IsReturned)
        {
            throw new InvalidOperationException("Rental has already been returned.");
        }

        if (returnedDateTime < RentedDateTime)
        {
            throw new ArgumentException("Return date cannot be earlier than rental date.");
        }

        ReturnedDateTime = returnedDateTime;
        var daysLate = (returnedDateTime.Date - DueDateTime.Date).Days;
        PenaltyAmount = daysLate > 0 ? daysLate * PenalryPerDay : 0.0f;
    }
}
