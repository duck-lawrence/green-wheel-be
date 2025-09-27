using System.Text.Json;
using System.Text.RegularExpressions;

namespace API.Policy
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // Ví dụ: FirstName -> first_name
            if (string.IsNullOrEmpty(name)) return name;

            var result = Regex.Replace(
                name,
                @"([a-z0-9])([A-Z])",
                "$1_$2"
            );

            return result.ToLower();
        }
    }
}
