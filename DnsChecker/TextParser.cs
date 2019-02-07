using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnsChecker
{
    class TextParser
    {
        public string ParseText(string text)
        {
            string RetVal = "";
            if (!String.IsNullOrEmpty(text))
            {
                string search = "\"value\":";                
                var stringArray = text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < stringArray.Length; i++)
                {
                    if (stringArray[i].Contains(search))
                    {
                        int lastIdxQuotes = stringArray[i].LastIndexOf('\u0022');
                        int thirdIdxQuotes = GetNthIndex(stringArray[i], '\u0022', 3);
                        string nsField = stringArray[i].Substring(thirdIdxQuotes + 1, lastIdxQuotes - thirdIdxQuotes - 2);
                        RetVal = nsField;
                    }
                }
            }
            return RetVal;
        }

        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
 