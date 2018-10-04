namespace Quack.Lexer.TokenDefinitions
{
	public class ParamDelimeterTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.PARAM_DELIMITER;

		public Token GetToken(string term) => new Token(TokenType.PARAM_DELIMITER);
	}
}