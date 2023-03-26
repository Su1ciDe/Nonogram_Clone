using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.JsonUtility;

namespace LevelSystem
{
	public static class LevelSystem
	{
		private const string PATH = "/Resources/";
		private const string LEVEL_FOLDER = "Levels/";
		private const string LEVEL_NAME = "Level";
		private const char SEPARATOR = '_';
		private const string EXTENSION = ".json";

		public static async Task Save(bool[][] cells, int levelNo)
		{
			string path = Application.dataPath + PATH + LEVEL_FOLDER + LEVEL_NAME + SEPARATOR + levelNo + EXTENSION;
			if (!Directory.Exists(Application.dataPath + PATH))
				Directory.CreateDirectory(Application.dataPath + PATH);

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
			try
			{
				await File.WriteAllTextAsync(path, level);
				Debug.Log(path + " Saved!");
			}
			catch (IOException e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static bool[][] Load(int levelNo)
		{
			string path = LEVEL_FOLDER + LEVEL_NAME + SEPARATOR + levelNo;

			var resource = Resources.LoadAsync<TextAsset>(path);
			var levelJson = ((TextAsset)resource.asset).text;

			var levelFile = FromJson<LevelFile>(levelJson);
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