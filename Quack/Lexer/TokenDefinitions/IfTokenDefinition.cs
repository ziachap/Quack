namespace Quack.Lexer.TokenDefinitions
{
	public class IfTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.IF;

		public Token GetToken(string term) => new Token(TokenType.IF);
	}
}