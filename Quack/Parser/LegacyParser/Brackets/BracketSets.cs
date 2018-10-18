using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser.LegacyParser.Brackets
{
	public static class BracketSets
	{
		public static readonly BracketSet Parentheses = new BracketSet(TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES);
		public static readonly BracketSet Braces = new BracketSet(TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES);
	}

	public class BracketSet
	{
		public BracketSet(TokenType openType, TokenType closeType)
		{
			OpenType = openType;
			CloseType = closeType;
		}

		public TokenType OpenType { get; }
		public TokenType CloseType { get; }
	}
}