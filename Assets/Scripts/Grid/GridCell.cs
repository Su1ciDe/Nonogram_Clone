using System;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utility;

namespace Grid
{
	public class GridCell : MonoBehaviour, IPointerDownHandler
	{
		public CellState CurrentState { get; set; } = CellState.Empty;
		public CellState CorrectState { get; set; }
		public bool IsCorrect => CurrentState == CorrectState || (CurrentState == CellState.X && CorrectState == CellState.Empty);

		public int X { get; set; }
		public int Y { get; set; }
		public Vector2Int Position => new Vector2Int(X, Y);

		[SerializeField] private GameObject filledState;
		[SerializeField] private GameObject xState;
		[Space]
		[SerializeField] private Image incorrect;
		[SerializeField] private Image hint;

		public event UnityAction<GridCell> OnStateChanged;

		private void OnDestroy()
		{
			incorrect.DOKill();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!Player.Instance.CanPlay) return;

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
				default:
					throw new ArgumentOutOfRangeException();
			}

			CheckIncorrect();
		}

		private void CheckIncorrect()
		{
			if (IsCorrect)
			{
				incorrect.gameObject.SetActive(false);
				HideHint();
			}
			else
			{
				incorrect.gameObject.SetActive(true);
				incorrect.DOKill();
				incorrect.DOFade(1, .5f).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Yoyo).OnKill(() =>
				{
					var color = incorrect.color;
					color.a = 0;
					incorrect.color = color;
				});
			}
		}

		public void ShowHint()
		{
			hint.gameObject.SetActive(true);
			hint.DOKill();
			hint.DOFade(1, .25f).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Yoyo).OnKill(() =>
			{
				var color = hint.color;
				color.a = 0;
				hint.color = color;
			});
		}

		private void HideHint()
		{
			hint.DOKill();
			hint.gameObject.SetActive(false);
		}
	}
}