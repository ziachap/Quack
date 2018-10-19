namespace Quack.Lexer.TokenDefinitions
{
	public interface ITokenDefinition
	{
		bool IsMatch(string term);
		Token GetToken(string term);
	}

	public enum TokenType
	{
		VAR_DECLARE,
		INT,
		BOOL,
		FUNC_DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		BOOLEAN_RELATIONAL_OPERATOR,
		BOOLEAN_EQUALITY_OPERATOR,
		BOOLEAN_LOGIC_OPERATOR,
		BOOLEAN_UNARY_OPERATOR,
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
		WHILE,
		PARAM_DELIMITER
	}
}