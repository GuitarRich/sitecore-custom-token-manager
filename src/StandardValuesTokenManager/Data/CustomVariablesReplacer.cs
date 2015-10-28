namespace StandardValuesTokenManager.Data
{
	using System;
	using System.Linq;
	using System.Text.RegularExpressions;

	using Sitecore.Data;
	using Sitecore.Data.Items;

	using StandardValuesTokenManager.Configuration;
	using StandardValuesTokenManager.Parsers;

	public class CustomVariablesReplacer : MasterVariablesReplacer
	{
		public override string Replace(string text, Item targetItem)
		{
			Sitecore.Diagnostics.Assert.ArgumentNotNull(text, "text");
			Sitecore.Diagnostics.Assert.ArgumentNotNull(targetItem, "targetItem");

			// Check to see if there is a matching item in the module settings
			var database = Sitecore.Configuration.Factory.GetDatabase(ConfigurationManager.Configuration.ConfigurationDatabase);
			var tokensFolder = database.GetItem(ConfigurationManager.Configuration.TokensFolder);

			// Get all the tokens in the field
			var matches = Regex.Matches(text, @"(\$\w+)");

			for (var i = 0; i < matches.Count; i++)
			{
				var match = matches[i];

				var customTokenParser =
					tokensFolder.Children.FirstOrDefault(
						child => string.Equals(child.Name, match.Value.Replace("$", string.Empty), StringComparison.InvariantCultureIgnoreCase));

				if (customTokenParser != null)
				{
					var tokenParserType = customTokenParser["Token Parser Type"];
					if (string.IsNullOrWhiteSpace(tokenParserType))
					{
						continue;
					}

					var tokenParserConfiguration = ParserFactory.GetTokenParserConfiguration(
						customTokenParser["Token Parser Name"],
						customTokenParser["Token Parser Type"]);
					var tokenParser = tokenParserConfiguration.GetInstance<ITokenParser>();
					text = tokenParser?.ParseToken(text);
				}
			}

			// Finally - pass through to the standard Sitecore token parser 
			// to pick up any OOTB tokens
			return base.Replace(text, targetItem);
		}
	}
}