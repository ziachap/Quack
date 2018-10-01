namespace Quack.Lexer
{
	public interface ILexer
	{
		TokenQueue Tokenise(string input);
	}
}