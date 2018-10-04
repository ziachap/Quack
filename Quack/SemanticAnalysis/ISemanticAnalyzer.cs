using Quack.Parser;
using Quack.SemanticValidation;

namespace Quack.SemanticAnalysis
{
	public interface ISemanticAnalyzer
	{
		SemanticAnalyzerResult Analyze(AstNode node);
	}
}
