using System.Collections.Generic;
using System.Linq;
using Quack.SemanticAnalysis.Exceptions;

namespace Quack.SemanticAnalysis
{
	public class DeclarationStore
	{
		private readonly Stack<DeclarationContext> _contexts;

		public DeclarationStore()
		{
			_contexts = new Stack<DeclarationContext>();
		}

		public void AddToCurrentContext(IDeclaration declaration)
		{
			_contexts.Peek().Declarations.Add(declaration);

			if (declaration is FunctionDeclaration function)
			{
				PushContext(function);
			}
		}

		public void PushContext(FunctionDeclaration function)
		{
			var context = new DeclarationContext(function.Value, new HashSet<IDeclaration>(function.Params));
			PushContext(context);
		}

		public void PushContext(DeclarationContext context)
		{
			foreach (var scopeDeclaration in context.Declarations)
			{
				if (ExistsInScope(scopeDeclaration.Value))
				{
					throw new DuplicateDeclarationException(scopeDeclaration.Value);
				}
			}

			_contexts.Push(context);
		}
		
		public DeclarationContext PopContext()
		{
			return _contexts.Pop();
		}

		public bool ExistsInScope(string value)
		{
			return _contexts.Any(s => s.Declarations.Any(d => d.ValueEquals(value)));
		}

		public IDeclaration FindDeclaration(string value)
		{
			return _contexts.SelectMany(s => s.Declarations).Single(x => x.ValueEquals(value));
		}
	}

	public class DeclarationContext
	{
		public DeclarationContext(string name)
		{
			Name = name;
			Declarations = new HashSet<IDeclaration>();
		}

		public DeclarationContext(string name, HashSet<IDeclaration> declarations)
		{
			Name = name;
			Declarations = declarations;
		}

		public string Name { get; }
		public HashSet<IDeclaration> Declarations { get; }
	}

}