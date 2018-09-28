namespace Quack.Lexer.TokenDefinitions
{
	public class StatementEndTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.STATEMENT_END;

		public Token GetToken(string term) => new Token(TokenType.STATEMENT_END);
	}
}