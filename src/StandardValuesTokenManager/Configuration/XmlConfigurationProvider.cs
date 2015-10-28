/*
 *
 * Configuration Provider and setup inspired by the genius of https://github.com/kamsar
 *
 */
namespace StandardValuesTokenManager.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Xml;

	using Sitecore.Configuration;
	using Sitecore.Diagnostics;

	using StandardValuesTokenManager.Configuration.Parsers;

	/// <summary>
	/// Reads the dependency configurations from XML (e.g. Sitecore web.config section sitecore/standardValuesTokenManager)
	/// </summary>
	public class XmlConfigurationProvider : IConfigurationProvider
	{
		private ITokenParserConfiguration[] commandParserProviders;

		/// <summary>
		/// Gets the command parser providers.
		/// </summary>
		/// <value>
		/// The command parser providers.
		/// </value>
		public ITokenParserConfiguration[] CommandParserProviders
		{
			get
			{
				if (this.commandParserProviders == null)
				{
					this.LoadConfiguration();
				}

				return this.commandParserProviders;
			}
		}

		/// <summary>
		/// Gets the configuration node.
		/// </summary>
		/// <returns></returns>
		protected virtual XmlNode GetConfigurationNode()
		{
			return Factory.GetConfigNode("/sitecore/standardValuesTokenManager");
		}

		/// <summary>
		/// Gets the configuration database.
		/// </summary>
		/// <value>The configuration database.</value>
		public string ConfigurationDatabase => Settings.GetSetting("SVTM.ConfigurationDatabase", "master");

		/// <summary>
		/// Gets the module settings item path.
		/// </summary>
		/// <value>
		/// The module settings item path.
		/// </value>
		public string ModuleSettingsItem => Settings.GetSetting("SVTM.SettingsItemPath", "/sitecore/system/Modules/StandardValuesTokenManager/Configuration/Settings");

		/// <summary>
		/// Gets the tokens folder path.
		/// </summary>
		/// <value>
		/// The tokens folder path.
		/// </value>
		public string TokensFolder => Settings.GetSetting("SVTM.TokensFolder", "/sitecore/system/Modules/StandardValuesTokenManager/Tokens");

		/// <summary>
		/// Loads the configuration.
		/// </summary>
		/// <exception cref="InvalidOperationException">The TokenParserProvider ' + configuration.Name + ' is defined twice. Configurations should have unique names.</exception>
		protected virtual void LoadConfiguration()
		{
			var configNode = this.GetConfigurationNode();
			Assert.IsNotNull(configNode, "Root standardValuesTokenManager config node not found. Missing StandardValuesTokenManager.config?");

			var configurationNodes = configNode.SelectNodes("./tokenParsers/commandParser");

			// no configs let's get outta here
			if (configurationNodes == null || configurationNodes.Count == 0)
			{
				this.commandParserProviders = new ITokenParserConfiguration[0];
				return;
			}

			var providers = new Collection<ITokenParserConfiguration>();
			var nameChecker = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (XmlElement element in configurationNodes)
			{
				var configuration = this.LoadTokenParserConfigurtation(element);

				if (nameChecker.Contains(configuration.Name))
				{
					throw new InvalidOperationException("The TokenParserProvider '" + configuration.Name + "' is defined twice. Configurations should have unique names.");
				}
				nameChecker.Add(configuration.Name);

				providers.Add(configuration);
			}

			this.commandParserProviders = providers.ToArray();
		}

		/// <summary>
		/// Loads the token parser configurtation.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <returns></returns>
		protected virtual ITokenParserConfiguration LoadTokenParserConfigurtation(XmlElement configuration)
		{
			var name = this.GetAttributeValue(configuration, "name");

			Assert.IsNotNullOrEmpty(name, "CommandParserProvider node had empty or missing name attribute.");

			var type = this.GetAttributeValue(configuration, "type");
			Assert.IsNotNullOrEmpty(type, "Command node had empty or missing type attribute.");

			return new TokenParserConfiguration(name, type);
		}

		/// <summary>
		/// Gets an XML attribute value, returning null if it does not exist and its inner text otherwise.
		/// </summary>
		protected virtual string GetAttributeValue(XmlNode node, string attribute)
		{
			return node?.Attributes?[attribute]?.InnerText;
		}
	}
}