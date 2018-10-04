namespace Quack.Lexer.TokenDefinitions
{
	public class DeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.DECLARE;

		public Token GetToken(string term) => new Token(TokenType.VAR_DECLARE);
	}
}