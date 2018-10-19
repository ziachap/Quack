
using System;
using System.Collections.Generic;
using Quack.Parser;

namespace Quack.SemanticAnalysis
{
	public interface IDeclaration
	{
		string Value { get; }
		bool ValueEquals(IDeclaration other);
		bool ValueEquals(string value);
	}

	public abstract class BaseDeclaration : IDeclaration
	{
		protected BaseDeclaration(string value)
		{
			Value = value;
		}

		public string Value { get; }
		public bool ValueEquals(IDeclaration other) => string.Equals(Value, other.Value);
		public bool ValueEquals(string value) => string.Equals(Value, value);
	}

	public class VariableDeclaration : BaseDeclaration
	{
		public VariableDeclaration(string value, string typeIdentifier) : base(value)
		{
			TypeIdentifier = typeIdentifier;
			IsImplicitlyTyped = typeIdentifier == LanguageConstants.ValueTypes.ANY;
		}

		public string TypeIdentifier { get; private set; }

		public bool IsImplicitlyTyped { get; }

		public void ImplicitSetType(string type)
		{
			if (IsImplicitlyTyped)
			{
				TypeIdentifier = type;
			}
			else
			{
				throw new Exception($"Tried to implicitly set type of explicitly typed variable");
			}
		}
	}

	public class FunctionDeclaration : BaseDeclaration
	{
		public FunctionDeclaration(string value, AstNode statements) : base(value)
		{
			Statements = statements;
			Params = new HashSet<IDeclaration>();
		}
		
		public AstNode Statements { get; }

		public HashSet<IDeclaration> Params { get; set; }
	}
}
