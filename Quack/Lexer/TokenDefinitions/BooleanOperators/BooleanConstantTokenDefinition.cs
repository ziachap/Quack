namespace Quack.Lexer.TokenDefinitions.BooleanOperators
{
	public class BooleanConstantTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.TRUE || term == LanguageConstants.FALSE;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_CONSTANT, info, term);
	}
}