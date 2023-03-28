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

		[Header("Prefabs")]
		[SerializeField] private GridCell cellPrefab;
		[Space]
		[SerializeField] private RectTransform numberRow;
		[SerializeField] private RectTransform numberColumn;
		[Space]
		[SerializeField] private SideNumber sideNumberPrefab;

		[Header("Layout")]
		[SerializeField] private GridLayoutGroup gridLayout;
		[Space]
		[SerializeField] private RectTransform leftNumberPanel;
		[SerializeField] private RectTransform topNumberPanel;

		private float offset;
		private Camera cam => GameManager.Instance.MainCamera;

		private void Awake()
		{
			CellStateCount = System.Enum.GetNames(typeof(CellState)).Length;
			LevelManager.OnLevelLoad += OnLevelLoaded;
			LevelManager.OnLevelUnload += OnLevelUnloaded;
		}

		private void OnDestroy()
		{
			LevelManager.OnLevelLoad -= OnLevelLoaded;
			LevelManager.OnLevelUnload -= OnLevelUnloaded;
			foreach (var cell in Cells.Values)
				cell.OnStateChanged -= OnCellStateChanged;
		}

		private void OnLevelLoaded(Level level)
		{
			CellCount = level.Cells.GetLength(0);
			CreateGrid(level.Cells);
		}

		private void OnLevelUnloaded()
		{
			ClearGrid();
		}

		#region Grid Generation

		public void CreateGrid(bool[][] cells)
		{
			Cells = new Dictionary<Vector2Int, GridCell>(CellCount * CellCount);

			float cellScale = gridLayout.GetComponent<RectTransform>().rect.width / CellCount;
			gridLayout.cellSize = cellScale * Vector2.one;
			gridLayout.constraintCount = CellCount;

			for (int x = 0; x < CellCount; x++)
			{
				var rowNumbers = new List<int>();
				var columnNumbers = new List<int>();

				for (int y = 0; y < CellCount; y++)
				{
					var cell = CreateCell(x, y);
					cell.CorrectState = cells[x][y] ? CellState.Filled : CellState.Empty;
					Cells[new Vector2Int(x, y)] = cell;

					cell.OnStateChanged += OnCellStateChanged;

					if (y > 0) //row
					{
						if (cells[x][y])
						{
							if (cells[x][y - 1])
								rowNumbers[^1]++;
							else
								rowNumbers.Add(1);
						}

						if (cells[y][x])
						{
							if (cells[y - 1][x])
								columnNumbers[^1]++;
							else
								columnNumbers.Add(1);
						}
					}
					else
					{
						if (cells[x][y])
							rowNumbers.Add(1);
						if (cells[y][x])
							columnNumbers.Add(1);
					}

					if (x > 0) //column
					{
					}
					else
					{
					}
				}

				// row number generation 
				numberRow.sizeDelta = new Vector2(numberRow.sizeDelta.x, cellScale);
				var row = Instantiate(numberRow, leftNumberPanel);
				int rowNumberCount = rowNumbers.Count;
				if (rowNumberCount.Equals(0))
				{
					CreateNumber(0, row);
				}
				else
				{
					for (int i = 0; i < rowNumberCount; i++)
						CreateNumber(rowNumbers[i], row);
				}

				// column number generation 
				numberColumn.sizeDelta = new Vector2(cellScale, numberColumn.sizeDelta.y);
				var column = Instantiate(numberColumn, topNumberPanel);
				int columnNumberCount = columnNumbers.Count;
				if (columnNumberCount.Equals(0))
				{
					CreateNumber(0, column);
				}
				else
				{
					for (int i = 0; i < columnNumberCount; i++)
						CreateNumber(columnNumbers[i], column);
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

		private void CreateNumber(int _number, RectTransform parent)
		{
			var sideNumber = Instantiate(sideNumberPrefab, parent);
			sideNumber.SetNumber(_number);
		}

		private void ClearGrid()
		{
			foreach (var cell in Cells.Values)
			{
				cell.OnStateChanged -= OnCellStateChanged;
				Destroy(cell.gameObject);
			}

			foreach (Transform leftNumbers in leftNumberPanel)
				Destroy(leftNumbers.gameObject);
			foreach (Transform topNumbers in topNumberPanel)
				Destroy(topNumbers.gameObject);
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