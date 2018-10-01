using System.Collections.Generic;
using System.Linq;
using Quack.Parser;
using Quack.SemanticValidation.Exceptions;

namespace Quack.SemanticValidation
{
	public class SemanticValidatorService : ISemanticValidatorService
	{
		private readonly HashSet<Identifier> _identifiers;

		public SemanticValidatorService()
		{
			_identifiers = new HashSet<Identifier>();
		}

		public void Validate(AstNode node)
		{
			ValidateNode(node);
			
			foreach (var child in node.Children)
			{
				Validate(child);
			}
		}

		private void ValidateNode(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
					Declare(node);
					break;
				case AstNodeType.LABEL:
					Label(node);
					break;
			}
		}

		private void Declare(AstNode node)
		{
			var label = node.Value;
			if (_identifiers.Any(i => i.ValueEquals(node.Value)))
			{
				throw new DuplicateDeclarationException(label);
			}
			_identifiers.Add(Variable(label));
		}

		private void Label(AstNode node)
		{
			if (!_identifiers.Any(i => i.ValueEquals(node.Value)))
			{
				throw new LabelNotDeclaredException(node.Value);
			}
		}

		private static Identifier Variable(string label) 
			=> new Identifier(label, IdentifierType.VARIABLE);
	}
}