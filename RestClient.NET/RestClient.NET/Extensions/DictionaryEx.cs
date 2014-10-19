using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaCahToa.Rest.Extensions
{
    public static class DictionaryEx
    {
        public static string ToQueryString(this Dictionary<string, string> dictionary)
        {
            List<string> ret = new List<string>();

            foreach(KeyValuePair<string, string> pair in dictionary)
            {
                ret.Add(Uri.EscapeDataString(pair.Key) + "=" + Uri.EscapeDataString(pair.Value));
            }

            return string.Join("&", ret);
        }
    }
}
