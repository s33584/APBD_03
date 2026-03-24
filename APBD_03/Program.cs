using APBD_03.Main;
using APBD_03.Models;
using APBD_03.Services;

namespace APBD_03;

public static class Program
{
    private static void Main()
    {
        //prepare data
        var utc = DateTimeKind.Utc;
        var date = new DateTime(2025, 3, 1, 9, 0, 0, utc);

        var repository = new Repository();
        var rentalService = new RentalService(repository);

        Console.WriteLine("=== TEST_MODELS ===");
        var laptop1 = new Laptop("laptop1", "test_laptop", "1920x1080", 16);
        var laptop2 = new Laptop("laptop2", "test_laptop", "1920x1080", 8);
        var projector1 = new Projector("projector1", "test_projector", 20, 90);
        var camera1 = new Camera("camera1", "test_camera", 2.0f, 10);
        var projector2 = new Projector("projector2", "test_projector", 9030, 789);

        rentalService.AddEquipment(laptop1);
        rentalService.AddEquipment(laptop2);
        rentalService.AddEquipment(projector1);
        rentalService.AddEquipment(camera1);
        rentalService.AddEquipment(projector2);
        rentalService.MarkEquipmentUnavailable(projector2.Id);
        Console.WriteLine("Added 5 items");
        Console.WriteLine();


        //run tests
        Console.WriteLine("=== TEST_USERS ===");
        var student1 = new User("student1", "Student", UserType.Student);
        var student2 = new User("student2", "Student", UserType.Student);
        var employee1 = new User("employee1", "Employee", UserType.Employee);
        var employee2 = new User("employee2", "Employee", UserType.Employee);
        rentalService.AddUser(student1);
        rentalService.AddUser(student2);
        rentalService.AddUser(employee1);
        rentalService.AddUser(employee2);
        Console.WriteLine("Added 4 users");
        Console.WriteLine();


        Console.WriteLine("=== TEST_ERRORS ===");
        Console.WriteLine("Renting unavailable item");
        try
        {
            rentalService.RentEquipment(student1.Id, projector2.Id, 3, date);
            Console.WriteLine("Should not be reached (if you are reading this text, than the programmer did a great job coding this project...)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Passed");
        }
        Console.WriteLine();


        Console.WriteLine("=== TEST_VALID ===");
        var employee1LaptopRental = rentalService.RentEquipment(employee1.Id, laptop1.Id, 5, date);
        Console.WriteLine(
            $"Rented '{laptop1.Name}' till '{employee1LaptopRental.DueDateTime}'");
        Console.WriteLine();


        Console.WriteLine("=== TEST_INVALID ===");
        var student1Rental1 = rentalService.RentEquipment(student1.Id, laptop2.Id, 4, date);
        var student1Rental2 = rentalService.RentEquipment(student1.Id, projector1.Id, 4, date);
        Console.WriteLine("Renting more than 2 items for student");
        try
        {
            rentalService.RentEquipment(student1.Id, camera1.Id, 2, date);
            Console.WriteLine("Should not be reached (if you are reading this... this is sad)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Passed");
        }
        Console.WriteLine();


        Console.WriteLine("=== TEST_RETURN ===");
        var onTimeReturnDate = date.AddDays(5);
        rentalService.ReturnEquipment(employee1LaptopRental.Id, onTimeReturnDate);
        Console.WriteLine($"Returned '{laptop1.Name}' on '{onTimeReturnDate}' with penalty: '{employee1LaptopRental.PenaltyAmount}'");
        Console.WriteLine();


        Console.WriteLine("=== TEST_LATE_RETURN ===");
        var lateRental = rentalService.RentEquipment(employee1.Id, camera1.Id, 2, date);
        var lateReturnDate = date.AddDays(7);
        rentalService.ReturnEquipment(lateRental.Id, lateReturnDate);
        Console.WriteLine($"Returned '{camera1.Name}' on '{lateReturnDate}' with due '{lateRental.DueDateTime}' with penalty: '{lateRental.PenaltyAmount}'");
        Console.WriteLine();


        Console.WriteLine("=== TEST_REPORT ===");
        PrintSummary(rentalService.GetSummary(date.AddDays(10)));
        Console.WriteLine();
        foreach (var r in rentalService.GetActiveRentalsForUser(student1.Id))
        {
            Console.WriteLine($"'{r.Equipment.Name}' until '{r.DueDateTime}'");
        }
    }

    private static void PrintSummary(RentalServiceSummary s)
    {
        Console.WriteLine($"Users registered:\t\t{s.TotalUsers}");
        Console.WriteLine($"Equipment items:\t\t{s.TotalEquipment}");
        Console.WriteLine($"Available:\t\t\t{s.AvailableEquipment}");
        Console.WriteLine($"Unavailable:\t\t\t{s.UnavailableEquipment}");
        Console.WriteLine($"Active rentals:\t\t\t{s.ActiveRentals}");
        Console.WriteLine($"Overdue rentals (as of now):\t{s.OverdueRentals}");
        Console.WriteLine($"Total penalties collected:\t{s.TotalPenalties:0.##}");
    }
}
