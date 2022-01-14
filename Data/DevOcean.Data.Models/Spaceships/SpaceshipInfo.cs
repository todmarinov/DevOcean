namespace DevOcean.Data.Models.Spaceships
{
	using DevOcean.Data.Models.Spaceships.Agreements;

	public class SpaceshipInfo : ISpaceshipInfo
	{
		//public SpaceshipInfo(int? yearOfPurchase = null, ulong? traveledLightMiles = null)
		//{
		//	this.YearOfPurchase = yearOfPurchase;
		//	this.TraveledLightMiles = traveledLightMiles;
		//}

		public int? YearOfPurchase { get; set; }

		public ulong? TraveledLightMiles { get; set; }
	}
}