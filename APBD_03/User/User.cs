namespace APBD_03.Domain;

public enum UserType
{
    Student = 1,
    Employee = 2
}

public sealed class User
{
    public User(string firstName, string lastName, UserType userType)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("User first name is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("User last name is required.");
        }

        Id = Guid.NewGuid();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UserType = userType;
    }

    public Guid Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public UserType UserType { get; }
}
