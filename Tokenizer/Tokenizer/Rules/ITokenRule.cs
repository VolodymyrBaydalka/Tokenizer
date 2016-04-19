using System;
using System.IO;
using System.Text;

namespace ZV.Tokenizer
{
	public interface ITokenRule
	{
		bool TryGet(TextReader reader, StringBuilder builder);
	}
}

