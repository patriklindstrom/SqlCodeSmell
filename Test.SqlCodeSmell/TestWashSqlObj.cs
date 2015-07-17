using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
namespace Test.SqlCodeSmell
{
    [TestFixture]
    public class TestWashSqlObject
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
            var sut = new SqlObject(sqlObjectData: mockSqlObjectReader.GetNextSqlObj());                        
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }

    }
}
