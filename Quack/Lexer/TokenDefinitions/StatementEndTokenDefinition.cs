namespace Quack.Lexer.TokenDefinitions
{
	public class StatementEndTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == ";";

		public Token GetToken(string term) => new Token(TokenType.STATEMENT_END);
	}
}