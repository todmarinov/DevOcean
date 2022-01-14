namespace DevOcean.Data.Models.Spaceships
{
	using DevOcean.Data.Models.Spaceships.Agreements;

	public class SpaceshipCargo : Spaceship, ISpaceshipCargo
	{
		public override void Accept(ISpaceshipVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}