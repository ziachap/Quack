namespace Quack.Lexer.TokenDefinitions
{
	public class DeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ValueTypes.ANY;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.VAR_DECLARE, info, "any");
	}

	public class IntTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ValueTypes.INT;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.INT, info, LanguageConstants.ValueTypes.INT);
	}

	public class BoolTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ValueTypes.BOOL;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOL, info, LanguageConstants.ValueTypes.BOOL);
	}
}
