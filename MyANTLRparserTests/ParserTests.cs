using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyANTLRparser;
using System;
using System.Collections.Generic;
using System.Text;


namespace MyANTLRparser.Tests
{
    [TestClass()]
    public class ParserTests
    {
        
        [TestMethod()]
        public void WhitespaceTest()
        {
            // arrange
            Parser p = new Parser("     ");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedElements.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedElements[0].ElementType == elementType.whiteSpace);
            
        }

        [TestMethod()]
        public void integerTest123()
        {
            // arrange

            Parser p = new Parser("123");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest0xabc()
        {
            // arrange

            Parser p = new Parser("0xabc");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest0xab_cd_ef()
        {
            // arrange

            Parser p = new Parser("0x_ab_cd_ef");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest123U()
        {
            // arrange

            Parser p = new Parser("123U");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest123L()
        {
            // arrange

            Parser p = new Parser("123L");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest123UL()
        {
            // arrange

            Parser p = new Parser("123UL");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest0b10011010()
        {
            // arrange

            Parser p = new Parser("0b10011010");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }
        [TestMethod()]
        public void integerTest0b_1001_1010()
        {
            // arrange

            Parser p = new Parser("0b_1001_1010");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalInteger));

        }

        [TestMethod()]
        public void realTest123()
        {
            // arrange

            Parser p = new Parser("123.0");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void realTest123e1()
        {
            // arrange

            Parser p = new Parser("123.0e1");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void invalid1eplus()
        {
            // arrange

            Parser p = new Parser("1e+");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.invalidToken));

        }

        [TestMethod()]
        public void valid123z()
        {
            // arrange

            Parser p = new Parser("123.0");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }
        [TestMethod()]
        public void valid12p30()
        {
            // arrange

            Parser p = new Parser("12.30");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }
        [TestMethod()]
        public void valid1p230()
        {
            // arrange

            Parser p = new Parser("1.230");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void validp1230()
        {
            // arrange

            Parser p = new Parser(".1230");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void valid1e12()
        {
            // arrange

            Parser p = new Parser("1e12");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void valid1p2e30()
        {
            // arrange

            Parser p = new Parser("1.2e30");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }

        [TestMethod()]
        public void valid1p23e12()
        {
            // arrange

            Parser p = new Parser("1.23e12");
            p.InitToCSharpStatemachine();
            // act
            p.ParseAll();
            // assert
            int count = p.ParsedTokens.Count;
            Assert.IsTrue(1 == count);
            Assert.IsTrue(p.ParsedTokens[0].
                TokenType.IsAnyOfTheseTypes(tokenType.literalReal));

        }


    }
}