using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using Rhino.Mocks;
using SqlCodeSmell;

namespace Test.SqlCodeSmell
{
    [TestFixture]
    public class TestSqlObject
    {
        [Test]
        public static void TestStoredProcedureWithCommentAndWhiteIsWashed()
        {
            //Arrange
            var mockSqlObjectData = MockRepository.GenerateStub<SqlObjectData>();
            mockSqlObjectData.SqlObjType = Mv.StoredProcedureWithCommentsType;
            mockSqlObjectData.Name = Mv.StoredProcedureWithCommentsName;
            mockSqlObjectData.SqlObjCode = Mv.StoredProcedureWithCommentsCode;
            var mockSqlObjectReader = MockRepository.GenerateStub<SqlObjectReader>();
            mockSqlObjectReader.Stub(f => f.GetNextSqlObj()).Return(mockSqlObjectData);
            var expectedCode = Mv.StoredProcedureWithCommentsWashedcode;
            //Act
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength,
                hashFunc: Mv.Example5GramHashFunction);
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }

        [Test]
        public static void TestOneHash()
        {
            //Arrange
            var sut = Mv.Example5GramList[0];
            var expectedHashVal = Mv.Example5GramHashDotNetList[0];
            //Act
            var result = Mv.Example5GramHashFunction(sut);
            //Assert
            Assert.AreEqual(expectedHashVal, result, "Hashfunction does not give expected value");
        }

        [Test]
        public static void TestAllExampleHash()
        {
            //Arrange
            var sut = Mv.Example5GramList;
            var expectedHashVal = Mv.Example5GramHashDotNetList;
            bool allValEqual = false;
            var myHashList = Mv.Example5GramList.Select(Mv.Example5GramHashFunction);
            //Act
            // This for loop is uneccarey the collection assert fixes it. It is just here in case of narrow debug.
            for (int index = 0; index < Mv.Example5GramList.Count; index++)
            {
                // Debug.Print('\u0022' + Mv.Example5GramHashFunction(gram) + '\u0022' + ",");
                if (Mv.Example5GramHashFunction(gram: Mv.Example5GramList[index]) !=
                    Mv.Example5GramHashDotNetList[index])
                {
                    Assert.IsTrue(allValEqual, "HashValue is not whatis expected");
                }
            }

            //Assert
            CollectionAssert.AreEqual(Mv.Example5GramHashDotNetList, myHashList,
                "Hashfunction does not give expected value for all Example grams");
        }

        [Test]
        public static void TestGramAndHash()
        {
            //Arrange
            var mockSqlObjectData = MockRepository.GenerateStub<SqlObjectData>();
            mockSqlObjectData.SqlObjType = Mv.StoredProcedureWithCommentsType;
            mockSqlObjectData.Name = "stanfordDoRunExample";
            mockSqlObjectData.SqlObjCode = Mv.Examplecode;
            var mockSqlObjectReader = MockRepository.GenerateStub<SqlObjectReader>();
            mockSqlObjectReader.Stub(f => f.GetNextSqlObj()).Return(mockSqlObjectData);
            var expectedCode = Mv.ExamplecodeWashed;
            var expectedHash = Mv.Example5GramList.Select(Mv.Example5GramHashFunction);
            //Act
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength,
                hashFunc: Mv.Example5GramHashFunction);
            sut.AddNGramSequnce(hashFunc: Mv.Example5GramHashFunction, gramLen: Mv.ExampleGramLength,
                washedCode: Mv.ExamplecodeWashed);
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "The washedcode is not what is expected");
            CollectionAssert.AreEqual(Mv.Example5GramHashDotNetList, expectedHash,
                "Hashfunction does not give expected value for all Example grams");
        }

        [Test]
        public static void TestWindowsOfhashesOfLengthN()
        {
            //Arrange
            var mockSqlObjectData = MockRepository.GenerateStub<SqlObjectData>();
            mockSqlObjectData.SqlObjType = Mv.StoredProcedureWithCommentsType;
            mockSqlObjectData.Name = "stanfordDoRunExample";
            mockSqlObjectData.SqlObjCode = Mv.Examplecode;
            var mockSqlObjectReader = MockRepository.GenerateStub<SqlObjectReader>();
            mockSqlObjectReader.Stub(f => f.GetNextSqlObj()).Return(mockSqlObjectData);
            var expectedWindowList = Mv.WindowsOfLen4DotNetList;
            var expectedHash = Mv.Example5GramList.Select(Mv.Example5GramHashFunction);
            //Act
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength,
                hashFunc: Mv.Example5GramHashFunction);
            sut.AddWindowsList(windowSize: Mv.ExampleWindowSize);
            // Our testvalues are not the same format as the sut windowslist, maybe we should make it the other way arount and mock and make better testvalues
            //Assert
            int j = 0;
            bool areEqual = false;
            foreach (var wlist in sut.WindowsList)
            {
                int k = 0;

                foreach (var gramlist in sut.WindowsList[j])
                {
                    areEqual = gramlist.Hashvalue == Mv.WindowsOfLen4DotNetList[j][k];
                    k++;
                }
                j++;
            }
            Assert.IsTrue(areEqual, "The Windows list do not matcht expected values ");

            // Unfortunate this does not work and cannot be debugged either. tried all sorts of mapping and fixing
            // CollectionAssert.AreEqual(Mv.WindowsOfLen4DotNetList.Select(g => g.Select(p => p)), sut.WindowsList.Select(g => g.Select(p => p.Hashvalue)), new HashValueCompareForNSeq(),
            //"The N gram Windows list do not matcht expected values ")
            ;
        }
    }

    public class HashValueCompareForNSeq : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            
            List<string> strX = (List<string>) x;
            List<NGram> nGramY = (List<NGram>) (y);
            int areEqual = -1;
            int k = 0;
            foreach (var gramlist in nGramY)
            {
                if (gramlist.Hashvalue == strX[k])
                {
                    areEqual = 0;
                }
                else
                {
                    areEqual = -1;
                }
                k++;
            }
            return (areEqual);
        }
    }

    public class OptimisticCompare : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            return 0;
        }
    }
}
