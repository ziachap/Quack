using System.Collections.Generic;
using Quack.Parser;

namespace Quack.SemanticValidation
{
	public class SemanticAnalyzerResult
	{
		public SemanticAnalyzerResult(AstNode entryNode, IEnumerable<IDefinition> functionDeclarations)
		{
			EntryNode = entryNode;
			FunctionDeclarations = functionDeclarations;
		}

		public AstNode EntryNode { get; }
		public IEnumerable<IDefinition> FunctionDeclarations { get; }
	}
}
