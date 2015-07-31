using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace SqlCodeSmell
{
    public enum EnumSqlObjType
    {
        StoredProcedure,
        ScalarFunction,
        TableFunction
    };

    public class SqlObject
    {
        private string _washedCode;
        private List<List<NGram>> _windowsList;
        private List<NGram> _fingerPrint;
        public string Name { get; set; }
        public EnumSqlObjType SqlObjType { get; set; }
        public string OrgCode { get; set; }
        public NGramSeq NGramSequnce { get; set; }

        public List<NGram> FingerPrint
        {
            get { return _fingerPrint; }
        }

        public string WashedCode
        {
            get { return _washedCode; }
        }

        public List<NGram> NGramList { get; set; }

        public List<List<NGram>> WindowsList
        {
            get { return _windowsList; }
        }

        public SqlObject(SqlObjectData sqlObjectData, int gramLen, Func<string, string> hashFunc)
        {
            Name = sqlObjectData.Name;
            SqlObjType = sqlObjectData.SqlObjType;
            OrgCode = sqlObjectData.SqlObjCode;
            _washedCode = WashCode(OrgCode);
            NGramSequnce = AddNGramSequnce(hashFunc, gramLen, _washedCode);
        }

        public NGramSeq AddNGramSequnce(Func<string, string> hashFunc, int gramLen, string washedCode)
        {
            var nGramSeq = new NGramSeq(hashFunc, gramLen);
            var gramlist = washedCode.SplitRunBy(gramLen);
            int i = 0;
            foreach (var grammy in gramlist)
            {
                nGramSeq.Add(gram: grammy, pos: (gramLen*i));
                i++;
            }

            return nGramSeq;
        }

        public void AddNFingerPrintSequnce(List<List<NGram>> wList)
        {
            // if there is nothing in the fingerprint we must store something to start with.
            _fingerPrint = new List<NGram>() {FindMinGram(wList, 0)};
            for (int j = 1; j < wList.Count; j++)
            {
                NGram minWnGram = FindMinGram(wList, j);
                var lastOrDefault = _fingerPrint.LastOrDefault();
                if (!lastOrDefault.Equals(minWnGram))
                {
                    _fingerPrint.Add(minWnGram);
                }
            }
        }

        public static NGram FindMinGram(List<List<NGram>> wList, int i)
        {
            return wList[i].FindLast(g => g.Hashvalue == wList[i].MinBy(m => m.Hashvalue).Hashvalue);
        }

        private string WashCode(string input)
        {
            var noComments = RemovComments(input);
            var noWhiteCode = RemoveWhitespace(noComments);
            return noWhiteCode.ToLower();
        }

        private string RemovComments(string input)
        {
            Regex findMultiLineComments = new Regex(@"\/\*.*|.*(\n\r)*\*\/");
            var noMultiLineComments = findMultiLineComments.Replace(input, String.Empty);
            Regex findSingleLineComments = new Regex(@"--.*");
            var noSingleLineComments = findSingleLineComments.Replace(noMultiLineComments, String.Empty);
            return noSingleLineComments.ToLower();
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !(Char.IsWhiteSpace(c) || Char.IsPunctuation(c) || Char.IsSeparator(c)))
                .ToArray());
        }

        public void AddWindowsList(int windowSize)
        {
            _windowsList = new List<List<NGram>>();
            int totalWindows = NGramSequnce.NGramList.Count - windowSize + 1;
            for (int page = 0; page < totalWindows; page++)
            {
                _windowsList.Add(NGramSequnce.NGramList.Skip(page).Take(windowSize).ToList());
            }
        }

        private int SelectUniqueHashInWindows()
        {
            // Min Hash and the max index in window to the right Aiken rule
            throw new NotImplementedException();
        }
    }

    public class SqlObjectData
    {
        public string SqlObjCode { get; set; }
        public string Name { get; set; }
        public EnumSqlObjType SqlObjType { get; set; }
    }

    public class SqlObjectReader
    {
        public virtual SqlObjectData GetNextSqlObj()
        {
            return null;
        }
    }

    public class NGram
    {
        public string Gram { get; set; }
        public string Hashvalue { get; set; }
        public int Position0Based { get; set; }
    }

    public class NGramSeq
    {
        public NGramSeq(Func<string, string> hashFunc, int gramLen)
        {
            HashFunc = hashFunc;
            NGramList = new List<NGram>();
            GramLen = gramLen;
        }

        public int GramLen { get; set; }

        public List<NGram> NGramList { get; set; }
        private Func<string, string> HashFunc { get; set; }

        public void Add(string gram, int pos)
        {
            NGramList.Add(new NGram {Gram = gram, Hashvalue = HashFunc(gram), Position0Based = pos});
        }
    }

    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }

        public static IEnumerable<string> SplitRunBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();
            return str.TakeWhile((t, i) => chunkLength + i <= str.Length)
                .Select((t, i) => str.Substring(i, chunkLength));
            //for (int i = 0; i < str.Length; i++)
            //{
            //    if (chunkLength + i > str.Length)
            //    { yield break; }

            //    yield return str.Substring(i, chunkLength);
            //}
        }
    }
}
