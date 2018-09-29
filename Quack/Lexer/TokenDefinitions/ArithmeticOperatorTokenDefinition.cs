namespace Quack.Lexer.TokenDefinitions
{
	public class ArithmeticOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) =>  term == "+" || term == "-" || term == "*" || term == "/";

		public Token GetToken(string term) => new Token(TokenType.ARITHMETIC_OPERATOR, term);
	}
}
