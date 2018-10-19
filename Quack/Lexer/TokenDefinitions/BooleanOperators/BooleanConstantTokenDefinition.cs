namespace Quack.Lexer.TokenDefinitions.BooleanOperators
{
	public class BooleanConstantTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.TRUE || term == LanguageConstants.FALSE;

		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_CONSTANT, term);
	}
}