namespace DevOcean.Data.Models.Spaceships
{
	using DevOcean.Data.Models.Spaceships.Agreements;

	public abstract class Spaceship : ISpaceship
	{
		public ISpaceshipInfo Info { get; set; }

		public abstract void Accept(ISpaceshipVisitor visitor);
	}
}