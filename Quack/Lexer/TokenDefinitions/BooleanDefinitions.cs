namespace Quack.Lexer.TokenDefinitions
{
	public class BooleanRelationalOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) 
			=> term == ">" 
			   || term == ">="
			   || term == "<"
			   || term == "<=";
		
		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_RELATIONAL_OPERATOR, info, term);
	}

	public class BooleanEqualityOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term)
			=> term == "!=" || term == "==";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_EQUALITY_OPERATOR, info, term);
	}

	public class BooleanLogicOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term)
			=>  term == "&&" || term == "||";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_LOGIC_OPERATOR, info, term);
	}

	public class BooleanUnaryOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "!";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_UNARY_OPERATOR, info, term);
	}
	
	public class BooleanConstantTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.TRUE || term == LanguageConstants.FALSE;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.BOOLEAN_CONSTANT, info, term);
	}
}