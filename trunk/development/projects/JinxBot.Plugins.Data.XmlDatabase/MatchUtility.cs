using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    internal static class MatchUtility
    {
        public static Regex CreateMetaMatch(string matchPattern, out string matchRegex)
        {
            Regex translator = new Regex(@"[][{}()*+?.\\^$|]");
            const string replacement = "\\$0";

            string escaped = translator.Replace(matchPattern, replacement);

            Regex starTranslator = new Regex(@"(?<!\\)\\\*");
            const string starReplacement = ".*";

            string temp = starTranslator.Replace(escaped, starReplacement);

            Regex questionTranslator = new Regex(@"(?<!\\)\\\?");
            const string questionReplacement = ".{1}";
            temp = questionTranslator.Replace(temp, questionReplacement);
            matchRegex = "\\A" + temp + "\\z";
            
            return new Regex(matchRegex, RegexOptions.IgnoreCase);
        }
    }
}
