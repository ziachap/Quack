using System;
using Ninject;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack
{
	public class Program
	{
		static void Main(string[] args)
		{
			var kernel = Kernel();

			var compiler = kernel.Get<ICompiler>();

			compiler.Compile();

			Console.ReadKey();
		}

		private static IKernel Kernel()
		{
			var kernel = new StandardKernel();

			kernel.Bind<ICompiler>().To<Compiler>();
			kernel.Bind<ILexer>().To<Lexer.Lexer>();
			BindTokenDefinitions(kernel);

			return kernel;
		}

		private static void BindTokenDefinitions(IKernel kernel)
		{
			kernel.Bind<ITokenDefinition>().To<DeclareTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<AssignTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<ArithmeticOperatorTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<StatementEndTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<PrintTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<NumberTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<VarNameTokenDefinition>();
		}

	}
}
