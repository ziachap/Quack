using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.SemanticAnalysis.Exceptions;

namespace Quack.SemanticAnalysis
{
	public class TypeComparer : ITypeComparer
	{
		public void AssertTypes(DebugInfo info, IEnumerable<string> actualTypes, params string[] expectedTypes)
		{
			foreach (var type in actualTypes)
			{
				AssertType(info, type, expectedTypes);
			}
		}

		public void AssertType(DebugInfo info, string actualType, params string[] expectedTypes)
		{
			if (expectedTypes.All(t => t != actualType))
			{
				throw new InvalidTypeException(info, actualType, expectedTypes);
			}
		}
	}
}