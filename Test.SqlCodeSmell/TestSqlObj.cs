using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
namespace Test.SqlCodeSmell
{
    [TestFixture]
    public class TestSqlObject
    {
        [Test]
        static public void TestStoredProcedureWithCommentAndWhiteIsWashed()
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
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength, hashFunc: Mv.Example5GramHashFunction);                        
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }

        [Test]
        static public void TestOneHash()
            {
            //Arrange
           var sut= Mv.Example5GramList[0];
           var expectedHashVal = Mv.Example5GramHashDotNetList[0];
             //Act
            var result = Mv.Example5GramHashFunction(sut);
                //Assert
            Assert.AreEqual(expectedHashVal,result, "Hashfunction does not give expected value");
            }

        [Test]
        static public void TestAllExampleHash()
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
                if (Mv.Example5GramHashFunction(gram: Mv.Example5GramList[index]) != Mv.Example5GramHashDotNetList[index])
                {
                    Assert.IsTrue(allValEqual, "HashValue is not whatis expected");
                }
            }
            
            //Assert
            CollectionAssert.AreEqual(Mv.Example5GramHashDotNetList, myHashList, "Hashfunction does not give expected value for all Example grams");
        }
        [Test]
        static public void TestGramAndHash()
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
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength, hashFunc: Mv.Example5GramHashFunction);
            sut.AddNGramSequnce(hashFunc: Mv.Example5GramHashFunction, gramLen: Mv.ExampleGramLength,
                washedCode: Mv.ExamplecodeWahsed);
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }
    }
}
