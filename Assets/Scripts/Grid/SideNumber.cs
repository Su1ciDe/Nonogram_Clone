using TMPro;
using UnityEngine;

namespace Grid
{
	public class SideNumber : MonoBehaviour
	{
		[SerializeField] private TMP_Text txtNumber;

		public void SetNumber(int number)
		{
			txtNumber.SetText(number.ToString());
		}
	}
}