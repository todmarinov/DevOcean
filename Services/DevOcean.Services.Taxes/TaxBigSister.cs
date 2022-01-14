namespace DevOcean.Services.Taxes
{
	using System;

	using DevOcean.Common.Extensions;
	using DevOcean.Data.Models.Spaceships;
	using DevOcean.Data.Models.Spaceships.Agreements;
	using DevOcean.Services.Taxes.Agreements;

	public class TaxBigSister : ITaxBigSister
	{
		public TaxBigSister(int yearOfTaxCalculation = 0)
		{
			this.YearOfTaxCalculation = yearOfTaxCalculation;
		}

		private decimal totalTax;
		public decimal TotalTax
		{
			get { return this.totalTax; }
			private set
			{
				this.totalTax = value.NonNegative();
			}
		}

		public int YearOfTaxCalculation { get; set; }

		private void SpaceshipValidation(ISpaceship spaceship)
		{
			if (spaceship == null)
			{
				throw new ArgumentNullException("Spaceship is null!");
			}
		}

		private void YearOfPurchaseValidation(ISpaceship spaceship)
		{
			this.SpaceshipValidation(spaceship);

			if (spaceship.Info == null ||
				spaceship.Info.YearOfPurchase == null)
			{
				throw new ArgumentNullException("Missing year of purchase of the spaceship!");
			}
		}

		private void YearOfPurchaseThenYearOfTaxCalculationValidation(ISpaceship spaceship, int yearOfTaxCalculation)
		{
			this.YearOfPurchaseValidation(spaceship);

			if (yearOfTaxCalculation < spaceship.Info.YearOfPurchase)
			{
				throw new ArgumentOutOfRangeException("The year of tax calculation should be >= the year of purchase of the spaceship!");
			}
		}

		private void TraveledLightMilesValidation(ISpaceship spaceship)
		{
			this.SpaceshipValidation(spaceship);

			if (spaceship.Info == null ||
				spaceship.Info.TraveledLightMiles == null)
			{
				throw new ArgumentNullException("Missing traveled light miles of the spaceship!");
			}
		}

		private decimal GetStandardTax(ISpaceship spaceship, decimal initialTax, decimal traveledTaxBy, ulong onEveryLightMiles, decimal ageTaxBy, int onEveryYears)
		{
			this.YearOfPurchaseThenYearOfTaxCalculationValidation(spaceship, this.YearOfTaxCalculation);
			this.TraveledLightMilesValidation(spaceship);

			decimal traveledTax = traveledTaxBy * ((ulong)spaceship.Info.TraveledLightMiles / onEveryLightMiles);
			decimal ageTax = ageTaxBy * ((this.YearOfTaxCalculation - (int)spaceship.Info.YearOfPurchase) / onEveryYears);
			decimal tax = initialTax + traveledTax + ageTax;

			return tax;
		}

		public void Visit(SpaceshipCargo spaceship)
		{
			var tax = this.GetStandardTax(spaceship, 10_000, 1_000, 1_000, -736, 1);
			this.TotalTax = tax;
		}

		public void Visit(SpaceshipFamily spaceship)
		{
			var tax = this.GetStandardTax(spaceship, 5_000, 100, 1_000, -355, 1);
			this.TotalTax = tax;
		}
	}
}