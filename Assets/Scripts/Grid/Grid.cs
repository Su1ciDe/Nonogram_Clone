using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Grid
{
	public class Grid : Singleton<Grid>
	{
		public Dictionary<Vector2Int, GridCell> Cells { get; set; } = new Dictionary<Vector2Int, GridCell>();
		public int CellStateCount;

		public int CellCount = 3;
		[SerializeField] private GridCell cellPrefab;
		[SerializeField] private GridLayoutGroup gridLayout;

		private float offset;
		private Camera cam => GameManager.Instance.MainCamera;

		private void Awake()
		{
			CellStateCount = Enum.GetNames(typeof(CellState)).Length;
		}

		private void Start()
		{
			CreateGrid();
		}

		private void OnDestroy()
		{
			foreach (var cell in Cells.Values)
				cell.OnStateChanged -= OnCellStateChanged;
		}

		#region Grid Generation

		public void CreateGrid()
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
					Cells[new Vector2Int(x, y)] = cell;

					cell.OnStateChanged += OnCellStateChanged;
				}
			}
		}

		private GridCell CreateCell(int x, int y)
		{
			var cell = Instantiate(cellPrefab, transform);
			cell.X = x;
			cell.Y = y;
			cell.gameObject.name = "Cell_" + x + "x" + y;
			return cell;
		}

		private void ClearAllCells()
		{
#if UNITY_EDITOR
			while (transform.childCount > 0)
				DestroyImmediate(transform.GetChild(0).gameObject);
#else
			foreach (var cell in Cells.Values)
			{
				cell.OnStateChanged -= OnCellStateChanged;
				Destroy(cell.gameObject);
			}
#endif
		}

		#endregion

		private void OnCellStateChanged(GridCell cell)
		{
		}

		// private void OnCellFilled(GridCell filledCell)
		// {
		// 	var directions = new Vector2Int[] { Direction.up, Direction.down, Direction.right, Direction.left, };
		// 	var connectedCells = new List<GridCell> { filledCell };
		// 	var visitedCells = new bool[CellCount, CellCount];
		//
		// 	for (int i = 0; i < connectedCells.Count; i++)
		// 	{
		// 		if (visitedCells[connectedCells[i].X, connectedCells[i].Y]) continue; // go next if visited
		// 		visitedCells[connectedCells[i].X, connectedCells[i].Y] = true; // now has visited
		//
		// 		for (int j = 0; j < 4; j++)
		// 		{
		// 			var neighbor = GetCell(new Vector2Int(connectedCells[i].X, connectedCells[i].Y) + directions[j]);
		// 			if (!neighbor) continue; // can overflow from the edges
		//
		// 			if (!visitedCells[neighbor.X, neighbor.Y] && !neighbor.IsEmpty)
		// 				connectedCells.Add(neighbor);
		// 		}
		// 	}
		// }

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