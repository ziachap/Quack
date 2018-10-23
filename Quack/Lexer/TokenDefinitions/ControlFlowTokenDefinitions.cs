namespace Quack.Lexer.TokenDefinitions
{
	public class IfTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.IF;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.IF, info);
	}

	public class ElseTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ELSE;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.ELSE, info);
	}

	public class WhileTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.WHILE;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.WHILE, info);
	}
}