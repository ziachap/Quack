using Quack.Parser;

namespace Quack.SemanticValidation
{
	public interface ISemanticValidatorService
	{
		void Validate(AstNode node);
	}
}
