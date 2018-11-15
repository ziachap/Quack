using Quack.Parser;

namespace Quack.SemanticAnalysis
{
	public interface ISemanticAnalyzer
	{
		SemanticAnalyzerResult Analyze(AstNode node);
	}
}
