namespace DevOcean.Services.Taxes.Tests.Mocks
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Extensions.DependencyInjection;

	using DevOcean.Data.Models.Spaceships;
	using DevOcean.Data.Models.Spaceships.Agreements;
	using DevOcean.Services.Taxes;
	using DevOcean.Services.Taxes.Agreements;
	using DevOcean.Services.Taxes.Tests.Mocks.Agreements;

	public abstract class TaxBigSisterSpaceshipMock : ITaxBigSisterSpaceshipMock
	{
		public TaxBigSisterSpaceshipMock()
		{
			this.ConfigureServices();
			this.SetDelegateByTypeFake();
			this.SetMock();
		}

		public ITaxBigSisterSpaceship TaxBigSisterSpaceship { get; protected set; }

		protected IServiceCollection ServiceCollection { get; set; }
		protected IServiceProvider ServiceProvider { get; set; }

		private void ConfigureServices()
		{
			this.ServiceCollection = new ServiceCollection()
					.AddTransient<ISpaceshipInfo, SpaceshipInfo>()
					.AddTransient<ISpaceshipCargo>(sp => new SpaceshipCargo() { Info = sp.GetService<ISpaceshipInfo>() })
					.AddTransient<ISpaceshipFamily>(sp => new SpaceshipFamily() { Info = sp.GetService<ISpaceshipInfo>() })
					.AddTransient<ITaxBigSister, TaxBigSister>();

			this.ServiceProvider = this.ServiceCollection.BuildServiceProvider();
		}

		protected IDictionary<Type, Delegate> DelegateByTypeFake { get; set; }

		protected delegate ITaxBigSister GetTaxBigSisterFakeDelegate();
		protected ITaxBigSister GetTaxBigSisterFake() => this.ServiceProvider.GetService<ITaxBigSister>();

		protected delegate ISpaceshipInfo GetSpaceshipInfoFakeDelegate();
		protected ISpaceshipInfo GetSpaceshipInfoFake() => this.ServiceProvider.GetService<ISpaceshipInfo>();

		protected delegate ISpaceshipCargo GetSpaceshipCargoFakeDelegate();
		protected ISpaceshipCargo GetSpaceshipCargoFake() => this.ServiceProvider.GetService<ISpaceshipCargo>();

		protected delegate ISpaceshipFamily GetSpaceshipFamilyFakeDelegate();
		protected ISpaceshipFamily GetSpaceshipFamilyFake() => this.ServiceProvider.GetService<ISpaceshipFamily>();

		protected void SetDelegateByTypeFake()
		{
			this.DelegateByTypeFake = new Dictionary<Type, Delegate>();

			this.DelegateByTypeFake.Add(typeof(ITaxBigSister), (GetTaxBigSisterFakeDelegate)this.GetTaxBigSisterFake);

			this.DelegateByTypeFake.Add(typeof(ISpaceshipInfo), (GetSpaceshipInfoFakeDelegate)this.GetSpaceshipInfoFake);

			this.DelegateByTypeFake.Add(typeof(ISpaceshipCargo), (GetSpaceshipCargoFakeDelegate)this.GetSpaceshipCargoFake);
			this.DelegateByTypeFake.Add(typeof(ISpaceshipFamily), (GetSpaceshipFamilyFakeDelegate)this.GetSpaceshipFamilyFake);
		}

		protected abstract void SetMock();
	}
}