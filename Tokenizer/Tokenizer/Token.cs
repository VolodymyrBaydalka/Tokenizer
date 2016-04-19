using System;

namespace ZV.Tokenizer
{
	public class Token
	{
		public ITokenRule Rule { get; set; }
		public string Text { get; set; }

		public override string ToString ()
		{
			return string.Format ("[{0}] {1}", Rule, Text);
		}
	}
}

