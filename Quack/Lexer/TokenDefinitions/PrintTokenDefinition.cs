namespace Quack.Lexer.TokenDefinitions
{
	public class PrintTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.PRINT;

		public Token GetToken(string term) => new Token(TokenType.PRINT);
	}
}