using System;

namespace Quack.Transpiler
{
	public class TranspilerException : Exception
	{
		public TranspilerException(string message) : base(message)
		{
		}
	}
}