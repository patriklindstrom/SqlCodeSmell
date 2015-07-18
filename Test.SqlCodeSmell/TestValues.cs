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
      public static string Examplecode = "A do run run run, a do run run";
      public static string ExamplecodeWahsed = "adorunrunrunadorunrun";

      public static List<string> Example5GramList = new List<string>()
      {
          "adoru",
          "dorun",
          "orunr",
          "runru",
          "unrun",
          "nrunr",
          "runru",
          "unrun",
          "nruna",
          "runad",
          "unado",
          "nador",
          "adoru",
          "dorun",
          "orunr",
          "runru",
          "unrun"
      };
      public static List<string> Example5GramHashList = new List<string>()
      {
          "77",
          "74",
          "42",
          "17",
          "98",
          "50",
          "17",
          "98",
          "8",
          "88",
          "67",
          "39",
          "77",
          "74",
          "42",
          "17",
          "98"
      };
      public static List<List<string>> WindowsOfLen4List = new List<List<string>>()
      {
       new List<string>{ "77","74","42","17"},
       new List<string>{ "74","42","17","98"},
       new List<string>{ "42","17","98","50"},
       new List<string>{ "17","98","50","17"},
       new List<string>{ "98","50","17","98"},
       new List<string>{ "50","17","98","8"},
       new List<string>{ "17","98","8","88"}, 
       new List<string>{ "98","8","88","67"}, 
       new List<string>{ "8","88","67","39"},
       new List<string>{ "88","67","39","77"}, 
       new List<string>{ "67","39","77","74"}, 
       new List<string>{ "39","77","74","42"},
       new List<string>{ "77","74","42","17"},
       new List<string>{ "74","42","17","98"}
      };

      public static List<string> WinnowingFingerprintList = new List<string> {"17", "17", "8", "39","17"};

      public static List<List<string>> WinnowingFingerprintWith0PosList = new List<List<string>>
      {
          new List<string> {"17", "3"},
          new List<string> {"17", "6"},
          new List<string> {"8", "8"},
          new List<string> {"39", "11"},
          new List<string> {"17", "15"}
      };
      public static string Example5GramHashFucntion  (string gram)
      {
          return gram.ToLower();
      }

    }
}
