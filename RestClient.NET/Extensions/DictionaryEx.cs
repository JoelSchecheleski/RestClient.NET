using System;
using System.Collections.Generic;
using System.Linq;

namespace SkaCahToa.Rest.Extensions
{
	internal static class DictionaryEx
    {
        internal static string ToQueryString(this Dictionary<string, string> dictionary)
        {
            List<string> ret = new List<string>();

            foreach (KeyValuePair<string, string> pair in dictionary.OrderBy(p => p.Key))
            {
                ret.Add(Uri.EscapeDataString(pair.Key) + "=" + Uri.EscapeDataString(pair.Value));
            }

            return string.Join("&", ret);
        }

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