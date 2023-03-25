using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.JsonUtility;

namespace LevelSystem
{
	public static class LevelSystem
	{
		private const string PATH = "/Levels/Resources/";
		private const string LEVEL_NAME = "Level";
		private const char SEPARATOR = '_';

		public static async Task Save(bool[][] cells, int levelNo)
		{
			string path = Application.dataPath + PATH + LEVEL_NAME + SEPARATOR + levelNo;
			Debug.Log(path);
			if (!Directory.Exists(Application.dataPath + PATH))
			{
				Debug.Log("asd");
				return;
			}

			if (File.Exists(path))
			{
				Debug.LogError("A level with this number exists");
				return;
			}

			var level = new LevelFile(cells);
			var levelJson = ToJson(level);

			await SaveFileAsync(path, levelJson);
		}

		private static async Task SaveFileAsync(string path, string level)
		{
			await File.WriteAllTextAsync(path, level);
			Debug.Log(path + " Saved!");
		}

		public static async Task<bool[][]> Load(int levelNo)
		{
			string path = PATH + LEVEL_NAME + SEPARATOR + levelNo;
			if (!File.Exists(path))
			{
				Debug.LogError(path + " does not exists!");
				return null;
			}

			var levelJson = await File.ReadAllTextAsync(path);

			LevelFile levelFile = FromJson<LevelFile>(levelJson);
			int count = levelFile.CellCount;
			var result = new bool[count][];

			for (var x = 0; x < count; x++)
			{
				result[x] = new bool[count];
				for (var y = 0; y < count; y++)
					result[x][y] = levelFile.Cells[y * count + x];
			}

			return result;
		}
	}

	public class LevelFile
	{
		public bool[] Cells;
		public int CellCount;

		public LevelFile(bool[][] cells)
		{
			CellCount = cells.GetLength(0);
			var j = 0;
			Cells = new bool[CellCount * CellCount];

			for (var y = 0; y < CellCount; y++)
				for (var x = 0; x < CellCount; x++)
					Cells[j++] = cells[x][y];
		}
	}
}