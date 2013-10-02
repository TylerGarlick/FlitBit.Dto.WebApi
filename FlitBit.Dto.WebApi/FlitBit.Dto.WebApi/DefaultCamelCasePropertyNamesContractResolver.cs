using System.Globalization;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlitBit.Dto.WebApi
{
    public class DefaultCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var res = base.CreateProperty(member, memberSerialization);
            res.PropertyName = ToCamelCase(res.PropertyName);
            res.UnderlyingName = ToCamelCase(res.UnderlyingName);
            return res;
        }

        string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
                return s;

            var stringBuilder = new StringBuilder();
            for (var startIndex = 0; startIndex < s.Length; ++startIndex)
            {
                var flag = startIndex + 1 < s.Length;
                if (startIndex == 0 || !flag || char.IsUpper(s[startIndex + 1]))
                {
                    var ch = char.ToLower(s[startIndex], CultureInfo.InvariantCulture);
                    stringBuilder.Append(ch);
                }
                else
                {
                    stringBuilder.Append(s.Substring(startIndex));
                    break;
                }
            }

            return stringBuilder.ToString();
        }
    }
}