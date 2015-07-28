using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SqlCodeSmell;

namespace Test.SqlCodeSmell
{
    // from http://stackoverflow.com/a/8944374/648076
    [TestFixture]
    public class SplitByTestFixture
    {
        [TestCase("abcabcabcabcabca", 1, 16)]
        [TestCase("abcabcabcabcabca", 2, 8)]
        [TestCase("abcabcabcabcabca", 3, 6)]
        [TestCase("abcabcabcabcabca", 4, 4)]
        [TestCase("abcabcabcabcabca", 5, 4)]
        [TestCase("abcabcabcabcabca", 6, 3)]
        [TestCase("abcabcabcabcabca", 7, 3)]
        [TestCase("abcabcabcabcabca", 8, 2)]
        [TestCase("abcabcabcabcabca", 9, 2)]
        [TestCase("abcabcabcabcabca", 10, 2)]
        [TestCase("abcabcabcabcabca", 11, 2)]
        [TestCase("abcabcabcabcabca", 12, 2)]
        [TestCase("abcabcabcabcabca", 13, 2)]
        [TestCase("abcabcabcabcabca", 14, 2)]
        [TestCase("abcabcabcabcabca", 15, 2)]
        [TestCase("abcabcabcabcabca", 16, 1)]
        [TestCase("abcabcabcabcabca", 17, 1)]
        [TestCase("abcabcabcabcabca", 512, 1)]
        [Test]
        public void SplitByTest(string str, int chunkLength, int expCount)
        {
            var words = str.SplitBy(chunkLength);
            int actual = words.Count();
            Assert.AreEqual(expCount, actual);
        }

        [Test]
        public void SplitrunByTest()
        {
            //Arrange
            var sut = Mv.ExamplecodeWashed;
            //Act
            var resultGramList = sut.SplitRunBy(Mv.ExampleGramLength);
            // Assert
            CollectionAssert.AreEqual(Mv.Example5GramList, resultGramList,"The washed code is not split up in a resultgram list as expected");
        }
    }
}
