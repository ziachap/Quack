using System;
using Ninject;
using Quack.File;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;
using Quack.Transpiler;

namespace Quack
{
	public class Program
	{
		static void Main(string[] args)
		{
			var kernel = Kernel();

			var compiler = kernel.Get<ICompiler>();

			compiler.Compile(args[0]);

			Console.ReadKey();
		}

		private static IKernel Kernel()
		{
			var kernel = new StandardKernel();

			kernel.Bind<ICompiler>().To<Compiler>();
			kernel.Bind<IFileReader>().To<FileReader>();
			kernel.Bind<IFileWriter>().To<FileWriter>();
			kernel.Bind<ISourceSanitizer>().To<SourceSanitizer>();
			kernel.Bind<ILexer>().To<Lexer.Lexer>();
			kernel.Bind<IParser>().To<Parser.Parser>();
			kernel.Bind<ITranspiler>().To<JavascriptTranspiler>();
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
			kernel.Bind<ITokenDefinition>().To<LabelTokenDefinition>();
		}

	}
}
