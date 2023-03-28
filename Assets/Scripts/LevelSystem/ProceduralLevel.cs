using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace LevelSystem
{
	public static class ProceduralLevel
	{
		private static Dictionary<Difficulty, WeightedRandomList<int>> lineAtAxises;

		private static bool hasInitiated;

		private static void Init()
		{
			if (hasInitiated) return;
			hasInitiated = true;
			lineAtAxises = new Dictionary<Difficulty, WeightedRandomList<int>>
			{
				{
					Difficulty.Easy, new WeightedRandomList<int>
					{
						{ 0, 15 },
						{ 1, 50 },
						{ 2, 25 },
						{ 3, 10 },
						{ 4, 5 }
					}
				},
				{
					Difficulty.Medium, new WeightedRandomList<int>
					{
						{ 0, 10 },
						{ 1, 25 },
						{ 2, 35 },
						{ 3, 30 },
						{ 4, 10 }
					}
				},
				{
					Difficulty.Hard, new WeightedRandomList<int>
					{
						{ 0, 5 },
						{ 1, 15 },
						{ 2, 30 },
						{ 3, 40 },
						{ 4, 25 },
						{ 5, 10 }
					}
				}
			};
		}

		public static bool[][] GenerateLevel(Difficulty difficulty, int gridSize)
		{
			Init();
			var diff = lineAtAxises[difficulty];

			var grid = new bool[gridSize][];

			for (int x = 0; x < gridSize; x++)
			{
				grid[x] = new bool[gridSize];

				int lineCount = Mathf.Clamp(diff.Next(), 0, Mathf.CeilToInt(gridSize / 2f));
				int maxLineLength = Mathf.FloorToInt(gridSize - (lineCount - 1) * 2);
				var lines = new List<int>();
				int totalLineLength = 0;
				for (int i = 0; i < lineCount; i++)
				{
					int length = Random.Range(1, maxLineLength);
					lines.Add(length);
					totalLineLength += length;
				}

				int emptyLineLength = gridSize - totalLineLength;

				for (int y = 0; y < gridSize; y++)
				{
					if (lines.Count <= 0) break;
					
					if (y > 0)
					{
						if (lines[0] > 0)
						{
							if (grid[x][y - 1])
							{
								grid[x][y] = true;
								lines[0]--;
							}
							else
							{
								StartALine(ref grid, x, y, ref lines, ref emptyLineLength);
							}
						}
						else
						{
							grid[x][y] = false;
							emptyLineLength--;
							lines.RemoveAt(0);
						}
					}
					else
					{
						StartALine(ref grid, x, y, ref lines, ref emptyLineLength);
					}
				}
			}

			return grid;
		}

		private static void StartALine(ref bool[][] grid, int x, int y, ref List<int> lines, ref int emptyLineLength)
		{
			bool isLine = Random.Range(0, 2) != 0;
			if (isLine)
			{
				grid[x][y] = true;
				lines[0]--;
			}
			else
			{
				grid[x][y] = false;
				emptyLineLength--;
			}
		}
	}
}