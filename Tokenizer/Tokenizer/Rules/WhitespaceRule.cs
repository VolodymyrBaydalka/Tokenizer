using System;
using System.IO;
using System.Text;

namespace ZV.Tokenizer
{
	public class WhitespaceRule : ITokenRule
	{
		public static WhitespaceRule Default { get; private set; }

		static WhitespaceRule(){
			Default = new WhitespaceRule();
		}

		public bool TryGet (TextReader reader, StringBuilder builder)
		{
			var result = false;
			var intChar = reader.Peek ();

			while (intChar != TextTokenizer.EndOfFile && char.IsWhiteSpace ((char)intChar)) {
				builder.Append ((char)intChar);
				reader.Read ();
				intChar = reader.Peek ();
				result = true;
			}

			return result;
		}
	}
}

