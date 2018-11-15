using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Parser;
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
		}

		public void PushBlankContext(string name = null)
		{
			var context = new DeclarationContext(name, new HashSet<IDeclaration>());
			PushContext(context);
		}

		public void PushContext(DeclarationContext context)
		{
			foreach (var scopeDeclaration in context.Declarations)
			{
				if (ExistsInScope(scopeDeclaration.Value))
				{
					// TODO: Need DebugInfo in here
					throw new DuplicateDeclarationException(new DebugInfo(), scopeDeclaration.Value);
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

		public void AssertDeclarationExists(AstNode node)
		{
			if (!ExistsInScope(node.Value))
			{
				throw new IdentifierNotDeclaredException(node.Info, node.Value);
			}
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

		public override string ToString()
		{
			return (!string.IsNullOrEmpty(Name) ? Name : "null") +
			       $" -> [{string.Join(", ", Declarations.Select(d => d.GetType().Name + " " + d.Value))}]";
		}
	}

}