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

        private NGramSeq AddNGramSequnce(Func<string, string> hashFunc,int gramLen,string washedCode)
        {
            var nGramSeq = new NGramSeq(hashFunc,gramLen);

            return NGramSequnce;
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
        public string Position0Based { get; set; } 
    }
    public class NGramSeq
    {
        public NGramSeq(Func<string, string> hashFunc,int gramLen)
        {
            HashFunc = hashFunc;
        }
        public List<NGram> NGramList { get; set; }
        public int GramLength { get; set; }
        public string Hashvalue { get; set; }
        private Func<string, string> HashFunc { get; set; }
       
    }
}
