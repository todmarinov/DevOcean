namespace DevOcean.Services.Taxes.Tests.Mocks.Agreements
{
	using System;
	using System.Collections.Generic;

	public interface ITaxBigSisterSpaceship
	{
		IDictionary<Type, Delegate> DelegateByType { get; }
	}
}