using System;
using System.IO;
using System.Text;

namespace ZV.Tokenizer
{
	public class SingleCharRule : ITokenRule
	{
		public char Char { get; set; }

		public SingleCharRule()
		{
		}

		public SingleCharRule(char c)
		{
			this.Char = c;
		}

		public bool TryGet (TextReader reader, StringBuilder builder)
		{
			var result = false;
			var intChar = reader.Peek ();

			if (intChar != TextTokenizer.EndOfFile && this.Char == (char)intChar) {
				builder.Append ((char)intChar);
				reader.Read ();
				result = true;
			}

			return result;
		}
	}
}

