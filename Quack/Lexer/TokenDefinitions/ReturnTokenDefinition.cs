namespace Quack.Lexer.TokenDefinitions
{
	public class ReturnTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.RETURN;

		public Token GetToken(string term) => new Token(TokenType.RETURN);
	}
}