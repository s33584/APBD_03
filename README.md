# APBD_03

## Project Instructions

-  Open `APBD_03/APBD_03` folder and run `dotnet run` in its root. It will run all the test needed.



## Design Decisions (Short Justification)

- Each file was placed in a specific folder to make it easy to understand the structure.

- `RentalService` has all the business logic and rule checks.

- `Repository` stores all the data. It isolates the storage logic from everything else.

- Model classes (`Camera`, `Laptop`, `Projector`) all inherit from `Equipment`, so that they present the same type of objects. This makes code easy to change later.


## Cohesion, Coupling, and Class Responsibilities

### Cohesion

- `RentalService.cs`:
  all rental rules are placed in one class (limits, availability checks, returns, reports).
- `Rental.cs`:
  all rental-specific fields and return behavior is kept inside `Rental` class.
- `Equipment.cs`:
  shared states (status change methods `MarKAsRented` and `MarkAsReturned`) are placed in one class.

### Coupling

- `RentalService.cs`depends on `Repository.cs` for accessing the data, rather than changing it from outer scope.
- `Program.cs` runs tests, prints but all rule decisions are onto `RentalService`.

### Class Responsibilities (clear ownership)

- `User.cs`: only stores user id, full name, and type.
- `Equipment.cs`: stores shared equipment data + defines shared status change methods.
- `Laptop.cs`, `Projector.cs`, `Camera.cs`: only define equipment-specific fields.
- `Rental.cs`: responsible for calculating penalty on return and storing rental lifecycle logic.
- `Repository.cs`: storing data with no direct access. Is responsible for Read operations.
- `RentalService.cs`: stores all business processes and validation.
- `Program.cs`: test case execution.

### Justification

Each class has a single responsibility connected with its type.
As all the fields have getters and no setters, user cannot modify it without class method.


## Organization of Classes, Files, and Layers

The project is organized by responsibility:

- `User/`, `Equipment/`, `Rental/` - main entities and enums.
- `Models/` - equipment types.
- `Repository/` - data storage.
- `Services/` - business logic and business operations.
- `Program.cs` - test case.

It is much more readable than putting all the files in one folder. I grouped files into folders by their responsibility.