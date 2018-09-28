using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quack.Parser;

namespace Quack.Transpiler
{
	public interface ITranspiler
	{
		string Transpile(AstNode rootNode);
	}

	public class TranspilerException : Exception
	{
		public TranspilerException(string message) : base(message)
		{
		}
	}
}
