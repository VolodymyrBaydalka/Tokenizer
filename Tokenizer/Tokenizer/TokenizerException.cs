using System;

namespace ZV.Tokenizer
{
	public class TokenizerException : Exception
	{
		public Token Token { get; set; }
		
		public TokenizerException (string message) : base(message)
		{
		}

		public TokenizerException (string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

