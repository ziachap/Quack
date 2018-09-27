using System;

namespace Quack.Lexer
{
	public class TermNotSupportedException : Exception
	{
		public TermNotSupportedException(string message) : base(message)
		{
		}
	}
}