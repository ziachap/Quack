namespace Quack.Lexer.TokenDefinitions
{
	public class LambdaTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.LAMBDA;

		public Token GetToken(string term) => new Token(TokenType.LAMBDA_OPERATOR, LanguageConstants.LAMBDA);
	}
}