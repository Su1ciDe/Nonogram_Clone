using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public class WeightedRandomList<T> : List<T>
	{
		private readonly List<int> weightList;

		public WeightedRandomList()
		{
			weightList = new List<int>();
		}

		/// <summary>
		/// Add an item with its weight
		/// </summary>
		/// <param name="item">Added item</param>
		/// <param name="weight">Higher the weight higher the chance</param>
		internal void Add(T item, int weight)
		{
			base.Add(item);
			weightList.Add(weight);
		}

		internal new void Remove(T item)
		{
			weightList.RemoveAt(base.IndexOf(item));
			base.Remove(item);
		}

		/// <summary>
		/// Gets the weighted random item
		/// </summary>
		/// <returns>Returns the item that has a higher chance</returns>
		public T Next()
		{
			int totalPriority = 0;
			var weightsOriginal = new List<int>(weightList);
			var tempWeights = new List<int>(weightList);

			for (int i = 0; i < base.Count; i++)
			{
				weightsOriginal[i] += totalPriority;
				totalPriority += tempWeights[i];
			}

			int randomPriority = Random.Range(0, totalPriority);
			for (int i = 0; i < base.Count; i++)
			{
				if (weightsOriginal[i] > randomPriority)
					return base[i];
			}

			return base[0];
		}
	}
}