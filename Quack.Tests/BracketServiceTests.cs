using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;

namespace Quack.Tests
{
	[TestFixture]
	public class BracketServiceTests
	{
		private IBracketService Service() => new BracketService();
		
		private TokenQueue ValidTokens(TokenType openType, TokenType closeType) => new TokenQueue(new[]
		{
			new Token(TokenType.NUMBER, "num1"),
			new Token(TokenType.ARITHMETIC_OPERATOR, "op1"),
			new Token(openType),
			new Token(TokenType.NUMBER, "num2"),
			new Token(TokenType.ARITHMETIC_OPERATOR, "op2"),
			new Token(TokenType.NUMBER, "num3"),
			new Token(closeType),
			new Token(closeType),
			new Token(TokenType.ARITHMETIC_OPERATOR, "op3"),
			new Token(TokenType.NUMBER, "num4")
		});

		private Queue<Token> Act(TokenQueue tokens, TokenType openType, TokenType closeType) 
			=> Service().TakeTokensUntilClose(tokens, openType, closeType);

		[TestCase(TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES)]
		[TestCase(TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES)]
		public void TakeTokensUntilClose_Returns_Tokens_Until_Matching_Close_Token(TokenType openType, TokenType closeType)
		{
			var tokens = ValidTokens(openType, closeType);

			var result = Act(tokens, openType, closeType);

			Assert.That(result.Count, Is.EqualTo(7));
			Assert.That(result.First().Value, Is.EqualTo("num1"));
			Assert.That(result.Last().Type, Is.EqualTo(closeType));
		}

		[TestCase(TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES)]
		[TestCase(TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES)]
		public void TakeTokensUntilClose_Dequeues_Tokens_Up_To_Matching_Close_Token(TokenType openType, TokenType closeType)
		{
			var tokens = ValidTokens(openType, closeType);

			var result = Act(tokens, openType, closeType);

			Assert.That(tokens.Count, Is.EqualTo(2));
			Assert.That(tokens.First().Value, Is.EqualTo("op3"));
			Assert.That(tokens.Last().Value, Is.EqualTo("num4"));
		}

		[TestCase(TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES)]
		[TestCase(TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES)]
		public void TakeTokensUntilClose_Throws_ParseException_When_Matching_Close_Token_Not_Found(TokenType openType, TokenType closeType)
		{
			var invalidTokens = new TokenQueue(new[]
			{
				new Token(TokenType.NUMBER, "num1"),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op1"),
				new Token(openType),
				new Token(TokenType.NUMBER, "num2"),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op2"),
				new Token(TokenType.NUMBER, "num3"),
				new Token(closeType),
				new Token(TokenType.ARITHMETIC_OPERATOR, "op3"),
				new Token(TokenType.NUMBER, "num4")
			});

			Assert.Throws<ParseException>(() => Act(invalidTokens, openType, closeType));
		}
	}
}
