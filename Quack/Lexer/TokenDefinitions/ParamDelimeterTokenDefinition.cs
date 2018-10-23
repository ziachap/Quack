namespace Quack.Lexer.TokenDefinitions
{
	public class ParamDelimeterTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.PARAM_DELIMITER;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.PARAM_DELIMITER, info);
	}
}