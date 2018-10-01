using System.Collections.Generic;
using Quack.Lexer;

namespace Quack.Parser
{
	public interface IParser
	{
		AstNode Parse(TokenQueue tokens);
	}
}
