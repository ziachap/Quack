namespace Quack.Lexer.TokenDefinitions
{
	public interface ITokenDefinition
	{
		bool IsMatch(string term);
		Token MakeToken(string term, DebugInfo info);
	}

	public enum TokenType
	{
		VAR_DECLARE,
		INT,
		BOOL,
		FUNC_DECLARE,
		RETURN,
		ASSIGN,
		LAMBDA_OPERATOR,
		ARITHMETIC_OPERATOR,
		BOOLEAN_CONSTANT,
		BOOLEAN_RELATIONAL_OPERATOR,
		BOOLEAN_EQUALITY_OPERATOR,
		BOOLEAN_LOGIC_OPERATOR,
		BOOLEAN_UNARY_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		IDENTIFIER,
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