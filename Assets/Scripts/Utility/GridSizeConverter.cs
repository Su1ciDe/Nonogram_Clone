using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Utility
{
	public static class GridSizeConverter
	{
		private static readonly ReadOnlyDictionary<int, int> GridSizeToInt = new(new Dictionary<int, int>
		{
			{ (int)GridSize._5x5, 5 },
			{ (int)GridSize._10x10, 10 },
			{ (int)GridSize._15x15, 15 },
			{ (int)GridSize._20x20, 20 },
			{ (int)GridSize._25x25, 15 }
		});

		public static int ToInt(this GridSize gridSizeEnum)

		{
			return GridSizeToInt[(int)gridSizeEnum];
		}
	}
}