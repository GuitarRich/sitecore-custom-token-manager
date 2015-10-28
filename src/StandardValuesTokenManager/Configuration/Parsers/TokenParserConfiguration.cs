namespace StandardValuesTokenManager.Configuration.Parsers
{
	using System;
	using System.Configuration;

	using StandardValuesTokenManager.Parsers;

	public class TokenParserConfiguration : ITokenParserConfiguration
	{
		protected Type CommandType;

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenParserConfiguration"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="typeName">Name of the type.</param>
		public TokenParserConfiguration(string name, string typeName)
		{
			this.Name = name;
			this.TypeName = typeName;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the name of the type.
		/// </summary>
		/// <value>
		/// The name of the type.
		/// </value>
		public string TypeName { get; set; }

		/// <summary>
		/// Gets the instance ot the <see cref="ITokenParser"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		/// <exception cref="ConfigurationErrorsException">The token parser type was not found</exception>
		public T GetInstance<T>() where T : ITokenParser
		{
			if (this.CommandType == null)
			{
				this.CommandType = Type.GetType(this.TypeName);
			}

			if (this.CommandType == null)
			{
				throw new ConfigurationErrorsException("The token parser type was not found");
			}

			var parser = (T)Activator.CreateInstance(this.CommandType);
			if (parser != null)
			{
				parser.Token = this.Name;
			}

			return parser;
		}
	}
}
