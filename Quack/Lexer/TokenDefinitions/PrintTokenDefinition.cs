namespace Quack.Lexer.TokenDefinitions
{
	public class PrintTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "print";

		public Token GetToken(string term) => new Token(TokenType.PRINT);
	}
}