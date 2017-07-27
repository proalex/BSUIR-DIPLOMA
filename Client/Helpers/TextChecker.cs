using System;
using System.Text.RegularExpressions;

namespace Client
{
    public static class TextChecker
	{
		public static bool OnlyNumbers(string text)
		{
			Regex regex= new Regex("[0-9]+");
			return regex.IsMatch(text);
		}

		public static bool URL(string text)
		{
			Uri uriResult;
			bool result = Uri.TryCreate(text, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

			return result;
		}
	}
}
