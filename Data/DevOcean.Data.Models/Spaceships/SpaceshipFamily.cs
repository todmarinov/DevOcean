namespace DevOcean.Data.Models.Spaceships
{
	using DevOcean.Data.Models.Spaceships.Agreements;

	public class SpaceshipFamily : Spaceship, ISpaceshipFamily
	{
		public override void Accept(ISpaceshipVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}