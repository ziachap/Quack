namespace Quack.Lexer.TokenDefinitions.BooleanOperators
{
	public class BooleanRelationalOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) 
			=> term == ">" 
			   || term == ">="
			   || term == "<"
			   || term == "<=";
		
		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_RELATIONAL_OPERATOR, term);
	}

	public class BooleanEqualityOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term)
			=> term == "!=" || term == "==";

		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_EQUALITY_OPERATOR, term);
	}

	public class BooleanLogicOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term)
			=>  term == "&&" || term == "||";

		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_LOGIC_OPERATOR, term);
	}

	public class BooleanPrefixOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "!";

		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_UNARY_OPERATOR, term);
	}
}