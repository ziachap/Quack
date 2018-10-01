
namespace Quack.SemanticValidation
{
	public class Identifier
	{
		public Identifier(string value, IdentifierType type)
		{
			Value = value;
			Type = type;
		}

		public string Value { get; }
		private IdentifierType Type { get; }

		public bool ValueEquals(Identifier other) => string.Equals(Value, other.Value);
		public bool ValueEquals(string value) => string.Equals(Value, value);
	}
	
	public enum IdentifierType
	{
		VARIABLE,
		FUNCTION
	}
}
