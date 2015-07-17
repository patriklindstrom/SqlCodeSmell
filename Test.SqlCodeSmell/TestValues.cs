using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.SqlCodeSmell
{
  /// <summary>
    /// Handy MockValues to use wherever in tests.
    /// </summary>
   public static class Mv
    {
       public static string StoredProcedureWithCommentsName = "StoredProcedureWithComments_SP";
       public static EnumSqlObjType StoredProcedureWithCommentsType = EnumSqlObjType.StoredProcedur;
       public static string StoredProcedureWithCommentsCode = "--I smell the blood of an ;Englishman\r\n Create procedure as select o.id,o.foo,u.fum from foo_TB as o\r\n " +
                                                               "/*here comes a join;\r\n" +
                                                               "we do not want to miss*/\r\n" +
                                                               "join fum_TB as u on u.id=o.id\r\n" +
                                                               "-- Will this work" +
                                                               "--join fi_TB as i on i.id=o.id";
       public static string StoredProcedureWithCommentsWashedcode = "createprocedureasselecto.id,o.foo,u.fumfromfoo_tbasojoinfum_tbasuonu.id=o.id";
    }
}
