namespace Quack.Lexer.TokenDefinitions
{
	public class FuncDeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.FUNCTION;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.FUNC_DECLARE, info);
	}
}