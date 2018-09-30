namespace Quack.Lexer.TokenDefinitions
{
	public class BooleanOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) 
			=> term == ">" 
			   || term == ">="
			   || term == "<"
			   || term == "<=" 
			   || term == "!=" 
			   || term == "==";
		
		public Token GetToken(string term) => new Token(TokenType.BOOLEAN_OPERATOR, term);
	}
}