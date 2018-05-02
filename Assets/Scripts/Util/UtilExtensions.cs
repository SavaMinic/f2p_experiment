using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Nordeus.Util.CSharpLib
{
	/// <summary>
	/// Static class containing extension methods for list.
	/// </summary>
	public static class UtilExtensions
	{

		/// <summary>
		/// Fetches a first element from list that satisfies the condition and removes it from the list.
		/// </summary>
		public static T FindAndRemove<T>(this IList<T> list, Predicate<T> filter)
		{
			T value = list.First(t => filter(t));
			if (value != null) { list.Remove(value); }
			return value;
		}

		/// <summary>
		/// Returns true if collection is null or empty,
		/// otherwise returns false.
		/// </summary>
		[ContractAnnotation("collection:null => true")]
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}

		public static int CountAll<T>(this List<T> list, Predicate<T> predicate)
		{
			if (list == null || predicate == null)
			{
				throw new ArgumentException("Both list and predicate must not be null!");
			}

			int count = 0;

			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					count++;
				}
			}

			return count;
		}

		public static object[] ToArray(this IList list)
		{
			if (list != null)
			{
				object[] array = new object[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = list[i];
				}
				return array;
			}
			else
			{
				return null;
			}
		}

		public static bool SequenceEquals(this IList leftList, IList rightList)
		{
			if (leftList.Count != rightList.Count) { return false; }

			for (int i = 0; i < leftList.Count; i++)
			{
				object left = leftList[i];
				object right = rightList[i];

				if (left == null && right == null) { continue; }

				if (left == null) { return false; }

				if (!left.Equals(right)) { return false; }
			}

			return true;
		}


		public static T Find<T>(this IList<T> list, Predicate<T> predicate)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}

			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}

			for (var index = 0; index < list.Count; index++)
			{
				var element = list[index];

				if (predicate(element))
				{
					return element;
				}
			}

			return default(T);
		}

		public static List<T> FilterByCriteria<T>(this ICollection<T> listToSearch, Predicate<T> criteria, int maximumNumberOfElement = 0)
		{

			if (criteria == null) throw new ArgumentNullException("criteria");

			List<T> foundElements = new List<T>();

			foreach (var key in listToSearch)
			{
				if (criteria(key))
				{
					foundElements.Add(key);

					if (maximumNumberOfElement > 0 && foundElements.Count >= maximumNumberOfElement) { break; }
				}
			}

			return foundElements;
		}

		/// <summary>
		/// Converts input IList to output List. List<T> has this method, but IList does not.
		/// </summary>
		public static List<TOutput> ConvertAll<TInput, TOutput>(this IList<TInput> inputList, Converter<TInput, TOutput> converter)
		{
			var outputList = new List<TOutput>(inputList.Count);

			for (int index = 0; index < inputList.Count; index++)
			{
				outputList.Add(converter(inputList[index]));
			}

			return outputList;
		}

		/// <summary>
		/// Checks if collection is null and clears is if it isn't.
		/// </summary>
		public static void ClearIfNotNull<T>(this ICollection<T> collection)
		{
			if (collection != null)
			{
				collection.Clear();
			}
		}


		/// <summary>
		/// Sequance compares two byte arrays.
		/// </summary>
		public static bool SequenceEqual(this byte[] a1, byte[] a2)
		{
			if (a1.Length != a2.Length)
				return false;

			for (int i = 0; i < a1.Length; i++)
				if (a1[i] != a2[i])
					return false;

			return true;
		}

		/// <summary>
		/// Shuffle a list.
		/// </summary>
		public static void Shuffle<T>(this List<T> list)
		{
			int count = list.Count;
			for (int index = 0; index < count; index++)
			{
				int chosenOne = GetRandomInt(index, count - 1);
				T temp = list[index];
				list[index] = list[chosenOne];
				list[chosenOne] = temp;
			}
		}

		/// <summary>
		/// Extension method for getting a random element from the list.
		/// </summary>
		/// <typeparam name="T">The type of the list.</typeparam>
		/// <param name="list">The list to get from.</param>
		/// <returns>A random element from the list.</returns>
		public static T GetRandom<T>(this List<T> list)
		{
			if (list.Count == 0) return default(T);

			int index = GetRandomInt(0, list.Count - 1);
			return list[index];
		}

		/// <summary>
		/// Gets random int. Assumes input arguments are correct.
		/// </summary>
		/// <param name="minValue">Min possible value (inclusive).</param>
		/// <param name="maxValue">Max possible value (inclusive).</param>
		/// <returns>Random int between the arguments passed, including arguments themselves.</returns>
		public static int GetRandomInt(int minValue, int maxValue)
		{
			return UnityEngine.Random.Range(minValue, maxValue + 1);
		}
		
		/// <summary>
		/// Calls the action if it's not null.
		/// </summary>
		public static void CallIfNotNull(this Action action)
		{
			if (action != null)
			{
				action();
			}
		}
		
		/// <summary>
		/// Calls the action if it's not null.
		/// </summary>
		public static void CallIfNotNull<T>(this Action<T> action, T arg)
		{
			if (action != null)
			{
				action(arg);
			}
		}

		/// <summary>
		/// Calls the action if it's not null.
		/// </summary>
		public static void CallIfNotNull<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			if (action != null)
			{
				action(arg1, arg2);
			}
		}
	
		/// <summary>
		/// Calls the action if it's not null.
		/// </summary>
		public static void CallIfNotNull<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (action != null)
			{
				action(arg1, arg2, arg3);
			}
		}
	
		public static bool IsNullOrWhiteSpace(this string value)
		{
			if (value == null) return true;

			for (int index = 0; index < value.Length; ++index)
			{
				if (!char.IsWhiteSpace(value[index])) return false;
			}

			return true;
		}
		
		/// <summary>
		/// Returns true if two numbers diff by less than 0.0001f. Much faster and simpler than Mathf.Approximately().
		/// </summary>
		public static bool Approximately(this float x, float y)
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (x == y) return true; // This fixes if both floats are Infinity.
			return (x > y ? (x - y) : (y - x)) < 0.0001f;
		}
		
		public static string FormatToTime(this float timeSeconds)
		{
			int hours = (int) (timeSeconds / 3600);
			int minutes = (int)(timeSeconds / 60) % 60;
			int seconds = (int)timeSeconds % 60;
			if (hours > 0)
			{
				if (minutes > 0)
					return hours + "h " + minutes + "m";
				return hours + "h";
			}

			if (minutes > 0)
			{
				if (seconds > 0)
					return minutes + "m " + seconds + "s";
				return minutes + "m";
			}

			return seconds + "s";
		}
		
		public static Color GetWithAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}
