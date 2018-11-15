using Quack.SemanticAnalysis;

namespace Quack.Transpiler
{
	public interface ITranspiler
	{
		string Transpile(SemanticAnalyzerResult rootNode);
	}
}
