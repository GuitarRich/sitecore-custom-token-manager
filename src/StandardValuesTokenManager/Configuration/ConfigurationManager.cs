namespace StandardValuesTokenManager.Configuration
{
	using Sitecore.Configuration;

	public class ConfigurationManager
	{
		static ConfigurationManager()
		{
			Configuration = (IConfigurationProvider)Factory.CreateObject("/sitecore/standardValuesTokenManager/configurationProvider", true);
		}

		public static IConfigurationProvider Configuration { get; }
	}
}
