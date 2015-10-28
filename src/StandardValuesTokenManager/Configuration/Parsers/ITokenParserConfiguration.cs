namespace StandardValuesTokenManager.Configuration.Parsers
{
	using StandardValuesTokenManager.Parsers;

	public interface ITokenParserConfiguration
	{
		string Name { get; set; }
		string TypeName { get; set; }

		T GetInstance<T>() where T : ITokenParser;
	}
}
