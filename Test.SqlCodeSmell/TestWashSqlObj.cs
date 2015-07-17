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
            var mockSqlObjectReader = MockRepository.GenerateStub<SqlObjectReader>();
            mockSqlObjectReader.Stub(f => f.GetNextType()).Return(Mv.StoredProcedureWithCommentsType);
            mockSqlObjectReader.Stub(f => f.GetNextName()).Return(Mv.StoredProcedureWithCommentsName);
            mockSqlObjectReader.Stub(f => f.GetNextSqlObjCode()).Return(Mv.StoredProcedureWithCommentsCode);
            var expectedCode = Mv.StoredProcedureWithCommentsWashedcode;
            //Act
            var sut = new SqlObject(name: mockSqlObjectReader.GetNextName()
                , sqlObjType: mockSqlObjectReader.GetNextType()
                , orgCode: mockSqlObjectReader.GetNextSqlObjCode());                        
            //Assert
            Assert.AreEqual(expectedCode, sut.WashedCode, "Washed code is not what I expected");
        }

    }
}
