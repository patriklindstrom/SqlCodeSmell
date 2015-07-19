using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks.Constraints;

namespace Test.SqlCodeSmell
{
  /// <summary>
    /// Handy MockValues to use wherever in tests.
    /// </summary>
   public static class Mv
    {
       public static string StoredProcedureWithCommentsName = "StoredProcedureWithComments_SP";
       public static EnumSqlObjType StoredProcedureWithCommentsType = EnumSqlObjType.StoredProcedure;
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
          // TODO: change all examples and use GetHashCode instead I cant figure out their hash algoritm they have used. 
          // as long as it is the same positions it is ok.
        var hash= gram.GetHashCode();
         // char[] gramCharArray = gram.ToCharArray();
          //byte[] asciiBytes = Encoding.ASCII.GetBytes(gram);
          //int gramSum = asciiBytes.Sum(c => c);
          //for (int i = 2; i < 550; i++)
          //{
          //    var j = gramSum % i;
          //    if (j == 77)
          //    {
          //        Debug.Print("+++ this is the onemodvalue:" + i.ToString());
          //    }
          //    Debug.Print(j.ToString());
          //    Debug.Print("modvalue:" + j.ToString());
          //}
          //// 239 is a prime looks like they are using that in the example from Stanford
          //int hashAsciiBytes = gramSum % 239;
        return hash.ToString();
      }

      public static int ExampleGramLength = 5;

    }
}
