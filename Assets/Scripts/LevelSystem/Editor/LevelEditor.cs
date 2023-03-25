using UnityEditor;
using UnityEngine;

namespace LevelSystem
{
	public class LevelEditor : EditorWindow
	{
		private static int cellCount;
		private int cellCountField;

		private bool[][] cells = new bool[cellCount][];

		private bool setupClicked;

		private string levelName;

		private const float CELL_SIZE = 25;

		[MenuItem("Nonogram/Level Editor")]
		private static void ShowWindow()
		{
			var window = GetWindow<LevelEditor>();
			window.titleContent = new GUIContent("Level Editor");
			window.Show();
		}

		private void OnGUI()
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Cell Count: ", EditorStyles.label);
			cellCountField = EditorGUILayout.IntField(cellCountField);
			if (GUILayout.Button("Setup"))
			{
				SetupGrid();
				setupClicked = true;
				var size = (cellCount + 2) * CELL_SIZE;
				minSize = new Vector2(size, size + CELL_SIZE);
			}

			GUILayout.EndHorizontal();

			if (cellCount > 0 && setupClicked)
			{
				Grid();

				GUILayout.Space((cellCount + 1) * CELL_SIZE);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Level Name: ");
				levelName = GUILayout.TextField(levelName);
				if (GUILayout.Button("Save"))
				{
					//
					Save();
				}

				GUILayout.EndHorizontal();
			}
		}

		private void SetupGrid()
		{
			cellCount = cellCountField;
			cells = new bool[cellCount][];
			for (int x = 0; x < cellCount; x++)
				cells[x] = new bool[cellCount];
		}

		private void Grid()
		{
			var size = (cellCount + 1) * CELL_SIZE;
			var rect = new Rect(25, 25, size, size);
			GUILayout.BeginArea(rect);

			GUILayout.BeginVertical();
			for (int x = 0; x < cellCount; x++)
			{
				GUILayout.BeginHorizontal();
				for (int y = 0; y < cellCount; y++)
				{
					var color = cells[x][y] ? Texture2D.blackTexture : Texture2D.whiteTexture;
					var buttonContent = new GUIContent();
					var buttonRect = GUILayoutUtility.GetRect(buttonContent, GUIStyle.none, GUILayout.Width(CELL_SIZE), GUILayout.Height(CELL_SIZE));

					GUI.skin.button.normal.background = color;
					if (GUI.Button(buttonRect, buttonContent))
					{
						cells[x][y] = !cells[x][y];

						Repaint();
					}
				}

				GUILayout.EndHorizontal();
			}

			GUI.skin.button.normal.background = default;

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void Save()
		{
			LevelFile.Save(cells, levelName);
		}
	}
}