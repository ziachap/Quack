namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class CloseBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "}";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.CLOSE_BRACES, info, term);
	}
}