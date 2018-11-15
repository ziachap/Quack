using System.Collections.Generic;
using Quack.Parser;

namespace Quack.SemanticAnalysis
{
	public class SemanticAnalyzerResult
	{
		public SemanticAnalyzerResult(AstNode entryNode, IEnumerable<IDeclaration> functionDeclarations)
		{
			EntryNode = entryNode;
			FunctionDeclarations = functionDeclarations;
		}

		public AstNode EntryNode { get; }
		public IEnumerable<IDeclaration> FunctionDeclarations { get; }
	}
}
