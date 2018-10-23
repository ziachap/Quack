namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class OpenBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "{";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.OPEN_BRACES, info, term);
	}
}