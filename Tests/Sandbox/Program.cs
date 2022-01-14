namespace Sandbox
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Microsoft.Extensions.DependencyInjection;

	using DevOcean.Data.Models.Spaceships;
	using DevOcean.Data.Models.Spaceships.Agreements;
	using DevOcean.Services.Taxes;
	using DevOcean.Services.Taxes.Agreements;

	public class Program
	{
		public static void Main()
		{
			IServiceCollection serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);
			IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

			BigSisterSpaceshipTaxCalculator(serviceProvider);
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddTransient<ISpaceshipInfo, SpaceshipInfo>()
				.AddTransient<ISpaceshipCargo>(sp => new SpaceshipCargo() { Info = sp.GetService<ISpaceshipInfo>() })
				.AddTransient<ISpaceshipFamily>(sp => new SpaceshipFamily() { Info = sp.GetService<ISpaceshipInfo>() })
				.AddTransient<ITaxBigSister, TaxBigSister>();
		}

		private static void BigSisterSpaceshipTaxCalculator(IServiceProvider serviceProvider)
		{
			do
			{
				StringBuilder view = new StringBuilder();
				view.AppendLine();
				view.AppendLine($"   |----------------------------------------|");
				view.AppendLine($"   |   BigSister spaceship tax calculator   |");
				view.AppendLine($"   |----------------------------------------|");
				view.AppendLine();

				try
				{
					var spaceship = InputSpaceship(view, serviceProvider);
					spaceship.Info = spaceship.Info != null ? spaceship.Info : serviceProvider.GetService<ISpaceshipInfo>();
					spaceship.Info.YearOfPurchase = InputSpaceshipYearOfPurchase(view);
					spaceship.Info.TraveledLightMiles = InputSpaceshipTraveledLightMiles(view);

					view.Append($"Year of tax calculation = ");

					do
					{
						PrintView(view);

						try
						{
							var spaceshipTaxVisitor = serviceProvider.GetService<ITaxBigSister>();
							spaceshipTaxVisitor.YearOfTaxCalculation = InputYearOfTaxCalculation(view);

							spaceship.Accept(spaceshipTaxVisitor);

							view.AppendLine(spaceshipTaxVisitor.YearOfTaxCalculation.ToString());
							view.AppendLine($"BigSister tax = {spaceshipTaxVisitor.TotalTax}");

							break;
						}
						catch (Exception exception)
						{
							Console.WriteLine(exception.Message);
							Console.Write($"Press any key to input valid data...");
							Console.ReadLine();
						}
					} while (true);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Message);
				}

				view.Append($"\r\nPress 'Enter' to make another check...");
				PrintView(view);
				Console.ReadLine();
			} while (true);
		}

		private static void PrintView(StringBuilder view)
		{
			Console.Clear();
			Console.Write(view.ToString());
		}

		private static ISpaceship InputSpaceship(StringBuilder view, IServiceProvider serviceProvider)
		{
			var availableSpaceshipTypes = new Dictionary<string, Func<ISpaceship>>();
			availableSpaceshipTypes.Add("Cargo".Trim(), () => serviceProvider.GetService<ISpaceshipCargo>());
			availableSpaceshipTypes.Add("Family".Trim(), () => serviceProvider.GetService<ISpaceshipFamily>());

			view.Append($"Spaceship type = ");
			do
			{
				PrintView(view);
				var input = Console.ReadLine().Trim();
				if (availableSpaceshipTypes.ContainsKey(input))
				{
					view.AppendLine($"{input}");
					return availableSpaceshipTypes[input].Invoke();
				}

				Console.Write($"Available spaceship types are: {string.Join(", ", availableSpaceshipTypes.Keys.ToList().OrderBy(t => t))}");
				Console.ReadLine();
			} while (true);
		}

		private static int InputSpaceshipYearOfPurchase(StringBuilder view)
		{
			view.Append($"Year of purchase = ");
			do
			{
				PrintView(view);
				var input = Console.ReadLine().Trim();
				if (int.TryParse(input, out var yearOfPurchase))
				{
					view.AppendLine($"{input}");
					return yearOfPurchase;
				}

				Console.Write($"The year of purchase should be integer of [{int.MinValue}, {int.MaxValue}]");
				Console.ReadLine();
			} while (true);
		}

		private static ulong InputSpaceshipTraveledLightMiles(StringBuilder view)
		{
			view.Append($"Traveled light miles = ");
			do
			{
				PrintView(view);
				var input = Console.ReadLine().Trim();
				if (ulong.TryParse(input, out var traveledLightMiles))
				{
					view.AppendLine($"{input}");
					return traveledLightMiles;
				}

				Console.Write($"Traveled light miles should be integer of [{ulong.MinValue}, {ulong.MaxValue}]");
				Console.ReadLine();
			} while (true);
		}

		private static int InputYearOfTaxCalculation(StringBuilder view)
		{
			do
			{
				PrintView(view);
				var input = Console.ReadLine().Trim();
				if (int.TryParse(input, out var yearOfTaxCalculation))
				{
					return yearOfTaxCalculation;
				}

				Console.WriteLine($"The year of tax calculation should be integer of [{int.MinValue}, {int.MaxValue}]");
				Console.ReadLine();
			} while (true);
		}
	}
}