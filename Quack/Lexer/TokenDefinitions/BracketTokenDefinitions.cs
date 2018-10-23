namespace Quack.Lexer.TokenDefinitions
{
	public class OpenParenthesesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "(";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.OPEN_PARENTHESES, info, term);
	}

	public class CloseParenthesesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == ")";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.CLOSE_PARENTHESES, info, term);
	}

	public class OpenBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "{";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.OPEN_BRACES, info, term);
	}

	public class CloseBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "}";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.CLOSE_BRACES, info, term);
	}
}