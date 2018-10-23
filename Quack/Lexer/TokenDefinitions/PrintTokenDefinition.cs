namespace Quack.Lexer.TokenDefinitions
{
	public class PrintTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.PRINT;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.PRINT, info);
	}
}