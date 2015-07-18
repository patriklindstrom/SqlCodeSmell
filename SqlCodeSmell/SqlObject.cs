using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test.SqlCodeSmell
{
   public enum EnumSqlObjType { StoredProcedur };
    public class SqlObject
    {
        private string _washedCode;
        public string Name { get; set; }
        public EnumSqlObjType SqlObjType { get; set; }
        public string OrgCode { get; set; }

        public string WashedCode
        {
            get { return _washedCode; }           
        }

        public List<NGram> nGramList { get; set; }

        public SqlObject(SqlObjectData sqlObjectData )
        {
            Name = sqlObjectData.Name;
            SqlObjType = sqlObjectData.SqlObjType;
            OrgCode = sqlObjectData.SqlObjCode;
            _washedCode = WashCode(OrgCode);
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
        public NGram(string gram,Func<string, string> hashFunc)
        {

        }
    }
    public class NGramSeq
    {
        public List<NGram> NGramList { get; set; }
        public int GramLength { get; set; }
        public string Hashvalue { get; set; }
        private Func<string, string> HashFunc()
        {
            throw new NotImplementedException();
        }
    }
}
