namespace DevOcean.Data.Models.Spaceships.Agreements
{
    public interface ISpaceshipVisitable
    {
        void Accept(ISpaceshipVisitor visitor);
    }
}