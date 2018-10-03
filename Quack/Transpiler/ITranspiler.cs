using Quack.SemanticValidation;

namespace Quack.Transpiler
{
	public interface ITranspiler
	{
		string Transpile(SemanticAnalyzerResult rootNode);
	}
}
