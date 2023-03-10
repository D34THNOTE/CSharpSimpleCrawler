using System.Text.RegularExpressions;

namespace crawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
            var url = "https://pja.edu.pl/";
            if(url.Length == 0) throw new ArgumentNullException("url");
            */
            // note to self: top right corner -> arrow next to "crawler" -> Edit Configuration.. -> Program arguments(paste url for the crawler there, using Rider)
            if (args.Length == 0) throw new ArgumentNullException(nameof(args));
            var url = args[0];

            // we set uri to absolute meaning the url contains "http" or "https", this method tries to create a Uri object and returns "false" if
            // the string is not a valid absolute URL, then we check if the url starts with "http"(which includes https)
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) || !uri.Scheme.StartsWith("http"))
            {
                throw new ArgumentException("Invalid URL", nameof(url));
            }
            var client = new HttpClient();
            
            HttpResponseMessage result = await client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Error while downloading the page");
            }
            
            if(result.IsSuccessStatusCode)
            {
                string htmlContent = await result.Content.ReadAsStringAsync();
                var regex = new Regex(@"[\w]+@[\w]+(\.[\w]{2,10}\b)" , RegexOptions.IgnoreCase);
                
                // returns a collection of objects Match
                var matches = regex.Matches(htmlContent);

                if (matches.Count == 0)
                {
                    throw new Exception("No email addresses found");
                }
                
                var uniqueMatches = new HashSet<string>();
                foreach (Match match in matches)
                {
                    uniqueMatches.Add(match.Value);
                }

                foreach (string email in uniqueMatches)
                {
                    Console.WriteLine(email);
                }
            }
        }
    }
}
