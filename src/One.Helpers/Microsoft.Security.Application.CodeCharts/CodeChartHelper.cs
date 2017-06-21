using System;
using System.Collections.Generic;
using System.Linq;
namespace Microsoft.Security.Application.CodeCharts
{
	internal static class CodeChartHelper
	{
		internal static IEnumerable<int> GetRange(int min, int max, Func<int, bool> exclusionFilter)
		{
			IEnumerable<int> enumerable = Enumerable.Range(min, max - min + 1);
			if (exclusionFilter != null)
			{
				enumerable = 
					from i in enumerable
					where !exclusionFilter(i)
					select i;
			}
			return enumerable;
		}
		internal static IEnumerable<int> GetRange(int min, int max)
		{
			return CodeChartHelper.GetRange(min, max, null);
		}
	}
}
