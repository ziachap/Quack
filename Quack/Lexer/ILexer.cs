namespace Quack.Lexer
{
	public interface ILexer
	{
		TokenQueue Tokenise(string[] sanitizedLines, string[] sourceLines);
	}
}