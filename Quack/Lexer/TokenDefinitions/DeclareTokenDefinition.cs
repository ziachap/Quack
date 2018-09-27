namespace Quack.Lexer.TokenDefinitions
{
	public class DeclareTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "declare";

		public Token GetToken(string term) => new Token(TokenType.DECLARE);
	}
}