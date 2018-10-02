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
		FUNC_DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		BOOLEAN_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		LABEL,
		OPEN_PARENTHESES,
		CLOSE_PARENTHESES,
		OPEN_BRACES,
		CLOSE_BRACES,
		IF,
		ELSE,
		WHILE
	}
}