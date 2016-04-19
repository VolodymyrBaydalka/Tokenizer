using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace ZV.Tokenizer
{
	public class TextTokenizer
	{
		internal const int EndOfFile = -1;

		private readonly TextReader _textReader;
		private readonly ITokenRule[] _rules;
		private readonly StringBuilder _buffer = new StringBuilder ();
		private readonly int _rulesLength;


		public TextTokenizer (string text, params ITokenRule[] rules)
			: this (new StringReader (text), rules)
		{
			
		}

		public TextTokenizer (TextReader reader, params ITokenRule[] rules)
		{
			_textReader = reader;
			_rules = rules;
			_rulesLength = rules.Length;
		}

		public Token NextToken ()
		{
			Token token = null;

			_buffer.Clear ();

			do {
				for (int i = 0; i < _rulesLength; i++) {
					if (_rules [i].TryGet (_textReader, _buffer)) {
						return new Token{ Rule = _rules [i], Text = _buffer.ToString () };
					}					
				}			

				_textReader.Read ();
			} while(_textReader.Peek () != EndOfFile);

			return token;
		}

		public Token NextToken (ITokenRule[] required = null, ITokenRule[] skip = null)
		{
			var token = NextToken ();

			while (token != null && skip != null && skip.Contains (token.Rule))
				token = NextToken ();
			
			if (required != null && !required.Contains (token.Rule))
				throw new TokenizerException (string.Format ("Not expected rule: {0}", token)){ Token = token };
			
			return token;
		}

		public Token[] ReadToEnd()
		{
			var list = new List<Token> ();
			var token = NextToken ();

			while (token != null) {
				list.Add (token);
				token = NextToken ();
			}

			return list.ToArray ();
		}
	}
}

