using System.Collections.Generic;
using Quack.Lexer;

namespace Quack.SemanticAnalysis
{
	public interface ITypeComparer
	{
		void AssertTypes(DebugInfo info, IEnumerable<string> actualTypes, params string[] expectedTypes);
		void AssertType(DebugInfo info, string actualType, params string[] expectedTypes);
	}
}
