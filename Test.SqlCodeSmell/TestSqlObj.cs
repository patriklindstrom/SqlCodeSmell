using System;
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
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength, hashFunc: Mv.Example5GramHashFucntion);                        
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }

        [Test]
        static public void TestOneHash()
            {
            //Arrange
           var sut= Mv.Example5GramList[0];
           var expectedHashVal = Mv.Example5GramHashList[0];
             //Act
            var result = Mv.Example5GramHashFucntion(sut);
                //Assert
            Assert.AreEqual(expectedHashVal,result, "Hashfunction does not give expected value");
            }

        [Test]
        static public void TestAllExampleHash()
        {
            //Arrange
            var sut = Mv.Example5GramList;
            var expectedHashVal = Mv.Example5GramHashList;
            bool allValEqual = false;
            var myHashList = Mv.Example5GramList.Select(Mv.Example5GramHashFucntion);
            //Act
            for (int index = 0; index < Mv.Example5GramList.Count; index++)
            {
                var gram = Mv.Example5GramList[index];
                
                if (Mv.Example5GramHashFucntion(gram) == Mv.Example5GramHashList[index])
                {
                    allValEqual = true;
                }
                else
                {
                    allValEqual = false;
                    break;
                }
            }
            
            //Assert
           // Assert.IsTrue(allValEqual, "Hashfunction does not give expected value for all Example grams");
            CollectionAssert.AreEqual(Mv.Example5GramList,myHashList, "Hashfunction does not give expected value for all Example grams");
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
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj(), gramLen: Mv.ExampleGramLength, hashFunc: Mv.Example5GramHashFucntion);
            sut.AddNGramSequnce(hashFunc: Mv.Example5GramHashFucntion, gramLen: Mv.ExampleGramLength,
                washedCode: Mv.ExamplecodeWahsed);
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }
    }
}
