using System;
using System.Collections.Generic;
using LevelSystem;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Grid
{
	public class Grid : Singleton<Grid>
	{
		public Dictionary<Vector2Int, GridCell> Cells { get; set; } = new Dictionary<Vector2Int, GridCell>();
		public int CellStateCount { get; private set; }
		public int CellCount { get; set; }

		[SerializeField] private GridCell cellPrefab;
		[SerializeField] private GridLayoutGroup gridLayout;

		private float offset;
		private Camera cam => GameManager.Instance.MainCamera;

		private void Awake()
		{
			CellStateCount = Enum.GetNames(typeof(CellState)).Length;
		}

		private void OnEnable()
		{
			LevelManager.OnLevelLoad += OnLevelLoaded;
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
			foreach (var cell in Cells.Values)
				cell.OnStateChanged -= OnCellStateChanged;
		}

		private void OnLevelLoaded(Level level)
		{
			CellCount = level.Cells.GetLength(0);
			CreateGrid(level.Cells);
		}

		#region Grid Generation

		public void CreateGrid(bool[][] cells)
		{
			ClearAllCells();

			Cells = new Dictionary<Vector2Int, GridCell>(CellCount * CellCount);

			float cellScale = gridLayout.GetComponent<RectTransform>().rect.width / CellCount;
			gridLayout.cellSize = cellScale * Vector2.one;
			gridLayout.constraintCount = CellCount;

			for (int x = 0; x < CellCount; x++)
			{
				for (int y = 0; y < CellCount; y++)
				{
					var cell = CreateCell(x, y);
					cell.CorrectState = cells[x][y] ? CellState.Filled : CellState.Empty;
					Cells[new Vector2Int(x, y)] = cell;

					cell.OnStateChanged += OnCellStateChanged;
				}
			}
		}

		private GridCell CreateCell(int x, int y)
		{
			var cell = Instantiate(cellPrefab, gridLayout.transform);
			cell.X = x;
			cell.Y = y;
			cell.gameObject.name = "Cell_" + x + "x" + y;
			return cell;
		}

		private void ClearAllCells()
		{
			foreach (var cell in Cells.Values)
			{
				cell.OnStateChanged -= OnCellStateChanged;
				Destroy(cell.gameObject);
			}
		}

		#endregion

		private void OnCellStateChanged(GridCell changedCell)
		{
			foreach (var gridCell in Cells.Values)
			{
				if (!gridCell.IsCorrect) return;
			}

			GameManager.Instance.Win();
		}

		public GridCell GetCell(Vector2Int position) => Cells.TryGetValue(position, out var cell) ? cell : null;

		private struct Direction
		{
			public static Vector2Int up => new Vector2Int(0, -1);
			public static Vector2Int down => new Vector2Int(0, 1);
			public static Vector2Int right => new Vector2Int(1, 0);
			public static Vector2Int left => new Vector2Int(-1, 0);
		}
	}
}