namespace Quack.Lexer.TokenDefinitions
{
	public class FuncDeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.FUNCTION;

		public Token GetToken(string term) => new Token(TokenType.FUNC_DECLARE);
	}
}