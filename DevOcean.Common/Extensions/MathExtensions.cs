namespace DevOcean.Common.Extensions
{
	public static class MathExtensions
	{
		public static decimal NonNegative(this decimal number)
		{
			return 0 <= number ? number : 0;
		}
	}
}