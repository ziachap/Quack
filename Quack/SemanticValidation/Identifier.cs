
using Quack.Parser;

namespace Quack.SemanticValidation
{
	public interface IDefinition
	{
		string Value { get; }
		bool ValueEquals(IDefinition other);
		bool ValueEquals(string value);
	}

	public abstract class BaseDefinition : IDefinition
	{
		protected BaseDefinition(string value)
		{
			Value = value;
		}

		public string Value { get; }
		public bool ValueEquals(IDefinition other) => string.Equals(Value, other.Value);
		public bool ValueEquals(string value) => string.Equals(Value, value);
	}

	public class VariableDefinition : BaseDefinition
	{
		public VariableDefinition(string value) : base(value)
		{
		}
	}

	public class FunctionDefinition : BaseDefinition
	{
		public FunctionDefinition(string value, AstNode statements) : base(value)
		{
			Statements = statements;
		}
		
		public AstNode Statements { get; }
	}
}
