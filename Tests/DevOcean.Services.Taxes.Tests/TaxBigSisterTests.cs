namespace DevOcean.Services.Taxes.Tests
{
	using System;
	using System.Collections.Generic;

	using NUnit.Framework;

	using DevOcean.Data.Models.Spaceships.Agreements;
	using DevOcean.Services.Taxes.Agreements;
	using DevOcean.Services.Taxes.Tests.Mocks;
	using DevOcean.Services.Taxes.Tests.Mocks.Agreements;

	[TestFixture]
	public class TaxBigSisterTests
	{
		public TaxBigSisterTests()
			: this(new TaxBigSisterSpaceshipMockMoq())
		{
		}

		public TaxBigSisterTests(ITaxBigSisterSpaceshipMock data)
		{
			this.data = data.TaxBigSisterSpaceship;

			this.get = this.data.DelegateByType;
		}

		private ITaxBigSisterSpaceship data;
		private IDictionary<Type, Delegate> get;
		private ITaxBigSister tax;

		[SetUp]
		protected void SetUpBeforeEveryTest()
		{
			this.tax = (ITaxBigSister)this.get[typeof(ITaxBigSister)].DynamicInvoke();
		}

		private ISpaceshipInfo NullSpaceshipInfoShouldThrowException(ITaxBigSister tax, ISpaceship spaceship)
		{
			var saved = spaceship.Info;

			spaceship.Info = null;
			Assert.Throws(
				Is.AssignableTo(typeof(ArgumentNullException)),
				() => spaceship.Accept(tax),
				$"{spaceship.GetType().Name}.{nameof(spaceship.Info)} == null should throw {nameof(ArgumentNullException)}!");

			return saved;
		}

		private void SetNotNullSpaceshipInfo(ISpaceship spaceship, ISpaceshipInfo spaceshipInfo) =>
			spaceship.Info = spaceshipInfo != null ? spaceshipInfo : (ISpaceshipInfo)this.get[typeof(ISpaceshipInfo)].DynamicInvoke();

		[Test]
		public void NullSpaceshipYearOfPurchaseShouldThrowException()
		{
			var spaceships = new List<ISpaceship>()
			{
				(ISpaceship)this.get[typeof(ISpaceshipCargo)].DynamicInvoke(),
				(ISpaceship)this.get[typeof(ISpaceshipFamily)].DynamicInvoke(),
			};

			Assert.Multiple(() =>
			{
				spaceships.ForEach(s =>
				{
					var saved = this.NullSpaceshipInfoShouldThrowException(this.tax, s);

					this.SetNotNullSpaceshipInfo(s, saved);
					s.Info.YearOfPurchase = null;
					Assert.Throws(
						Is.AssignableTo(typeof(ArgumentNullException)),
						() => s.Accept(this.tax),
						$"{s.GetType().Name}.{nameof(s.Info.YearOfPurchase)} == null should throw {nameof(ArgumentNullException)}!");
				});
			});
		}

		[Test]
		public void NullSpaceshipTraveledLightMilesShouldThrowException()
		{
			var spaceships = new List<ISpaceship>()
			{
				(ISpaceship)this.get[typeof(ISpaceshipCargo)].DynamicInvoke(),
				(ISpaceship)this.get[typeof(ISpaceshipFamily)].DynamicInvoke(),
			};

			Assert.Multiple(() =>
			{
				spaceships.ForEach(s =>
				{
					var saved = this.NullSpaceshipInfoShouldThrowException(this.tax, s);

					this.SetNotNullSpaceshipInfo(s, saved);
					s.Info.TraveledLightMiles = null;
					Assert.Throws(
						Is.AssignableTo(typeof(ArgumentNullException)),
						() => s.Accept(this.tax),
						$"{s.GetType().Name}.{nameof(s.Info.TraveledLightMiles)} == null should throw {nameof(ArgumentNullException)}!");
				});
			});
		}

		[Test]
		public void YearOfTaxCalculationBeforeSpaceshipYearOfPurchaseShouldThrowException()
		{
			var spaceships = new List<ISpaceship>()
			{
				(ISpaceship)this.get[typeof(ISpaceshipCargo)].DynamicInvoke(),
				(ISpaceship)this.get[typeof(ISpaceshipFamily)].DynamicInvoke(),
			};

			Assert.Multiple(() =>
			{
				spaceships.ForEach(s =>
				{
					this.SetNotNullSpaceshipInfo(s, s.Info);
					s.Info.YearOfPurchase = 0;
					s.Info.TraveledLightMiles = 0;
					this.tax.YearOfTaxCalculation = -1;
					Assert.Throws(
						Is.AssignableTo(typeof(ArgumentOutOfRangeException)),
						() => s.Accept(this.tax),
						$"{nameof(this.tax.YearOfTaxCalculation)} < {s.GetType().Name}.{nameof(s.Info.YearOfPurchase)} should throw {nameof(ArgumentOutOfRangeException)}!");
				});
			});
		}

		[Test]
		[TestCase(typeof(ISpaceshipCargo), "0", 130, 3, (ulong)7, TestName = nameof(ISpaceshipCargo) + " TotalTax < 0 to 0")]
		[TestCase(typeof(ISpaceshipFamily), "0", 130, 3, (ulong)7, TestName = nameof(ISpaceshipFamily) + " TotalTax < 0 to 0")]
		[TestCase(typeof(ISpaceshipCargo), "2640", 13, 3, (ulong)7, TestName = nameof(ISpaceshipCargo) + " TotalTax >= 0")]
		[TestCase(typeof(ISpaceshipFamily), "1450", 13, 3, (ulong)7, TestName = nameof(ISpaceshipFamily) + " TotalTax >= 0")]
		public void TotalTaxShouldBeRightCalculated(Type typeOfSpaceship, string expectedTotalTaxAsString, int yearOfTaxCalculation, int spaceshipYearOfPurchase, ulong spaceshipTraveledLightMiles)
		{
			var s = (ISpaceship)this.get[typeOfSpaceship].DynamicInvoke();
			this.SetNotNullSpaceshipInfo(s, s.Info);
			s.Info.YearOfPurchase = spaceshipYearOfPurchase;
			s.Info.TraveledLightMiles = spaceshipTraveledLightMiles;
			this.tax.YearOfTaxCalculation = yearOfTaxCalculation;
			s.Accept(this.tax);
			Assert.AreEqual(Convert.ToDecimal(expectedTotalTaxAsString), this.tax.TotalTax, $"Urong {s.GetType().Name} tax calculation!");
		}
	}
}