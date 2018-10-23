namespace Quack.Lexer.TokenDefinitions
{
	public class LambdaTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.LAMBDA;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.LAMBDA_OPERATOR, info, LanguageConstants.LAMBDA);
	}
}