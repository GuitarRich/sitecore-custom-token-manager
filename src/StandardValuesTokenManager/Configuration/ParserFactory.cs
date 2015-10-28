namespace StandardValuesTokenManager.Configuration
{
	using StandardValuesTokenManager.Configuration.Parsers;

	public class ParserFactory
	{
		public static ITokenParserConfiguration GetTokenParserConfiguration(string name, string typeName)
		{
			return new TokenParserConfiguration(name, typeName);
		}
	}
}
