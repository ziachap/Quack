namespace Quack.Lexer.TokenDefinitions
{
	public class DeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.DECLARE;

		public Token GetToken(string term) => new Token(TokenType.VAR_DECLARE, "any");
	}

	public class IntTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.INT;

		public Token GetToken(string term) => new Token(TokenType.INT, LanguageConstants.INT);
	}

	public class BoolTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.BOOL;

		public Token GetToken(string term) => new Token(TokenType.BOOL, LanguageConstants.BOOL);
	}
}