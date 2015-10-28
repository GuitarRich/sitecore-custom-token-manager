namespace StandardValuesTokenManager.Parsers
{
	public interface ITokenParser
	{
		/// <summary>
		/// Gets or sets the token string.
		/// </summary>
		/// <value>
		/// The token.
		/// </value>
		string Token { get; set; }

		/// <summary>
		/// Parses the input and replaces matching tokens
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		string ParseToken(string input);
	}
}
