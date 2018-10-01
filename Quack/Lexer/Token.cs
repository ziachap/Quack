using System;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;

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

		public override string ToString()
		{
			var type = Enum.GetName(typeof(TokenType), Type);
			var valueString = Value != null ? " : " + Value : string.Empty;
			return $"[{type}{valueString}]";
		}

		public void AssertType(TokenType expectedType)
		{
			if (Type != expectedType)
			{
				throw new ParseException($"Expected [{TokenTypeName(expectedType)}] but got {ToString()}");
			}

			string TokenTypeName(TokenType type) => Enum.GetName(typeof(TokenType), type);
		}
	}
}
