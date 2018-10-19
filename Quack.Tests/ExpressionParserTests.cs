using System.Linq;
using NUnit.Framework;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;
using Quack.Parser.LegacyParser;
using Quack.Parser.LegacyParser.Brackets;

namespace Quack.Tests
{
	[TestFixture]
	public class ExpressionParserTests
	{
		private IBracketService BracketService() => new BracketService();

		private IExpressionParser Service() => new ExpressionParser(BracketService());

		private AstNode Act(TokenQueue tokens) 
			=> Service().ParseExpression(tokens);

		private void AssertAstNode(AstNode node, AstNodeType type, string value = null)
		{
			Assert.That(node.Type, Is.EqualTo(type));
			Assert.That(node.Value, Is.EqualTo(value));
		}

		[Test]
		public void ParseExpression_Can_Parse_Arithmetic_Expression()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.NUMBER, "num1"),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op1"),
				new Token(TokenType.LABEL, "label1")
			});

			var result = Act(tokens);

			AssertAstNode(result, AstNodeType.ARITHMETIC_OPERATOR, "op1");
			AssertAstNode(result.Children.ElementAt(0), AstNodeType.NUMBER, "num1");
			AssertAstNode(result.Children.ElementAt(1), AstNodeType.LABEL, "label1");
		}

		[Test]
		public void ParseExpression_Can_Parse_Boolean_Expression()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.NUMBER, "num1"),
				new Token(TokenType.BOOLEAN_RELATIONAL_OPERATOR, "op1"),
				new Token(TokenType.LABEL, "label1")
			});

			var result = Act(tokens);

			AssertAstNode(result, AstNodeType.BOOLEAN_OPERATOR, "op1");
			AssertAstNode(result.Children.ElementAt(0), AstNodeType.NUMBER, "num1");
			AssertAstNode(result.Children.ElementAt(1), AstNodeType.LABEL, "label1");
		}

		[Test]
		public void ParseExpression_Can_Parse_Label()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.LABEL, "label1")
			});

			var result = Act(tokens);
			
			AssertAstNode(result, AstNodeType.LABEL, "label1");
		}

		[Test]
		public void ParseExpression_Can_Parse_Number()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.NUMBER, "num1"),
			});

			var result = Act(tokens);

			AssertAstNode(result, AstNodeType.NUMBER, "num1");
		}

		[Test]
		public void ParseExpression_Can_Parse_Nested_Factors()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.OPEN_PARENTHESES, "("),
				new Token(TokenType.OPEN_PARENTHESES, "("),
				new Token(TokenType.NUMBER, "num1"),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op1"),
				new Token(TokenType.LABEL, "label1"),
				new Token(TokenType.CLOSE_PARENTHESES, ")"),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op2"),
				new Token(TokenType.LABEL, "label2"),
				new Token(TokenType.CLOSE_PARENTHESES, ")"),
			});

			var result = Act(tokens);

			AssertAstNode(result, AstNodeType.FACTOR);

			var outerOp = result.Children.Single();
			AssertAstNode(outerOp, AstNodeType.ARITHMETIC_OPERATOR, "op2");
			AssertAstNode(outerOp.Children.ElementAt(0), AstNodeType.FACTOR);
			AssertAstNode(outerOp.Children.ElementAt(1), AstNodeType.LABEL, "label2");

			var innerOp = outerOp.Children.ElementAt(0).Children.Single();
			AssertAstNode(innerOp, AstNodeType.ARITHMETIC_OPERATOR, "op1");
			AssertAstNode(innerOp.Children.ElementAt(0), AstNodeType.NUMBER, "num1");
			AssertAstNode(innerOp.Children.ElementAt(1), AstNodeType.LABEL, "label1");
		}

		[Test]
		public void ParseExpression_Throws_ParseException_When_Token_Not_Recognised()
		{
			var tokens = new TokenQueue(new[]
			{
				new Token(TokenType.FUNC_DECLARE, "BAD_TOKEN"),
			});

			Assert.Throws<ParseException>(() => Act(tokens));
		}
	}
}
