using Quack.Lexer.TokenDefinitions;

namespace Quack.Lexer
{
	public class Token
	{
		public Token(TokenType type, string value = null)
		{
			Type = type;
			Value = value;
		}

		public TokenType Type { get; }
		public string Value { get; }
	}
}