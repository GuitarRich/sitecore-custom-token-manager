namespace StandardValuesTokenManager.Configuration
{
	using StandardValuesTokenManager.Configuration.Parsers;

	public interface IConfigurationProvider
	{
		ITokenParserConfiguration[] CommandParserProviders { get; }

		/// <summary>
		/// Gets the configuration database.
		/// </summary>
		/// <value>The configuration database.</value>
		string ConfigurationDatabase { get; }

		/// <summary>
		/// Gets the module settings item path.
		/// </summary>
		/// <value>
		/// The module settings item path.
		/// </value>
		string ModuleSettingsItem { get; }

		/// <summary>
		/// Gets the tokens folder path.
		/// </summary>
		/// <value>
		/// The tokens folder path.
		/// </value>
		string TokensFolder { get; }

	}
}
