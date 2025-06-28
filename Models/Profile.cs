using Ctor.Entities;

namespace Ctor.Models;

public class Profile
{
  public DateOnly? DateOfBirth { get; set; }
  public TelephoneEntity? Telephone { get; set; } = new();
  public AddressEntity? Address { get; set; } = new();
  public BankEntity? BankInfo { get; set; } = new();
}
