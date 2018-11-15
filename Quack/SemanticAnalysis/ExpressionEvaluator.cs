using System;
using System.Linq;
using Quack.Parser;
using Quack.SemanticAnalysis.Exceptions;

namespace Quack.SemanticAnalysis
{
	public class ExpressionEvaluator : IExpressionEvaluator
	{
		private readonly DeclarationStore _declarations;
		private readonly ITypeComparer _typeComparer;

		public ExpressionEvaluator(DeclarationStore declarations, ITypeComparer typeComparer)
		{
			_declarations = declarations;
			_typeComparer = typeComparer;
		}

		public string Type(AstNode expr)
		{
			switch (expr.Type)
			{
				case AstNodeType.ARITHMETIC_OPERATOR:
					_typeComparer.AssertTypes(expr.Info, expr.Children.Select(Type), LanguageConstants.ValueTypes.INT);
					return expr.TypeIdentifier;

				case AstNodeType.BOOLEAN_RELATIONAL_OPERATOR:
				case AstNodeType.BOOLEAN_EQUALITY_OPERATOR:
					var leftType = Type(expr.Children.First());
					_typeComparer.AssertType(expr.Info, leftType, LanguageConstants.ValueTypes.BOOL, LanguageConstants.ValueTypes.INT);
					var rightType = Type(expr.Children.ElementAt(1));
					_typeComparer.AssertType(expr.Info, rightType, leftType);
					return expr.TypeIdentifier;

				case AstNodeType.BOOLEAN_LOGIC_OPERATOR:
					_typeComparer.AssertTypes(expr.Info, expr.Children.Select(Type), LanguageConstants.ValueTypes.BOOL);
					return expr.TypeIdentifier;

				case AstNodeType.BOOLEAN_UNARY_OPERATOR:
					_typeComparer.AssertType(expr.Info, Type(expr.Children.Single()), LanguageConstants.ValueTypes.BOOL);
					return expr.TypeIdentifier;

				case AstNodeType.BOOLEAN_CONSTANT:
				case AstNodeType.NUMBER:
					return expr.TypeIdentifier;

				case AstNodeType.FACTOR:
					return Type(expr.Children.Single());

				case AstNodeType.IDENTIFIER:
					return LookupIdentifierType(expr);

				case AstNodeType.FUNC_INVOKE:
					throw new NotImplementedException();

				default:
					throw new BaseLanguageException(expr.Info, "Unexpected AstNodeType in expression");
			}
		}

		private string LookupIdentifierType(AstNode identifier)
		{
			_declarations.AssertDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);
			return declaration.TypeIdentifier;
		}
	}
}