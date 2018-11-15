using System.Collections.Generic;
using Quack.Lexer;
using Quack.Parser;

namespace Quack.SemanticAnalysis
{
	public interface IExpressionEvaluator
	{
		string Type(AstNode exp);
	}
}
