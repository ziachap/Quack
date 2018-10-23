namespace Quack.Lexer.TokenDefinitions
{
	public class ArithmeticOperatorTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) =>  term == "+" || term == "-" || term == "*" || term == "/";

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.ARITHMETIC_OPERATOR, info, term);
	}
}
