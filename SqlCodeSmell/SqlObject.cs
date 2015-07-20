using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test.SqlCodeSmell
{
   public enum EnumSqlObjType { StoredProcedure,ScalarFunction,TableFunction };
    public class SqlObject
    {
        private string _washedCode;
        public string Name { get; set; }
        public EnumSqlObjType SqlObjType { get; set; }
        public string OrgCode { get; set; }
        public NGramSeq NGramSequnce { get; set; }

        public string WashedCode
        {
            get { return _washedCode; }           
        }

        public List<NGram> NGramList { get; set; }

        public SqlObject(SqlObjectData sqlObjectData, int gramLen, Func<string, string> hashFunc)
        {
            Name = sqlObjectData.Name;
            SqlObjType = sqlObjectData.SqlObjType;
            OrgCode = sqlObjectData.SqlObjCode;
            _washedCode = WashCode(OrgCode);
            NGramSequnce = AddNGramSequnce(hashFunc, gramLen, _washedCode);
        }

        public NGramSeq AddNGramSequnce(Func<string, string> hashFunc,int gramLen,string washedCode)
        {
            var nGramSeq = new NGramSeq(hashFunc,gramLen);
            var gramlist = washedCode.SplitBy(gramLen);
            int i = 0;
            foreach (var grammy in gramlist)
            {
                nGramSeq.Add(gram: grammy, pos: (gramLen*i));
                i++;
            }

            return nGramSeq;
        }

        private string WashCode(string input)
        {
            var noComments = RemovComments(input);
            var noWhiteCode = RemoveWhitespace(noComments);
            return  noWhiteCode.ToLower();
        }

        private string RemovComments(string input)
        {
            Regex findMultiLineComments = new Regex(@"\/\*.*|.*(\n\r)*\*\/");
            var noMultiLineComments = findMultiLineComments.Replace(input, String.Empty);
            Regex findSingleLineComments = new Regex(@"--.*");
            var noSingleLineComments = findSingleLineComments.Replace(noMultiLineComments, String.Empty);
            return noSingleLineComments.ToLower() ;
        }

        private  string RemoveWhitespace( string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
    public class SqlObjectData
    {
        public string SqlObjCode { get; set; }       
        public  string Name{ get; set; }
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
        public NGramSeq(Func<string, string> hashFunc,int gramLen)
        {
            HashFunc = hashFunc;
            NGramList = new List<NGram>();
        }
        public List<NGram> NGramList { get; set; }
        private Func<string, string> HashFunc { get; set; }

        public void Add(string gram,int pos)
        {
           NGramList.Add(new NGram{Gram = gram,Hashvalue = HashFunc(gram),Position0Based = pos});
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

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
    }
}
