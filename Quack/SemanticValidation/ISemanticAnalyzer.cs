using Quack.Parser;

namespace Quack.SemanticValidation
{
	public interface ISemanticAnalyzer
	{
		SemanticAnalyzerResult Analyze(AstNode node);
	}
}
