namespace Quack.Lexer.TokenDefinitions
{
	public class StatementEndTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.STATEMENT_END;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.STATEMENT_END, info);
	}
}