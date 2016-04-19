using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ZV.Tokenizer;
using System.Collections;

namespace Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
			var text = "This is  {test} \"te\\\"xt\".";
			var tokenizer = new TextTokenizer (text, 
				WordRule.Default, WhitespaceRule.Default, StringRule.Default, 
				new SingleCharRule ('{'), new SingleCharRule ('}'), new SingleCharRule ('.'));

			var tokens = tokenizer.ReadToEnd ();

			Assert.AreEqual (tokens [0].Rule, WordRule.Default);
			Assert.AreEqual (tokens [1].Rule, WhitespaceRule.Default);
			Assert.AreEqual (tokens [2].Rule.GetType(), typeof(WordRule));
			Assert.AreEqual (tokens [3].Text, "  ");
			Assert.AreEqual (tokens [4].Rule.GetType(), typeof(SingleCharRule));
			Assert.AreEqual (tokens [5].Text, "test");
			Assert.AreEqual (tokens [6].Text, "}");
			Assert.AreEqual (tokens [7].Rule.GetType(), typeof(WhitespaceRule));
			Assert.AreEqual (tokens [8].Rule, StringRule.Default);
			Assert.AreEqual (tokens [8].Text, "te\"xt");
		}

		[Test ()]
		public void TestJSONParser(){
			
			var jsonText = "{ a : 10, b: 'text', c : [ 1, 'a'], d: { a: 10, b: 'text', c: true }}";
			var json = new SimpleJSON ().Parse (jsonText) as IDictionary;

			Assert.AreEqual (json ["a"], 10.0d);
		}

		class SimpleJSON {
			ITokenRule startObject = new SingleCharRule ('{');
			ITokenRule endObject = new SingleCharRule ('}');
			ITokenRule startArray = new SingleCharRule ('[');
			ITokenRule endArray = new SingleCharRule (']');
			ITokenRule colon = new SingleCharRule (':');
			ITokenRule comma = new SingleCharRule (',');
			ITokenRule word = new WordRule ();
			ITokenRule text = new StringRule ();

			public object Parse(string input)
			{
				var tokenizer = new TextTokenizer (input, colon, comma, startArray, endArray, startObject, endObject, text, word);

				return ParseValue (tokenizer, null);
			}

			public object ParseValue(TextTokenizer tokenizer, ITokenRule endToken)
			{
				var required = endToken == null ? new []{ word, text, startObject, startArray } : new []{ word, text, startObject, startArray, endToken };
					
				var token = tokenizer.NextToken (required: required);

				if (token.Rule == word) {
					var boolValue = false;
					var numberValue = 0.0d;

					if (bool.TryParse (token.Text, out boolValue))
						return boolValue;

					if (double.TryParse (token.Text, out numberValue))
						return numberValue;

					throw new NotSupportedException ("not supported token");
				} else if (token.Rule == text) {
					return token.Text;
				} else if (token.Rule == startArray) {
					var list = new List<object> ();

					object val = null;

					while ((val = ParseValue (tokenizer, endArray)) != null) {
						list.Add (val);

						if (tokenizer.NextToken (required: new []{ endArray, comma }).Rule == endArray)
							break;
					}

					return list.ToArray ();
				} else if (token.Rule == startObject) {
					var obj = new Dictionary<string, object> ();

					do {
						var prop = tokenizer.NextToken (new []{ word, endObject });

						if(prop.Rule == endObject)
							break;

						tokenizer.NextToken (new []{ colon });

						obj.Add(prop.Text, ParseValue(tokenizer, null));

						token = tokenizer.NextToken (new []{ comma, endObject });

					} while(token.Rule != endObject);

					return obj;
				}

				return null;
			}
		}
	}
}

