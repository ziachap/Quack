using System;
using System.Collections.Generic;

namespace Quack.File
{
	public interface ISourceSanitizer
	{
		IEnumerable<string> Sanitize(string[] lines);
	}
}