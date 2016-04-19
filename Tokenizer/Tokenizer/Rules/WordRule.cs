using System;
using System.IO;
using System.Text;

namespace ZV.Tokenizer
{
	public class WordRule : ITokenRule
	{
		public static WordRule Default { get; private set; }

		static WordRule(){
			Default = new WordRule();
		}
		
		public bool TryGet (TextReader reader, StringBuilder builder)
		{
			var result = false;
			var intChar = reader.Peek ();

			while (intChar != TextTokenizer.EndOfFile && char.IsLetterOrDigit ((char)intChar)) {
				builder.Append ((char)intChar);
				reader.Read ();
				intChar = reader.Peek ();
				result = true;
			}

			return result;
		}
	}
}

