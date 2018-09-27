namespace Quack.Lexer.TokenDefinitions
{
	public interface ITokenDefinition
	{
		bool IsMatch(string term);
		Token GetToken(string term);
	}

	public enum TokenType
	{
		DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		VAR_NAME
	}
}