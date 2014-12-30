using System;
using System.Collections.Generic;
using System.Linq;

namespace SkaCahToa.Rest.Extensions
{
	internal static class DictionaryEx
	{
		/// <summary>
		/// Flattens dictionary to a valid http request QueryString.
		/// </summary>
		/// <param name="dictionary"></param>
		/// <returns>Valid QueryString</returns>
		internal static string ToQueryString(this Dictionary<string, string> dictionary)
		{
			List<string> ret = new List<string>();

			foreach (KeyValuePair<string, string> pair in dictionary.OrderBy(p => p.Key))
			{
				ret.Add(Uri.EscapeDataString(pair.Key) + "=" + Uri.EscapeDataString(pair.Value));
			}

			return string.Join("&", ret);
		}

		/// <summary>
		/// Add key value pair to dictionary if key doesn't exist.
		/// </summary>
		/// <typeparam name="TKey">Key Type</typeparam>
		/// <typeparam name="TValue">Value Type</typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key">KeyValuePair Key</param>
		/// <param name="value">KeyValuePair Value</param>
		/// <returns>true if pair was added to the dictionary.</returns>
		internal static bool AddSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, value);
				return true;
			}
			return false;
		}
	}
}