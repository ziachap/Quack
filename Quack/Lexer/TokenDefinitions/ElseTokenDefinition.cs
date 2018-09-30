namespace Quack.Lexer.TokenDefinitions
{
	public class ElseTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ELSE;

		public Token GetToken(string term) => new Token(TokenType.ELSE);
	}
}