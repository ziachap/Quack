using Quack.Parser;

namespace Quack.Transpiler
{
	public interface ITranspiler
	{
		string Transpile(AstNode rootNode);
	}
}
