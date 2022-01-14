namespace DevOcean.Data.Models.Spaceships.Agreements
{
	public interface ISpaceshipInfo
	{
		int? YearOfPurchase { get; set; }

		ulong? TraveledLightMiles { get; set; }
	}
}