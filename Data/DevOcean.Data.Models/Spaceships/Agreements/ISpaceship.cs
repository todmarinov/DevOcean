namespace DevOcean.Data.Models.Spaceships.Agreements
{
	public interface ISpaceship : ISpaceshipVisitable
	{
		ISpaceshipInfo Info { get; set; }
	}
}