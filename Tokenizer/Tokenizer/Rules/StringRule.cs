using System;
using System.IO;
using System.Text;
using System.Linq;

namespace ZV.Tokenizer
{
	public class StringRule : ITokenRule
	{
		private static readonly char[] _borders = new []{ '\'', '"' };
		private const char _spec = '\\';

		public static StringRule Default { get; private set; }

		static StringRule(){
			Default = new StringRule();
		}
		
		public bool TryGet (TextReader reader, StringBuilder builder)
		{
			var result = false;
			var intChar = reader.Peek ();

			if (intChar != TextTokenizer.EndOfFile && _borders.Contains ((char)intChar)) {
				var border = reader.Read ();
				var spec = false;
				result = true;

				while ((intChar = reader.Read()) != border || spec) {
					if (intChar == TextTokenizer.EndOfFile)
						throw new TokenizerException ("Unexpected end of text");

					var c = (char)intChar;

					if (spec) {
						builder.Append (c);
						spec = false;
					} else if (c == _spec) {
						spec = true;
					} else {
						builder.Append (c);
					}
				}
			}

			return result;
		}
	}
}

