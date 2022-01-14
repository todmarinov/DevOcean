namespace DevOcean.Services.Taxes.Tests.Mocks
{
	using Moq;

	using DevOcean.Services.Taxes.Tests.Mocks.Agreements;

	public class TaxBigSisterSpaceshipMockMoq : TaxBigSisterSpaceshipMock
	{
		protected override void SetMock()
		{
			var mockedSpaceships = new Mock<ITaxBigSisterSpaceship>();

			mockedSpaceships.Setup(r => r.DelegateByType).Returns(this.DelegateByTypeFake);

			this.TaxBigSisterSpaceship = mockedSpaceships.Object;
		}
	}
}