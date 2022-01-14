namespace DevOcean.Services.Taxes.Agreements
{
	using DevOcean.Data.Models.Spaceships.Agreements;

	public interface ITaxSpaceship : ITax, ISpaceshipVisitor
	{
	}
}