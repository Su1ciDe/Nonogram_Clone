using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utility;

namespace Grid
{
	public class GridCell : MonoBehaviour, IPointerClickHandler
	{
		public CellState CurrentState { get; set; } = CellState.Empty;
		public CellState CorrectState { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public Vector2Int Position => new Vector2Int(X, Y);

		[SerializeField] private GameObject filledState;
		[SerializeField] private GameObject xState;

		public event UnityAction<GridCell> OnStateChanged;

		public void OnPointerClick(PointerEventData eventData)
		{
			ChangeState();

			OnStateChanged?.Invoke(this);
		}

		private void ChangeState()
		{
			CurrentState = (CellState)(((int)CurrentState + 1) % Grid.Instance.CellStateCount);
			switch (CurrentState)
			{
				case CellState.Empty:
					filledState.SetActive(false);
					xState.SetActive(false);
					break;
				case CellState.Filled:
					xState.SetActive(false);
					filledState.SetActive(true);
					break;
				case CellState.X:
					filledState.SetActive(false);
					xState.SetActive(true);
					break;
			}
		}
	}
}