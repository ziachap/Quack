using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quack.File
{
	public interface IFileReader
	{
		string LoadFromFile(string path);
	}
}
