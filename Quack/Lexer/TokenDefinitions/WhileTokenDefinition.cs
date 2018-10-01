namespace Quack.Lexer.TokenDefinitions
{
	public class WhileTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.WHILE;

		public Token GetToken(string term) => new Token(TokenType.WHILE);
	}
}