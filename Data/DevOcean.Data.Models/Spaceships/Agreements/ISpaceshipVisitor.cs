namespace DevOcean.Data.Models.Spaceships.Agreements
{
    using DevOcean.Data.Models.Spaceships;

    public interface ISpaceshipVisitor
    {
        void Visit(SpaceshipCargo spaceship);

        void Visit(SpaceshipFamily spaceship);
    }
}