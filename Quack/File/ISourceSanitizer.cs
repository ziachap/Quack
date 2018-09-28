using System;

namespace Quack.File
{
	public interface ISourceSanitizer
	{
		string Sanitize(string input);
	}
}