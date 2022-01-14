namespace DevOcean.Services.Taxes.Agreements
{
	public interface ITaxBigSister : ITaxSpaceship
	{
		int YearOfTaxCalculation { get; set; }
	}
}