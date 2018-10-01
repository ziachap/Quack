using Quack.Lexer.TokenDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quack.Lexer
{
	public class TokenQueue : Queue<Token>
	{
		public TokenQueue()
		{
		}

		public TokenQueue(IEnumerable<Token> tokens) : base(tokens)
		{
		}

		public bool IsNextType(TokenType type, int index = 0) 
			=> Count > index && this.ElementAt(index).Type == type;

		public void Skip(TokenType expectedType) => Dequeue().AssertType(expectedType);
	}
}
