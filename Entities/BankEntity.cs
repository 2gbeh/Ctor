namespace Ctor.Entities;

public class BankEntity
{
  public string? AccountName { get; set; }
  public string? AccountNumber { get; set; }
  public AccountType AccountType { get; set; } = AccountType.Savings;
  public string? BankName { get; set; }
  public string? SortCode { get; set; }
  public string? CurrencyName { get; set; }
  public string? CurrencyShortName { get; set; }
  public string? CurrencySymbol { get; set; }
  public string? Country { get; set; }
}

public enum AccountType
{
  Savings,
  Current,
  Domiciliary,
}