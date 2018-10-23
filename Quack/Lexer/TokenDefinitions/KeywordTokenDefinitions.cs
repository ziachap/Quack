namespace Quack.Lexer.TokenDefinitions
{
	public class FuncDeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.FUNCTION;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.FUNC_DECLARE, info);
	}

	public class ReturnTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.RETURN;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.RETURN, info);
	}

	public class PrintTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.PRINT;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.PRINT, info);
	}
}