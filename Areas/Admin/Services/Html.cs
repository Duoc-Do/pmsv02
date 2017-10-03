using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Areas.Admin.Services.CodeFormatter;

namespace WebApp.Areas.Admin.Services
{

    /// <summary>
    /// Represents a ResolveLinks helper
    /// </summary>
    public partial class ResolveLinksHelper
    {
        #region Fields
        /// <summary>
        /// The regular expression used to parse links.
        /// </summary>
        private static readonly Regex regex = new Regex("((http://|https://|www\\.)([A-Z0-9.\\-]{1,})\\.[0-9A-Z?;~&\\(\\)#,=\\-_\\./\\+]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string link = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";
        private const int MAX_LENGTH = 50;
        #endregion

        #region Utilities

        /// <summary>
        /// Shortens any absolute URL to a specified maximum length
        /// </summary>
        private static string ShortenUrl(string url, int max)
        {
            if (url.Length <= max)
                return url;

            // Remove the protocal
            int startIndex = url.IndexOf("://");
            if (startIndex > -1)
                url = url.Substring(startIndex + 3);

            if (url.Length <= max)
                return url;

            // Compress folder structure
            int firstIndex = url.IndexOf("/") + 1;
            int lastIndex = url.LastIndexOf("/");
            if (firstIndex < lastIndex)
            {
                url = url.Remove(firstIndex, lastIndex - firstIndex);
                url = url.Insert(firstIndex, "...");
            }

            if (url.Length <= max)
                return url;

            // Remove URL parameters
            int queryIndex = url.IndexOf("?");
            if (queryIndex > -1)
                url = url.Substring(0, queryIndex);

            if (url.Length <= max)
                return url;

            // Remove URL fragment
            int fragmentIndex = url.IndexOf("#");
            if (fragmentIndex > -1)
                url = url.Substring(0, fragmentIndex);

            if (url.Length <= max)
                return url;

            // Compress page
            firstIndex = url.LastIndexOf("/") + 1;
            lastIndex = url.LastIndexOf(".");
            if (lastIndex - firstIndex > 10)
            {
                string page = url.Substring(firstIndex, lastIndex - firstIndex);
                int length = url.Length - max + 3;
                if (page.Length > length)
                    url = url.Replace(page, "..." + page.Substring(length));
            }

            return url;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            var info = CultureInfo.InvariantCulture;
            foreach (Match match in regex.Matches(text))
            {
                if (!match.Value.Contains("://"))
                {
                    text = text.Replace(match.Value, string.Format(info, link, "http://", match.Value, ShortenUrl(match.Value, MAX_LENGTH)));
                }
                else
                {
                    text = text.Replace(match.Value, string.Format(info, link, string.Empty, match.Value, ShortenUrl(match.Value, MAX_LENGTH)));
                }
            }

            return text;
        }
        #endregion
    }

    /// <summary>
    /// Handles all of the options for changing the rendered code.
    /// </summary>
    public partial class HighlightOptions
    {
        public string Code { get; set; }
        public bool DisplayLineNumbers { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public bool AlternateLineNumbers { get; set; }
    }

    /// <summary>
    /// Represents a code format helper
    /// </summary>
    public partial class CodeFormatHelper
    {
        #region Fields
        //private static Regex regexCode1 = new Regex(@"(?<begin>\[code:(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;alt=(?<altlinenumbers>(?:on|off)))?(?:;(?<title>.*?))?\])(?<code>.*?)(?<end>\[/code\])", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private readonly static Regex regexHtml = new Regex("<[^>]*>", RegexOptions.Compiled);
        private readonly static Regex regexCode2 = new Regex(@"\[code\](?<inner>(.*?))\[/code\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Utilities

        /// <summary>
        /// Code evaluator method
        /// </summary>
        /// <param name="match">Match</param>
        /// <returns>Formatted text</returns>
        private static string CodeEvaluator(Match match)
        {
            if (!match.Success)
                return match.Value;

            var options = new HighlightOptions();

            options.Language = match.Groups["lang"].Value;
            options.Code = match.Groups["code"].Value;
            options.DisplayLineNumbers = match.Groups["linenumbers"].Value == "on" ? true : false;
            options.Title = match.Groups["title"].Value;
            options.AlternateLineNumbers = match.Groups["altlinenumbers"].Value == "on" ? true : false;

            string result = match.Value.Replace(match.Groups["begin"].Value, "");
            result = result.Replace(match.Groups["end"].Value, "");
            result = Highlight(options, result);
            return result;

        }

        /// <summary>
        /// Code evaluator method
        /// </summary>
        /// <param name="match">Match</param>
        /// <returns>Formatted text</returns>
        private static string CodeEvaluatorSimple(Match match)
        {
            if (!match.Success)
                return match.Value;

            var options = new HighlightOptions();

            options.Language = "c#";
            options.Code = match.Groups["inner"].Value;
            options.DisplayLineNumbers = false;
            options.Title = string.Empty;
            options.AlternateLineNumbers = false;

            string result = match.Value;
            result = Highlight(options, result);
            return result;

        }

        /// <summary>
        /// Strips HTML
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns>Formatted text</returns>
        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return regexHtml.Replace(html, string.Empty);
        }

        /// <summary>
        /// Returns the formatted text.
        /// </summary>
        /// <param name="options">Whatever options were set in the regex groups.</param>
        /// <param name="text">Send the e.body so it can get formatted.</param>
        /// <returns>The formatted string of the match.</returns>
        private static string Highlight(HighlightOptions options, string text)
        {
            switch (options.Language)
            {
                case "c#":
                    var csf = new CSharpFormat();
                    csf.LineNumbers = options.DisplayLineNumbers;
                    csf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(csf.FormatCode(text));

                case "vb":
                    var vbf = new VisualBasicFormat();
                    vbf.LineNumbers = options.DisplayLineNumbers;
                    vbf.Alternate = options.AlternateLineNumbers;
                    return vbf.FormatCode(text);

                case "js":
                    var jsf = new JavaScriptFormat();
                    jsf.LineNumbers = options.DisplayLineNumbers;
                    jsf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(jsf.FormatCode(text));

                case "html":
                    var htmlf = new HtmlFormat();
                    htmlf.LineNumbers = options.DisplayLineNumbers;
                    htmlf.Alternate = options.AlternateLineNumbers;
                    text = StripHtml(text).Trim();
                    string code = htmlf.FormatCode(HttpUtility.HtmlDecode(text)).Trim();
                    return code.Replace("\r\n", "<br />").Replace("\n", "<br />");

                case "xml":
                    var xmlf = new HtmlFormat();
                    xmlf.LineNumbers = options.DisplayLineNumbers;
                    xmlf.Alternate = options.AlternateLineNumbers;
                    text = text.Replace("<br />", "\r\n");
                    text = StripHtml(text).Trim();
                    string xml = xmlf.FormatCode(HttpUtility.HtmlDecode(text)).Trim();
                    return xml.Replace("\r\n", "<br />").Replace("\n", "<br />");

                case "tsql":
                    var tsqlf = new TsqlFormat();
                    tsqlf.LineNumbers = options.DisplayLineNumbers;
                    tsqlf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(tsqlf.FormatCode(text));

                case "msh":
                    var mshf = new MshFormat();
                    mshf.LineNumbers = options.DisplayLineNumbers;
                    mshf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(mshf.FormatCode(text));

            }

            return string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatTextSimple(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Contains("[/code]"))
            {
                text = regexCode2.Replace(text, new MatchEvaluator(CodeEvaluatorSimple));
                text = regexCode2.Replace(text, "$1");
            }
            return text;
        }

        #endregion
    }

    public partial class BBCodeHelper
    {
        #region Fields
        private static readonly Regex regexBold = new Regex(@"\[b\](.+?)\[/b\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexItalic = new Regex(@"\[i\](.+?)\[/i\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUnderLine = new Regex(@"\[u\](.+?)\[/u\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl1 = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl2 = new Regex(@"\[url\](.+?)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //private static readonly Regex regexQuote = new Regex(@"\[quote=(.+?)\](.+?)\[/quote\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexQuote = new Regex(@"\[quote=(.+?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexImg1 = new Regex(@"\[img\=([^\]]+)\]([^\]]+)\[/img\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexImg2 = new Regex(@"\[img\](.+?)\[/img\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexSize = new Regex(@"\[size=(.+?)\](.+?)\[/size\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexColor = new Regex(@"\[color=(.+?)\](.+?)\[/color\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexIframe = new Regex(@"\[iframe (.+?)\]\[/iframe\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexjwplayer = new Regex(@"\[jwplayer\](.+?)\[/jwplayer\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex regexAlltaghtml = new Regex(@"\[(.+?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="replaceBold">A value indicating whether to replace Bold</param>
        /// <param name="replaceItalic">A value indicating whether to replace Italic</param>
        /// <param name="replaceUnderline">A value indicating whether to replace Underline</param>
        /// <param name="replaceUrl">A value indicating whether to replace URL</param>
        /// <param name="replaceCode">A value indicating whether to replace Code</param>
        /// <param name="replaceQuote">A value indicating whether to replace Quote</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string text, bool replaceBold, bool replaceItalic,
            bool replaceUnderline, bool replaceUrl, bool replaceCode, bool replaceQuote)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = ConvertPlainTextToHtml(text);

            if (replaceBold)
            {
                // format the bold tags: [b][/b]
                // becomes: <strong></strong>
                text = regexBold.Replace(text, "<strong>$1</strong>");
            }

            if (replaceItalic)
            {
                // format the italic tags: [i][/i]
                // becomes: <em></em>
                text = regexItalic.Replace(text, "<em>$1</em>");
            }

            if (replaceUnderline)
            {
                // format the underline tags: [u][/u]
                // becomes: <u></u>
                text = regexUnderLine.Replace(text, "<u>$1</u>");
            }


            //hình ảnh

            // format the url tags: [url=http://www.lotusviet.vn]my site[/url]
            // becomes: <a href="http://www.lotusviet.vn">my site</a>
            text = regexImg1.Replace(text, "<img src=\"$1\"/>");

            // format the url tags: [url]http://www.lotusviet.vn[/url]
            // becomes: <a href="http://www.lotusviet.vn">http://www.lotusviet.vn</a>
            text = regexImg2.Replace(text, "<img src=\"$1\"/>");

            //kết thúc hình ảnh

            //xem phim
            // format the italic tags: [jwplayer][/jwplayer]
            // becomes: <em></em>
            //while (regexjwplayer.IsMatch(text))
            //{
            //    text = regexjwplayer.Replace(text, string.Format("<div id=\"{0}\"></div><script type=\"text/javascript\">jwplayer('{0}'){1}", SenViet.Uti.Generate.GetStringID(), ".setup({$1});</script>"));
            //}

            //text= regexjwplayer.Replace(text, new MatchEvaluator(GenJWPlayer));
            string _text = regexjwplayer.Replace(text, new MatchEvaluator(GenJWPlayer));
            text = _text;


            //string result = regexjwplayer.Replace(text, new MatchEvaluator(RegExSample.CapText));
            //}  hành động, huyền bí, rùng rợn
            //        string    <div id="myElement">
            //</div>
            //<script type="text/javascript">
            //    jwplayer("myElement").setup({
            //        file: "http://vdc1.cdn.clip.vn:8080/a6aa2353/media33/level4/0/0/47/1568608.mp4?start=16.22&fb=2~1~0",
            //        width: 705,
            //        height: 400
            //    });
            //</script>


            //ket thuc xem phim


            if (replaceUrl)
            {
                // format the url tags: [url=http://www.lotusviet.vn]my site[/url]
                // becomes: <a href="http://www.lotusviet.vn">my site</a>
                text = regexUrl1.Replace(text, "<a target=\"_blank\" href=\"$1\" rel=\"nofollow\">$2</a>");

                // format the url tags: [url]http://www.lotusviet.vn[/url]
                // becomes: <a href="http://www.lotusviet.vn">http://www.lotusviet.vn</a>
                text = regexUrl2.Replace(text, "<a target=\"_blank\" href=\"$1\" rel=\"nofollow\">$1</a>");
            }



            if (replaceQuote)
            {
                //while (regexQuote.IsMatch(text))
                //    text = regexQuote.Replace(text, "<b>$1 đã viết:</b><div class=\"quote\">$2</div>");

                while (regexQuote.IsMatch(text))
                    text = regexQuote.Replace(text, "<b>$1 đã viết:</b><div class=\"quote\">");

                text = text.Replace("[/quote]", "</div>");

            }

            while (regexSize.IsMatch(text))
                text = regexSize.Replace(text, "<span style=\"font-size:$1px;\">$2</span>");

            while (regexColor.IsMatch(text))
                text = regexColor.Replace(text, "<span style=\"color:$1;\">$2</span>");

            while (regexIframe.IsMatch(text))
                text = regexIframe.Replace(text, "<iframe $1></iframe>");







            //all tag html
            while (regexAlltaghtml.IsMatch(text))
                text = regexAlltaghtml.Replace(text, "<$1>");

            if (replaceCode)
            {
                text = CodeFormatHelper.FormatTextSimple(text);
            }

            text = text.Replace("svu0001", "[");
            text = text.Replace("svu0002", "]");

            text = text.Replace("svu0003", "<");
            text = text.Replace("svu0004", ">");

            return text;
        }

        public static string FormatText(string text)
        {
            //return FormatText(text, false, false, false, false, false, false);
            return FormatText(text, true, true, true, true, true, true);
        }

        public static string GenJWPlayer(Match m)
        {
            // Get the matched string. 
            //string x = m.ToString();
            // If the first char is lower case... 
            //if (char.IsLower(x[0]))
            //{
            //    // Capitalize it. 
            //    return char.ToUpper(x[0]) + x.Substring(1, x.Length - 1);
            //}
            //return x;
            string x = string.Format("<div id=\"{0}{1}\"></div><script type=\"text/javascript\">jwplayer(\"{0}{1}\"){2}", "jwplayer", m.Index, ".setup({$1});</script>");
            string kq = regexjwplayer.Replace(m.ToString(), x);
            return kq;//string.Format("<div id=\"{0}\"></div><script type=\"text/javascript\">jwplayer(\"{0}\").setup({1});</script>", SenViet.Uti.Generate.GetStringID(), m.ToString());
        }

        /// <summary>
        /// Removes all quotes from string
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns>string</returns>
        public static string RemoveQuotes(string str)
        {
            str = Regex.Replace(str, @"\[quote=(.+?)\]", String.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\[/quote\]", String.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return str;
        }


        /// <summary>
        /// Converts plain text to HTML
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }

        #endregion
    }

    public static class HtmlHelpers
    {

        #region Fields
        private readonly static Regex paragraphStartRegex = new Regex("<p>", RegexOptions.IgnoreCase);
        private readonly static Regex paragraphEndRegex = new Regex("</p>", RegexOptions.IgnoreCase);
        //private static Regex ampRegex = new Regex("&(?!(?:#[0-9]{2,4};|[a-z0-9]+;))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Utilities

        private static string EnsureOnlyAllowedHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address,cite";

            var m = Regex.Matches(text, "<.*?>", RegexOptions.IgnoreCase);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    text = text.Remove(m[i].Index, m[i].Length);
                }
            }

            return text;
        }

        private static bool IsValidTag(string tag, string tags)
        {
            string[] allowedTags = tags.Split(',');
            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            var endchars = new char[] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);
            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="stripTags">A value indicating whether to strip tags</param>
        /// <param name="convertPlainTextToHtml">A value indicating whether HTML is allowed</param>
        /// <param name="allowHtml">A value indicating whether HTML is allowed</param>
        /// <param name="allowBBCode">A value indicating whether BBCode is allowed</param>
        /// <param name="resolveLinks">A value indicating whether to resolve links</param>
        /// <param name="addNoFollowTag">A value indicating whether to add "noFollow" tag</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string text, bool stripTags,
            bool convertPlainTextToHtml, bool allowHtml,
            bool allowBBCode, bool resolveLinks, bool addNoFollowTag)
        {

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            try
            {
                if (stripTags)
                {
                    text = HtmlHelpers.StripTags(text);
                }

                if (allowHtml)
                {
                    text = HtmlHelpers.EnsureOnlyAllowedHtml(text);
                }
                else
                {
                    text = HttpUtility.HtmlEncode(text);
                }

                if (convertPlainTextToHtml)
                {
                    text = HtmlHelpers.ConvertPlainTextToHtml(text);
                }

                if (allowBBCode)
                {
                    text = BBCodeHelper.FormatText(text, true, true, true, true, true, true);
                }

                if (resolveLinks)
                {
                    text = ResolveLinksHelper.FormatText(text);
                }

                if (addNoFollowTag)
                {
                    //add noFollow tag. not implemented
                }
            }
            catch (Exception exc)
            {
                text = string.Format("Text cannot be formatted. Error: {0}", exc.Message);
            }
            return text;
        }

        /// <summary>
        /// Strips tags
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string StripTags(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, @"(>)(\r|\n)*(<)", "><");
            text = Regex.Replace(text, "(<[^>]*>)([^<]*)", "$2");
            text = Regex.Replace(text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");

            return text;
        }

        /// <summary>
        /// replace anchor text (remove a tag from the following url <a href="http://example.com">Name</a> and output only the string "Name")
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Text</returns>
        public static string ReplaceAnchorTags(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1", RegexOptions.IgnoreCase);
            return text;
        }

        /// <summary>
        /// Converts plain text to HTML
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }

        /// <summary>
        /// Converts HTML to plain text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="decode">A value indicating whether to decode text</param>
        /// <param name="replaceAnchorTags">A value indicating whether to replace anchor text (remove a tag from the following url <a href="http://example.com">Name</a> and output only the string "Name")</param>
        /// <returns>Formatted text</returns>
        public static string ConvertHtmlToPlainText(string text,
            bool decode = false, bool replaceAnchorTags = false)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            if (decode)
                text = HttpUtility.HtmlDecode(text);

            text = text.Replace("<br>", "\n");
            text = text.Replace("<br >", "\n");
            text = text.Replace("<br />", "\n");
            text = text.Replace("&nbsp;&nbsp;", "\t");
            text = text.Replace("&nbsp;&nbsp;", "  ");

            if (replaceAnchorTags)
                text = ReplaceAnchorTags(text);

            return text;
        }

        /// <summary>
        /// Converts text to paragraph
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string ConvertPlainTextToParagraph(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = paragraphStartRegex.Replace(text, string.Empty);
            text = paragraphEndRegex.Replace(text, "\n");
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = text + "\n\n";
            text = text.Replace("\n\n", "\n");
            var strArray = text.Split(new char[] { '\n' });
            var builder = new StringBuilder();
            foreach (string str in strArray)
            {
                if ((str != null) && (str.Trim().Length > 0))
                {
                    builder.AppendFormat("<p>{0}</p>\n", str);
                }
            }
            return builder.ToString();
        }
        #endregion

        public static MvcHtmlString svDatePickerDropDowns(this HtmlHelper html,
        string dayName, string monthName, string yearName,
        int? beginYear = null, int? endYear = null,
        int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null, bool localizeLabels = true)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            string dayLocale, monthLocale, yearLocale;
            if (localizeLabels)
            {
                //var locService = EngineContext.Current.Resolve<ILocalizationService>();
                dayLocale = "Ngày";//locService.GetResource("Common.Day");
                monthLocale = "Tháng";//locService.GetResource("Common.Month");
                yearLocale = "Năm";//locService.GetResource("Common.Year");
            }
            else
            {
                dayLocale = "Ngày";
                monthLocale = "Tháng";
                yearLocale = "Năm";
            }

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", dayLocale);
            for (int i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", monthLocale);
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                                    i,
                                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", yearLocale);

            if (beginYear == null)
                beginYear = DateTime.UtcNow.Year - 100;
            if (endYear == null)
                endYear = DateTime.UtcNow.Year;

            for (int i = beginYear.Value; i <= endYear.Value; i++)
                years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
        }

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }

        public static MvcHtmlString BBCodeEditor<TModel>(this HtmlHelper<TModel> html, string name)
        {
            var sb = new System.Text.StringBuilder();

            //var storeLocation = EngineContext.Current.Resolve<IWebHelper>().GetStoreLocation();

            var storeLocation = "/";
            string bbEditorWebRoot = String.Format("{0}Scripts/", storeLocation);

            sb.AppendFormat("<script src=\"{0}Scripts/editors/BBEditor/ed.js\" type=\"text/javascript\"></script>", storeLocation);
            sb.Append(Environment.NewLine);
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    var webRoot = '{0}';", bbEditorWebRoot);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    edToolbar('{0}');", name);
            sb.Append(Environment.NewLine);
            sb.Append("</script>");
            sb.Append(Environment.NewLine);

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString svImage(this HtmlHelper helper, string id, string url)
        {

            return svImage(helper, id, url, null);
        }

        public static MvcHtmlString svImage(this HtmlHelper helper, string id, string url, string alternateText)
        {

            return svImage(helper, id, url, alternateText, null);
        }

        public static MvcHtmlString svImage(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        {
            // Instantiate a UrlHelper   
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            // Create tag builder  
            var builder = new TagBuilder("img");

            // Create valid id  
            builder.GenerateId(id);

            // Add attributes  
            if (!string.IsNullOrEmpty(url))
            {

                builder.MergeAttribute("src", urlHelper.Content(url));
            }
            else
            {
                builder.MergeAttribute("src", "");
            }
            builder.MergeAttribute("alt", alternateText);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            // Render tag  
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString svLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return svLabelFor(html, expression, null);
        }

        public static MvcHtmlString svLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            var builder = new TagBuilder("label");

            string fieldName = Services.ModelMeta.GetFieldName(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);


            var AllowDBNull = arrMeta.AllowDBNull == false ? " (*)" : "";
            var DisplayName = arrMeta.Des + AllowDBNull;


            builder.MergeAttribute("for", fieldName);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            builder.AddCssClass("control-label");
            builder.InnerHtml = DisplayName;
            return MvcHtmlString.Create(builder.ToString());

            //// Create tag builder  
            //var builderdiv = new TagBuilder("div");
            //builderdiv.AddCssClass("control-label");
            //builderdiv.InnerHtml = builder.ToString();
            //return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString svEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return svEditorFor(html, expression, null);
        }

        public static MvcHtmlString svEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="text-box single-line">
            //<div class="editor-field">
            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="input-validation-error text-box single-line">
            //<span class="field-validation-error">The value ',' is invalid.</span>
            //</div>

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);
            string datatype = Services.ExConvert.Sqltype2Systemtype(arrMeta.DATA_TYPE);

            var builder = new TagBuilder("input");

            //((List<SenViet.Models.SysTableDetail>)html.ViewData[metadata.ContainerType.Name]).Where(m => m.ColumnName == metadata.PropertyName).ToArray();

            var AllowDBNull = arrMeta.AllowDBNull == false ? " (*):" : ":";
            var IsValid = arrMeta.IsValid;

            var DisplayName = arrMeta.Des + AllowDBNull;
            var ReadOnly = arrMeta.ReadOnly;

            var fullfieldname = ExpressionHelper.GetExpressionText(expression); // trả về tên đầy đủ field bao gồm Prefix ví dụ AppTourView.TourGroupID

            //Set lookup

            // Kết thúc set lookup

            builder.MergeAttribute("fieldName", metadata.PropertyName, true);
            builder.AddCssClass("text-box single-line");
            builder.MergeAttribute("type", "text");
            builder.MergeAttribute("name", fullfieldname, true);
            builder.AddCssClass("form-control");

            if (ReadOnly == true)
            {
                builder.MergeAttribute("readonly", "readonly", true);
                //builder.MergeAttribute("disabled", "disabled", true);
            }

            if (IsValid == true)
            {
                builder.MergeAttribute("isautocomplete", "isautocomplete", true);
            }

            if (datatype == "numeric")
            {
                builder.MergeAttribute("style", "text-align:right;");
                builder.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(arrMeta.FormatValue, arrMeta.CultureInfo).ToString());
            }

            builder.GenerateId(fullfieldname);

            ModelState modelState = null;
            html.ViewData.ModelState.TryGetValue(fullfieldname, out modelState);

            //gán giá trị theo định dạng của kiểu dữ liệu
            //builder.AddCssClass(SenViet.Html.Uti.GetClassNameByType(metadata.ModelType.ToString(), arrMeta.FormatValue));
            //string attemptedValue = Services.ExConvert.Data2String(metadata.Model, metadata.ModelType.ToString(), arrMeta.FormatValue, arrMeta.CultureInfo);

            builder.AddCssClass(Services.Uti.GetClassNameByType(datatype, arrMeta.FormatValue));
            string attemptedValue = Services.ExConvert.Data2String(metadata.Model, datatype, arrMeta.FormatValue, arrMeta.CultureInfo);

            builder.MergeAttribute("value", attemptedValue, true);


            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }


            // If there are any errors for a named field, we add the css attribute.
            string errormessage = "";
            if (modelState != null)
            {
                if (modelState.Errors.Count > 0)
                {
                    builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    errormessage = string.Format("<span class='field-validation-error'>{0}</span>", modelState.Errors[0].ErrorMessage);
                }
            }

            // Create tag builder  
            var builderdiv = new TagBuilder("div");
            builderdiv.AddCssClass("editor-field");
            builderdiv.InnerHtml = builder.ToString();
            builderdiv.InnerHtml += errormessage;

            switch (datatype)
            {
                case "boolean":

                    builder.MergeAttribute("type", "checkbox", true);
                    builder.MergeAttribute("class", "check-box", true);
                    builder.MergeAttribute("value", "true", true);

                    if (attemptedValue.ToString() != "")
                    {
                        if (Boolean.Parse(attemptedValue))
                        {
                            builder.MergeAttribute("checked", "checked");
                        }
                    }
                    builderdiv.InnerHtml = builder.ToString();
                    builderdiv.InnerHtml += errormessage;
                    builderdiv.InnerHtml += string.Format("<input type='hidden' value='false' name='{0}'>", fullfieldname);
                    break;
                //return System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression);
                default:
                    break;
            }



            return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString svDisplayFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);

            //((List<SenViet.Models.SysTableDetail>)html.ViewData[metadata.ContainerType.Name]).Where(m => m.ColumnName == metadata.PropertyName).ToDictionary(m=>m.ColumnName);

            switch (metadata.ModelType.ToString())
            {
                case "System.Nullable`1[System.Boolean]":
                    return System.Web.Mvc.Html.DisplayExtensions.DisplayFor(html, expression);
                case "System.Boolean":
                    return System.Web.Mvc.Html.DisplayExtensions.DisplayFor(html, expression);
                default:
                    return MvcHtmlString.Create(Services.ExConvert.Data2String(metadata.Model, metadata.ModelType.ToString(), arrMeta.FormatValue, arrMeta.CultureInfo));
            }

        }

        #region tạo control for bootstrap

        public static MvcHtmlString bsValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            TagBuilder div = new TagBuilder("div");
            if (!htmlHelper.ViewData.ModelState.IsValid)
            {
                div.AddCssClass("callout callout-danger");
            }
            MvcHtmlString _validation = System.Web.Mvc.Html.ValidationExtensions.ValidationSummary(htmlHelper, excludePropertyErrors);
            div.InnerHtml = _validation.ToString();
            return MvcHtmlString.Create(div.ToString());
        }

        public static MvcHtmlString bsLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsLabelFor(html, expression, null);
        }

        public static MvcHtmlString bsLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            var builder = new TagBuilder("label");

            string fieldName = Services.ModelMeta.GetFieldName(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);

            var AllowDBNull = arrMeta.AllowDBNull == false ? " (*)" : "";
            var DisplayName = arrMeta.Des + AllowDBNull;


            builder.MergeAttribute("for", fieldName);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            builder.AddCssClass("control-label");
            builder.InnerHtml = DisplayName;
            return MvcHtmlString.Create(builder.ToString());
        }


        public static MvcHtmlString bsLabel(this HtmlHelper html, string fieldName, string DisplayName)
        {
            return bsLabel(html, fieldName, DisplayName, null);
        }

        public static MvcHtmlString bsLabel(this HtmlHelper html, string fieldName, string DisplayName, object htmlAttributes)
        {

            var builder = new TagBuilder("label");

            builder.MergeAttribute("for", fieldName);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            builder.AddCssClass("control-label");
            builder.InnerHtml = DisplayName;
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString bsAutoComplete(this HtmlHelper html,string Name, string fieldName, string attemptedValue)
        {
            return bsAutoComplete(html,Name,fieldName, attemptedValue, null);
        }
        public static MvcHtmlString bsAutoComplete(this HtmlHelper html,string Name, string fieldName, string attemptedValue, object htmlAttributes)
        {

            var builder = new TagBuilder("select");

            builder.MergeAttribute("isautocomplete", "isautocomplete", true);

            var optiontag = new TagBuilder("option");

            optiontag.MergeAttribute("value", attemptedValue, true);
            builder.InnerHtml = optiontag.ToString();

            builder.MergeAttribute("name", Name);
            builder.MergeAttribute("fieldname", fieldName);
            builder.MergeAttribute("id", Name);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            builder.AddCssClass("form-control text-box single-line");

            return MvcHtmlString.Create(builder.ToString());
        }



        public static MvcHtmlString bsDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectlist)
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("form-group");

            MvcHtmlString label = bsLabelFor(html, expression);
            MvcHtmlString dropdown = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, selectlist, new { @class = "form-control" });
            div.InnerHtml = label.ToString();
            div.InnerHtml += dropdown.ToString();
            return MvcHtmlString.Create(div.ToString());
        }


        public static MvcHtmlString bsDropDownList2For<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectlist)
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("form-group");

            //MvcHtmlString label = bsLabelFor(html, expression);
            MvcHtmlString dropdown = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, selectlist, new { @class = "form-control" });
            //div.InnerHtml = label.ToString();
            div.InnerHtml += dropdown.ToString();
            return MvcHtmlString.Create(div.ToString());
        }

        public static MvcHtmlString bsDropDownList2For<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectlist, string optionLabel)
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("form-group");

            //MvcHtmlString label = bsLabelFor(html, expression);
            MvcHtmlString dropdown = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, selectlist, optionLabel, new { @class = "form-control" });
            //div.InnerHtml = label.ToString();
            div.InnerHtml += dropdown.ToString();
            return MvcHtmlString.Create(div.ToString());
        }


        public static MvcHtmlString bsEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsEditorFor(html, expression, null);
        }

        public static MvcHtmlString bsEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="text-box single-line">
            //<div class="editor-field">
            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="input-validation-error text-box single-line">
            //<span class="field-validation-error">The value ',' is invalid.</span>
            //</div>

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);
            string datatype = Services.ExConvert.Sqltype2Systemtype(arrMeta.DATA_TYPE);

            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            #endregion

            var builder = new TagBuilder("input");


            //((List<SenViet.Models.SysTableDetail>)html.ViewData[metadata.ContainerType.Name]).Where(m => m.ColumnName == metadata.PropertyName).ToArray();

            var AllowDBNull = arrMeta.AllowDBNull == false ? " (*):" : ":";
            var IsValid = arrMeta.IsValid;

            var DisplayName = arrMeta.Des + AllowDBNull;
            var ReadOnly = arrMeta.ReadOnly;

            var fullfieldname = ExpressionHelper.GetExpressionText(expression); // trả về tên đầy đủ field bao gồm Prefix ví dụ AppTourView.TourGroupID

            //Set lookup
            if (IsValid == true)
            {
                builder = new TagBuilder("select");
            }

            // Kết thúc set lookup

            builder.MergeAttribute("fieldName", metadata.PropertyName, true);
            builder.AddCssClass("text-box single-line");
            builder.MergeAttribute("type", "text");
            builder.MergeAttribute("name", fullfieldname, true);
            builder.AddCssClass("form-control");

            if (ReadOnly == true)
            {
                builder.MergeAttribute("readonly", "readonly", true);
                //builder.MergeAttribute("disabled", "disabled", true);
            }

            if (IsValid == true)
            {
                builder.MergeAttribute("isautocomplete", "isautocomplete", true);
            }

            if (datatype == "numeric")
            {
                builder.MergeAttribute("style", "text-align:right;");
                builder.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(arrMeta.FormatValue, arrMeta.CultureInfo).ToString());
            }

            builder.GenerateId(fullfieldname);

            ModelState modelState = null;
            html.ViewData.ModelState.TryGetValue(fullfieldname, out modelState);

            //gán giá trị theo định dạng của kiểu dữ liệu
            //builder.AddCssClass(SenViet.Html.Uti.GetClassNameByType(metadata.ModelType.ToString(), arrMeta.FormatValue));
            //string attemptedValue = Services.ExConvert.Data2String(metadata.Model, metadata.ModelType.ToString(), arrMeta.FormatValue, arrMeta.CultureInfo);

            builder.AddCssClass(Services.Uti.GetClassNameByType(datatype, arrMeta.FormatValue));
            string attemptedValue = Services.ExConvert.Data2String(metadata.Model, datatype, arrMeta.FormatValue, arrMeta.CultureInfo);

            if (IsValid == true)
            {
                var optiontag = new TagBuilder("option");
                optiontag.MergeAttribute("value", attemptedValue, true);
                builder.InnerHtml = optiontag.ToString();
            }
            else
            {
                builder.MergeAttribute("value", attemptedValue, true);
            }


            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }


            // If there are any errors for a named field, we add the css attribute.
            string errormessage = "";
            if (modelState != null)
            {
                if (modelState.Errors.Count > 0)
                {
                    builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    errormessage = string.Format("<span class='label label-warning'>{0}</span>", modelState.Errors[0].ErrorMessage);
                }
            }



            // Create tag builder  
            var builderdiv = new TagBuilder("div");
            builderdiv.AddCssClass("form-group");
            builderdiv.InnerHtml = strlabel.ToString();
            #region nếu có autocomplete thì cho phép nút thêm
            //if (IsValid == true && arrMeta.TableNameValid.Contains(".exe"))
            //{
            //    TagBuilder newaddcontainer = new TagBuilder("div");
            //    newaddcontainer.AddCssClass("input-group");
            //    newaddcontainer.InnerHtml = builder.ToString();

            //    TagBuilder newaddbuttongroup = new TagBuilder("div");
            //    newaddbuttongroup.AddCssClass("input-group-btn");
            //    TagBuilder newaddbutton = new TagBuilder("btton");
            //    newaddbutton.AddCssClass("btn btn-default");
            //    newaddbutton.InnerHtml = "<i class=\"fa fa-plus\"></i>";
            //    newaddbutton.MergeAttribute("onclick", string.Format("Addnew('{0}')", arrMeta.ColumnName), true);

            //    newaddbuttongroup.InnerHtml += newaddbutton.ToString();
            //    newaddcontainer.InnerHtml += newaddbuttongroup.ToString();

            //    builderdiv.InnerHtml += newaddcontainer.ToString();

            //}
            //else
            //{
            //    builderdiv.InnerHtml += builder.ToString();
            //}
            builderdiv.InnerHtml += builder.ToString();
            #endregion

            builderdiv.InnerHtml += errormessage;
            if (!string.IsNullOrEmpty(errormessage))
            {
                builderdiv.AddCssClass("form-group has-error");
            }
            switch (datatype)
            {
                case "boolean":

                    //var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);

                    builder.MergeAttribute("type", "checkbox", true);
                    builder.MergeAttribute("class", "check-box", true);
                    builder.MergeAttribute("value", "true", true);

                    if (attemptedValue.ToString() != "")
                    {
                        if (Boolean.Parse(attemptedValue))
                        {
                            builder.MergeAttribute("checked", "checked");
                        }
                    }

                    builderdiv.AddCssClass("checkbox");
                    builderdiv.InnerHtml = builder.ToString();
                    builderdiv.InnerHtml += arrMeta.Des;
                    builderdiv.InnerHtml += errormessage;
                    builderdiv.InnerHtml += string.Format("<input type='hidden' value='false' name='{0}'>", fullfieldname);
                    break;
                //return System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression);
                default:
                    break;
            }



            return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString bsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsTextAreaFor(html, expression, null);
        }
        public static MvcHtmlString bsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="text-box single-line">
            //<div class="editor-field">
            //<input type="text" value="" name="AppTourView.TourCode" id="AppTourView_TourCode" class="input-validation-error text-box single-line">
            //<span class="field-validation-error">The value ',' is invalid.</span>
            //</div>

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);
            string datatype = Services.ExConvert.Sqltype2Systemtype(arrMeta.DATA_TYPE);

            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            #endregion

            var builder = new TagBuilder("textarea");

            //((List<SenViet.Models.SysTableDetail>)html.ViewData[metadata.ContainerType.Name]).Where(m => m.ColumnName == metadata.PropertyName).ToArray();

            var AllowDBNull = arrMeta.AllowDBNull == false ? " (*):" : ":";
            var IsValid = arrMeta.IsValid;

            var DisplayName = arrMeta.Des + AllowDBNull;
            var ReadOnly = arrMeta.ReadOnly;

            var fullfieldname = ExpressionHelper.GetExpressionText(expression); // trả về tên đầy đủ field bao gồm Prefix ví dụ AppTourView.TourGroupID

            //Set lookup

            // Kết thúc set lookup

            builder.MergeAttribute("fieldName", metadata.PropertyName, true);
            //builder.AddCssClass("text-box single-line");
            //builder.MergeAttribute("type", "text");
            builder.MergeAttribute("name", fullfieldname, true);
            builder.AddCssClass("form-control");

            if (ReadOnly == true)
            {
                builder.MergeAttribute("readonly", "readonly", true);
                //builder.MergeAttribute("disabled", "disabled", true);
            }

            if (IsValid == true)
            {
                builder.MergeAttribute("isautocomplete", "isautocomplete", true);
            }

            if (datatype == "numeric")
            {
                builder.MergeAttribute("style", "text-align:right;");
                builder.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(arrMeta.FormatValue, arrMeta.CultureInfo).ToString());
            }

            builder.GenerateId(fullfieldname);

            ModelState modelState = null;
            html.ViewData.ModelState.TryGetValue(fullfieldname, out modelState);

            //gán giá trị theo định dạng của kiểu dữ liệu
            //builder.AddCssClass(SenViet.Html.Uti.GetClassNameByType(metadata.ModelType.ToString(), arrMeta.FormatValue));
            //string attemptedValue = Services.ExConvert.Data2String(metadata.Model, metadata.ModelType.ToString(), arrMeta.FormatValue, arrMeta.CultureInfo);

            builder.AddCssClass(Services.Uti.GetClassNameByType(datatype, arrMeta.FormatValue));
            string attemptedValue = Services.ExConvert.Data2String(metadata.Model, datatype, arrMeta.FormatValue, arrMeta.CultureInfo);

            //builder.MergeAttribute("value", attemptedValue, true); // do textarea không có value
            builder.SetInnerText(attemptedValue);


            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }


            // If there are any errors for a named field, we add the css attribute.
            string errormessage = "";
            if (modelState != null)
            {
                if (modelState.Errors.Count > 0)
                {
                    builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    errormessage = string.Format("<span class='label label-warning'>{0}</span>", modelState.Errors[0].ErrorMessage);
                }
            }

            // Create tag builder  
            var builderdiv = new TagBuilder("div");
            builderdiv.AddCssClass("form-group");
            builderdiv.InnerHtml = strlabel.ToString();
            builderdiv.InnerHtml += builder.ToString();
            builderdiv.InnerHtml += errormessage;
            if (!string.IsNullOrEmpty(errormessage))
            {
                builderdiv.AddCssClass("form-group has-error");
            }
            switch (datatype)
            {
                case "boolean":

                    //var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);

                    builder.MergeAttribute("type", "checkbox", true);
                    builder.MergeAttribute("class", "check-box", true);
                    builder.MergeAttribute("value", "true", true);

                    if (attemptedValue.ToString() != "")
                    {
                        if (Boolean.Parse(attemptedValue))
                        {
                            builder.MergeAttribute("checked", "checked");
                        }
                    }

                    builderdiv.AddCssClass("checkbox");
                    builderdiv.InnerHtml = builder.ToString();
                    builderdiv.InnerHtml += arrMeta.Des;
                    builderdiv.InnerHtml += errormessage;
                    builderdiv.InnerHtml += string.Format("<input type='hidden' value='false' name='{0}'>", fullfieldname);
                    break;
                //return System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expression);
                default:
                    break;
            }



            return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString bsDisplayFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var arrMeta = Services.GlobalMeta.GetMetaObject(metadata.ContainerType.Name).GetMetaByColumnName(metadata.PropertyName);

            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            MvcHtmlString strdisplay = MvcHtmlString.Empty;
            #endregion

            var builderdiv = new TagBuilder("div");
            var builderp = new TagBuilder("p");
            //builderdiv.AddCssClass("form-group");
            builderdiv.InnerHtml = strlabel.ToString();


            switch (metadata.ModelType.ToString())
            {
                case "System.Nullable`1[System.Boolean]":
                    strdisplay = System.Web.Mvc.Html.DisplayExtensions.DisplayFor(html, expression);
                    break;
                case "System.Boolean":
                    strdisplay = System.Web.Mvc.Html.DisplayExtensions.DisplayFor(html, expression);
                    break;
                default:
                    strdisplay = MvcHtmlString.Create(Services.ExConvert.Data2String( metadata.Model, metadata.ModelType.ToString(), arrMeta.FormatValue, arrMeta.CultureInfo));
                    break;
            }

            builderp.InnerHtml += strdisplay.ToString();
            builderdiv.InnerHtml += builderp.ToString();
            return MvcHtmlString.Create(builderdiv.ToString());
        }
        #endregion



        public static MvcHtmlString svHeaderFor(this HtmlHelper htmlHelper, string metaname)
        {
            var metacolumns = Services.GlobalMeta.GetMetaObject(metaname).GetMetaTable();
            string tagheader = "";
            foreach (var item in metacolumns)
            {
                tagheader += svHeaderColFor(htmlHelper, item, metaname);
            }

            return MvcHtmlString.Create(tagheader.ToString());
        }

        public static MvcHtmlString svHeaderColFor(this HtmlHelper htmlHelper, Models.SysTableDetailView item, string metaname, string des = "")
        {
            KeyValuePair<string, Models.SysTableDetailView> _item = new KeyValuePair<string, Models.SysTableDetailView>("A", item);

            return svHeaderColFor(htmlHelper, _item, metaname, des);
        }

        public static MvcHtmlString svHeaderColFor(this HtmlHelper htmlHelper, KeyValuePair<string, Models.SysTableDetailView> item, string metaname, string des = "")
        {
            TagBuilder tagheadercol = new TagBuilder("td");
            tagheadercol.AddCssClass("sv-gv-header");
            tagheadercol.MergeAttribute("id", string.Format("{0}.sort.{1}", metaname, item.Value.ColumnName));

            //thiết lập độ rộng của cột
            tagheadercol.MergeAttribute("style", string.Format("width:{0}px;", item.Value.GridViewWidth + 30));
            //kết thúc thiết lập độ rộng của cột

            TagBuilder table = new TagBuilder("table");
            table.MergeAttribute("cellspacing", "0");
            table.MergeAttribute("cellpadding", "0");
            table.MergeAttribute("style", "width:100%;border-collapse:collapse;");
            TagBuilder table_tr = new TagBuilder("tr");
            TagBuilder table_td = new TagBuilder("td");
            table_td.MergeAttribute("style", "width:100%;");

            table_td.SetInnerText(item.Value.Des);
            if (!string.IsNullOrWhiteSpace(des))
            {
                table_td.SetInnerText(des);
            }

            table_tr.InnerHtml = table_td.ToString();



            if (!string.IsNullOrWhiteSpace(item.Value.OrderExpression))
            {
                tagheadercol.MergeAttribute("OrderExpression", (item.Value.OrderExpression.Contains("asc") ? "asc" : "desc"));
                TagBuilder table_tdorder = new TagBuilder("td");
                table_tdorder.MergeAttribute("style", "padding-left: 5px;");
                table_tdorder.SetInnerText(item.Value.OrderExpression);

                TagBuilder imgsort = new TagBuilder("div");
                imgsort.AddCssClass(string.Format("sv-gv-header-{0}", (item.Value.OrderExpression.Contains("asc") ? "asc" : "desc")));

                table_tdorder.InnerHtml = imgsort.ToString();

                table_tr.InnerHtml += table_tdorder.ToString();
            }


            table.InnerHtml = table_tr.ToString();

            tagheadercol.InnerHtml = table.ToString();
            if (item.Value.GridViewShow != true)
            {
                tagheadercol.MergeAttribute("style", "display: none;", true);
            }

            return MvcHtmlString.Create(tagheadercol.ToString());
        }

        public static MvcHtmlString svFilterColFor(this HtmlHelper htmlHelper, KeyValuePair<string, Models.SysTableDetailView> item, string metaname)
        {

            string datatype = Services.ExConvert.Sqltype2Systemtype(item.Value.DATA_TYPE);

            TagBuilder filtercol = new TagBuilder("td");
            filtercol.AddCssClass("sv-gv-filter");
            filtercol.MergeAttribute("datatype", datatype);

            if ("numeric.datetime".Contains(datatype))
            {

                char[] delimiterChars = { ';' };
                string[] arrexpression = item.Value.FilterExpression == null ? null : item.Value.FilterExpression.Split(delimiterChars);

                for (int i = 0; i < 3; i++)
                {

                    TagBuilder div = new TagBuilder("div");

                    div.AddCssClass("sv-gv-filter-box");

                    TagBuilder span = new TagBuilder("span");
                    span.SetInnerText(i == 0 ? "=" : i == 1 ? ">" : "<");
                    var col = new TagBuilder("input");
                    string name = "";
                    switch (i)
                    {
                        case 0:
                            name = string.Format("{0}.filter.{1}.eq", metaname, item.Value.ColumnName);
                            break;
                        case 1:
                            name = string.Format("{0}.filter.{1}.gt", metaname, item.Value.ColumnName);
                            break;
                        case 2:
                            name = string.Format("{0}.filter.{1}.lt", metaname, item.Value.ColumnName);
                            break;
                        default:
                            break;
                    }

                    //
                    if (datatype == "numeric")
                    {
                        col.MergeAttribute("style", "text-align:right;");
                        col.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(item.Value.FormatValue, item.Value.CultureInfo).ToString());
                    }
                    col.AddCssClass(Services.Uti.GetClassNameByType(datatype, item.Value.FormatValue));
                    string attemptedValue = Services.ExConvert.Data2String(arrexpression == null ? "" : arrexpression[i] == null ? "" : arrexpression[i], datatype, item.Value.FormatValue, item.Value.CultureInfo);
                    //
                    col.MergeAttribute("name", name);
                    //col.MergeAttribute("type", "text");
                    col.MergeAttribute("value", attemptedValue);
                    col.MergeAttribute("fieldname", name, false);

                    div.InnerHtml = span.ToString();
                    div.InnerHtml += col.ToString();

                    filtercol.InnerHtml += div.ToString();
                }

                if (item.Value.GridViewShow != true)
                {
                    filtercol.MergeAttribute("style", "display: none;", false);
                }

            }
            else
            {
                var col = new TagBuilder("input");
                string name = string.Format("{0}.filter.{1}", metaname, item.Value.ColumnName);
                col.MergeAttribute("name", name);

                col.MergeAttribute("type", "text");
                col.MergeAttribute("value", item.Value.FilterExpression);

                col.MergeAttribute("fieldname", name, false);
                if (item.Value.GridViewShow != true)
                {
                    filtercol.MergeAttribute("style", "display: none;", false);
                }
                TagBuilder div = new TagBuilder("div");
                div.InnerHtml = col.ToString();
                filtercol.InnerHtml += div.ToString();
            }
            return MvcHtmlString.Create(filtercol.ToString());

        }

        public static MvcHtmlString svFilterFor(this HtmlHelper htmlHelper, string metaname)
        {
            var metacolumns = Services.GlobalMeta.GetMetaObject(metaname).GetMetaTable();
            string filtertag = "";

            foreach (var item in metacolumns)
            {
                string datatype = Services.ExConvert.Sqltype2Systemtype(item.Value.DATA_TYPE);


                TagBuilder filtercol = new TagBuilder("td");
                filtercol.AddCssClass("sv-gv-filter");
                filtercol.MergeAttribute("datatype", datatype);

                if ("numeric.datetime".Contains(datatype))
                {

                    char[] delimiterChars = { ';' };
                    string[] arrexpression = item.Value.FilterExpression == null ? null : item.Value.FilterExpression.Split(delimiterChars);

                    for (int i = 0; i < 3; i++)
                    {

                        TagBuilder div = new TagBuilder("div");

                        div.AddCssClass("sv-gv-filter-box");

                        TagBuilder span = new TagBuilder("span");
                        span.SetInnerText(i == 0 ? "=" : i == 1 ? ">" : "<");
                        var col = new TagBuilder("input");
                        string name = "";
                        switch (i)
                        {
                            case 0:
                                name = string.Format("{0}.filter.{1}.eq", metaname, item.Value.ColumnName);
                                break;
                            case 1:
                                name = string.Format("{0}.filter.{1}.gt", metaname, item.Value.ColumnName);
                                break;
                            case 2:
                                name = string.Format("{0}.filter.{1}.lt", metaname, item.Value.ColumnName);
                                break;
                            default:
                                break;
                        }

                        //
                        if (datatype == "numeric")
                        {
                            col.MergeAttribute("style", "text-align:right;");
                            col.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(item.Value.FormatValue, item.Value.CultureInfo).ToString());
                        }
                        col.AddCssClass(Services.Uti.GetClassNameByType(datatype, item.Value.FormatValue));
                        string attemptedValue = Services.ExConvert.Data2String(arrexpression == null ? "" : arrexpression[i] == null ? "" : arrexpression[i], datatype, item.Value.FormatValue, item.Value.CultureInfo);
                        //
                        col.MergeAttribute("name", name);
                        col.MergeAttribute("type", "text");
                        //col.MergeAttribute("value", arrexpression == null ? "" : arrexpression[i] == null ? "" : arrexpression[i]);
                        col.MergeAttribute("value", attemptedValue);
                        col.MergeAttribute("fieldname", name, false);

                        div.InnerHtml = span.ToString();
                        div.InnerHtml += col.ToString();

                        filtercol.InnerHtml += div.ToString();
                    }

                    //filtercol.InnerHtml = table.ToString();

                    if (item.Value.GridViewShow != true)
                    {
                        filtercol.MergeAttribute("style", "display: none;", false);
                    }
                    //filtercol.InnerHtml = table.ToString();
                    //filtertag += filtercol.ToString();
                }
                else
                {

                    var col = new TagBuilder("input");
                    string name = string.Format("{0}.filter.{1}", metaname, item.Value.ColumnName);
                    col.MergeAttribute("name", name);

                    col.MergeAttribute("type", "text");
                    col.MergeAttribute("value", item.Value.FilterExpression);

                    col.MergeAttribute("fieldname", name, false);
                    if (item.Value.GridViewShow != true)
                    {
                        filtercol.MergeAttribute("style", "display: none;", false);
                    }
                    TagBuilder div = new TagBuilder("div");
                    div.InnerHtml = col.ToString();
                    filtercol.InnerHtml += div.ToString();
                }

                filtertag += filtercol.ToString();
            }

            return MvcHtmlString.Create(filtertag);
        }

        public static MvcHtmlString svDataRow<TModel>(this HtmlHelper htmlHelper, TModel row, Dictionary<string, Models.SysTableDetailView> columns)
        {
            string datarow = "";

            foreach (var column in columns)
            {
                if (column.Value.UType == true) continue;

                var value = row.GetType().GetProperty(column.Value.ColumnName).GetValue(row, null) ?? String.Empty;
                var td = new TagBuilder("td");
                td.AddCssClass("sv-gv");
                td.MergeAttribute("sv-fieldname", column.Value.ColumnName, false);

                string datatype = Services.ExConvert.Sqltype2Systemtype(column.Value.DATA_TYPE);
                if (datatype == "boolean")
                {
                    var col = new TagBuilder("input");
                    col.MergeAttribute("type", "checkbox", true);
                    col.MergeAttribute("disabled", "disabled", true);

                    col.AddCssClass("check-box");
                    if (value.ToString() != "")
                    {
                        if ((bool)value)
                        {
                            col.MergeAttribute("checked", "checked");
                        }
                    }
                    td.InnerHtml = col.ToString();
                }
                else
                {
                    if (datatype == "numeric")
                    {
                        td.MergeAttribute("align", "right", false);
                    }
                    string _value = string.Format("<span title=\"{0}\">{0}</span>", Services.ExConvert.Data2String(value, datatype, column.Value.FormatValue, column.Value.CultureInfo));
                    td.InnerHtml = _value;
                }



                if (column.Value.GridViewShow != true)
                {
                    td.MergeAttribute("style", "display: none;", false);
                }

                datarow += td;
            } //kết thúc quét cột


            return MvcHtmlString.Create(datarow);
        }

        public static MvcHtmlString svHiddenFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return System.Web.Mvc.Html.InputExtensions.HiddenFor(html, expression);
        }

        public static MvcHtmlString svHiddenFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return System.Web.Mvc.Html.InputExtensions.HiddenFor(html, expression, htmlAttributes);
        }

        public static MvcHtmlString svDropDownList(this HtmlHelper htmlHelper, string name)
        {
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlHelper, name);
        }

        public static MvcHtmlString svDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList)
        {
            return System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList);
        }

        public static MvcHtmlString svTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return svTextAreaFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString svTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, htmlAttributes);
        }

        public static MvcHtmlString svGridReport(this HtmlHelper html, IEnumerable<dynamic> source)
        {
            if (source == null || source.Count() == 0)
            {
                return new MvcHtmlString(string.Empty);
            }
            string tablename = "";
            foreach (object item in source)
            {
                tablename = item.GetType().Name;
                break;
            }
            return svGridReport(html, source, tablename, false);

        }

        //lưới liệt kê danh sách có phân trang
        public static MvcHtmlString svGridReport(this HtmlHelper html, IEnumerable<dynamic> source, string tablename, bool isthrilldown)
        {

            var metaobject = Services.GlobalMeta.GetMetaObject(tablename);
            var columns = metaobject.GetMetaTable();

            TagBuilder divcontainer = new TagBuilder("div");
            divcontainer.AddCssClass("sv-grid-container");
            #region bảng phân trang
            TagBuilder tablepaging = new TagBuilder("table");
            tablepaging.AddCssClass("sv-grid-paging");
            TagBuilder tablepagingtr = new TagBuilder("tr");
            TagBuilder tablepagingtd = new TagBuilder("td");
            MvcHtmlString divpaging = System.Web.Mvc.Html.PartialExtensions.Partial(html, "_PagingPartial", metaobject.Paging);
            tablepagingtd.InnerHtml = divpaging.ToString();
            tablepagingtr.InnerHtml = tablepagingtd.ToString();
            tablepaging.InnerHtml = tablepagingtr.ToString();

            divcontainer.InnerHtml = tablepaging.ToString();

            #endregion

            #region border nội dung
            TagBuilder divbordertable = new TagBuilder("div");
            divbordertable.AddCssClass("sv-border-table");
            #region bảng nội dung
            TagBuilder table = new TagBuilder("table");

            table.MergeAttribute("tablename", tablename);
            table.AddCssClass("sv-gv-table");

            #region tạo header và filter

            string tagheader = "";
            string tagfilter = "";

            #region thrilldown
            if (isthrilldown)
            {

                TagBuilder tagheadercol = new TagBuilder("td");
                tagheadercol.AddCssClass("sv-gv-header");
                tagheadercol.MergeAttribute("style", string.Format("width:{0}px;", 48));
                tagheader += tagheadercol.ToString();

                TagBuilder tagfiltercol = new TagBuilder("td");
                tagfiltercol.AddCssClass("sv-gv-filter");
                tagfilter += tagfiltercol.ToString();
            }

            #endregion



            foreach (var item in columns)
            {
                tagheader += svHeaderColFor(html, item, tablename);
                tagfilter += svFilterColFor(html, item, tablename);
            }
            TagBuilder tableheader = new TagBuilder("tr");
            TagBuilder tablefilter = new TagBuilder("tr");
            tableheader.InnerHtml = tagheader;
            tablefilter.InnerHtml = tagfilter;

            table.InnerHtml = tableheader.ToString();
            table.InnerHtml += tablefilter.ToString();

            #endregion

            #region tạo dòng nội dung
            if (source != null && source.Count() > 0)
            {

                foreach (object item in source)
                {
                    if (item != null)
                    {
                        //var value = item.GetType().GetProperty(keyfield).GetValue(item, null) ?? String.Empty;
                        var value = "";
                        var tagcontent = new TagBuilder("tr");
                        tagcontent.AddCssClass("sv-gv-datarow");
                        tagcontent.MergeAttribute("sv-gv-datarow-para", value.ToString());

                        #region thrilldown
                        if (isthrilldown)
                        {
                            TagBuilder thrilldowncol = new TagBuilder("td");
                            thrilldowncol.AddCssClass("sv-gv");
                            TagBuilder thrilldownbutton = new TagBuilder("a");
                            thrilldownbutton.AddCssClass("btn sv-report-thrilldown");
                            thrilldownbutton.InnerHtml = "<i class=\"fa fa-plus-circle\"></i>";
                            thrilldowncol.InnerHtml = thrilldownbutton.ToString();
                            tagcontent.InnerHtml = thrilldowncol.ToString();
                        }

                        #endregion

                        tagcontent.InnerHtml += svDataRow(html, item, columns).ToString();
                        table.InnerHtml += tagcontent.ToString();
                    }
                }
            }

            #endregion

            #endregion
            divbordertable.InnerHtml = table.ToString();
            divcontainer.InnerHtml += divbordertable.ToString();
            #endregion

            return new MvcHtmlString(divcontainer.ToString());
        }


        public static MvcHtmlString svGridList(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield)
        {
            if (source == null || source.Count() == 0)
            {
                return new MvcHtmlString(string.Empty);
            }
            string tablename = "";
            foreach (object item in source)
            {
                tablename = item.GetType().Name;
                break;
            }
            return svGridList(html, source, keyfield, tablename);

        }

        //lưới liệt kê danh sách có phân trang
        public static MvcHtmlString svGridList(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield, string tablename)
        {

            //string tablename = "";
            //foreach (object item in source)
            //{
            //    tablename = item.GetType().Name;
            //    break;
            //}

            var metaobject = Services.GlobalMeta.GetMetaObject(tablename);
            var columns = metaobject.GetMetaTable();

            TagBuilder divcontainer = new TagBuilder("div");
            divcontainer.AddCssClass("sv-grid-container");
            #region bảng phân trang
            TagBuilder tablepaging = new TagBuilder("table");
            tablepaging.AddCssClass("sv-grid-paging");
            TagBuilder tablepagingtr = new TagBuilder("tr");
            TagBuilder tablepagingtd = new TagBuilder("td");
            MvcHtmlString divpaging = System.Web.Mvc.Html.PartialExtensions.Partial(html, "_PagingPartial", metaobject.Paging);
            tablepagingtd.InnerHtml = divpaging.ToString();
            tablepagingtr.InnerHtml = tablepagingtd.ToString();
            tablepaging.InnerHtml = tablepagingtr.ToString();

            #endregion

            #region border nội dung
            TagBuilder divbordertable = new TagBuilder("div");
            divbordertable.AddCssClass("sv-border-table");
            #region bảng nội dung
            TagBuilder table = new TagBuilder("table");

            table.MergeAttribute("tablename", tablename);
            table.AddCssClass("sv-gv-table");


            table.MergeAttribute("data-senviet-keys", keyfield);

            #region tạo header và filter

            string tagheader = "";
            string tagfilter = "";
            foreach (var item in columns)
            {
                tagheader += svHeaderColFor(html, item, tablename);
                tagfilter += svFilterColFor(html, item, tablename);
            }
            TagBuilder tableheader = new TagBuilder("tr");
            TagBuilder tablefilter = new TagBuilder("tr");
            tableheader.InnerHtml = tagheader;
            tablefilter.InnerHtml = tagfilter;

            table.InnerHtml = tableheader.ToString();
            table.InnerHtml += tablefilter.ToString();

            #endregion

            #region tạo dòng nội dung
            if (source != null && source.Count() > 0)
            {

                foreach (object item in source)
                {
                    if (item != null)
                    {

                        var arrkey = keyfield.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        string strpara = "";

                        foreach (var keyname in arrkey)
                        {
                            var value = item.GetType().GetProperty(keyname).GetValue(item, null) ?? String.Empty;
                            strpara += string.Format("&{0}={1}", keyname, value);
                        }

                        var tagcontent = new TagBuilder("tr");
                        tagcontent.AddCssClass("sv-gv-datarow");
                        tagcontent.MergeAttribute("sv-gv-datarow-para", strpara.ToString());
                        tagcontent.InnerHtml = svDataRow(html, item, columns).ToString();
                        table.InnerHtml += tagcontent.ToString();


                        //var value = item.GetType().GetProperty(keyfield).GetValue(item, null) ?? String.Empty;

                        //var tagcontent = new TagBuilder("tr");
                        //tagcontent.AddCssClass("sv-gv-datarow");
                        //tagcontent.MergeAttribute("sv-gv-datarow-para", value.ToString());
                        //tagcontent.InnerHtml = svDataRow(html, item, columns).ToString();
                        //table.InnerHtml += tagcontent.ToString();
                    }
                }
            }

            #endregion

            #endregion
            divbordertable.InnerHtml = table.ToString();
            divcontainer.InnerHtml += divbordertable.ToString();
            #endregion

            divcontainer.InnerHtml += tablepaging.ToString();

            return new MvcHtmlString(divcontainer.ToString());
        }

        //lưới nhập liệu chứng từ
        public static MvcHtmlString svGridVoucher(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield, string defaultkeyvalue, object newitem)
        {

            TagBuilder table = new TagBuilder("table");

            string tablename = "";
            tablename = newitem.GetType().Name;


            var columns = Services.GlobalMeta.GetMetaObject(tablename).GetMetaTable();

            table.MergeAttribute("tablename", tablename);
            table.MergeAttribute("keyfield", keyfield);
            table.MergeAttribute("defaultkeyvalue", defaultkeyvalue);
            table.AddCssClass("sv-grid-voucher");


            #region tạo header
            var theader = new TagBuilder("thead");
            var trheader = new TagBuilder("tr");
            foreach (var item in columns)
            {
                var th = new TagBuilder("th");
                th.InnerHtml = item.Value.Des;

                th.MergeAttribute("style", string.Format("width: {0}px;", item.Value.GridViewWidth + 14));

                if (item.Value.GridViewShow != true)
                {
                    th.MergeAttribute("style", string.Format("width: {0}px;display: none;", item.Value.GridViewWidth), true);
                }

                th.MergeAttribute("fieldname", item.Value.ColumnName);

                trheader.InnerHtml += th;
            }
            var thdelete = new TagBuilder("th");
            thdelete.MergeAttribute("style", "width: 52px;");
            trheader.InnerHtml += thdelete.ToString();
            theader.InnerHtml = trheader.ToString();
            table.InnerHtml += theader;
            #endregion

            #region Tạo từng dòng của bảng

            //Tạo dòng thêm mới dữ liệu
            table.InnerHtml += AddRowNew(html, newitem, columns, tablename, keyfield, -1);



            if (source != null)
            {
                int i = 0;
                foreach (object item in source)
                {
                    if (item != null)
                    {
                        table.InnerHtml += AddRow(html, item, columns, tablename, keyfield, i);
                        i++;
                    }
                }
            }

            #endregion

            #region Tạo script xóa và thêm mới cho bảng
            //Phải tìm giải pháp đồng bộ bộ cultureinfo của script với cấu hình hệ thống

            var div = new TagBuilder("div");
            div.AddCssClass("border-table");
            string fc = @"function senvietgridremoverow(delobject) {
            $(delobject).parent().find(':hidden').val('-1');
            $(delobject).parent().parent().hide();
            //$(delobject).parent().parent().remove();
            if (window.form_refresh)
            {
               form_refresh();
            }
            }";

            fc += @"    function senvietgridaddrow(a) {
        var tablename = $(a).closest('table').attr('tablename');
        var keyfield = $(a).closest('table').attr('keyfield');
        var keyvalue = $(a).closest('table').attr('defaultkeyvalue');

        var tbbody = $(a).closest('tbody');
        var rowcount = $(tbbody).children().size()-1;
        
        var newrow = $(tbbody).find('tr[rownumber=-1]').clone();

        //reset lại giá trị dòng thêm sau khi copy ra để nhập mới
        $(tbbody).find('tr[rownumber=-1]').find('input').val('');


        $(newrow).attr({rownumber: rowcount });

        $(tbbody).find('tr:last').after(newrow)
        $(tbbody).find('tr:last').show();

        $(tbbody).find('tr:last>td').each(function (index, value) {

            var col = $(value).find('input').first();
            var fieldname = $(col).attr('fieldname');
            $(col).attr({
                name: tablename + 's[' + rowcount + '].' + fieldname,
                id: tablename + 's_' + rowcount + '__' + fieldname
            });
            //$(col).val('');

            if (fieldname == keyfield) {
                $(col).val(keyvalue);
            };
            if (fieldname == tablename) {

                $(col).val(keyvalue);
                $(col).attr({
                    name: tablename + 'z[' + rowcount + ']'
                });
            };
        });";

            fc += " $(tbbody).find('tr:last td:last').find('button').remove(); ";
            fc += " $(tbbody).find('tr:last td:last').prepend('<a class=\"btn btn-default\" onclick=\"senvietgridremoverow(this)\" href=\"javascript:void(0);\"> <span class=\"fa fa-minus-circle\"></span> </a>'); ";

            //"<a onclick='remove(this)' href='javascript:void(0);'> Xóa </a>"

            fc += "$(tbbody).find('tr:last input[isautocomplete = \"isautocomplete\"]').each(function (i) {";

            UrlHelper u = new UrlHelper(html.ViewContext.RequestContext);
            string url = u.Action("autocomplete", "Services", new { area = "Accounting" });
            fc += string.Format("$(this).lookup('{0}'){1}", url, ";}); ");

            fc += @"$('.numeric').each(function (i) {
                        var decimaldigits = $(this).attr('decimaldigits');
                        $(this).autoNumeric({ mDec: decimaldigits });
                    });
                    $('.datetime').datetimeEntry({ datetimeFormat: 'D/O/Y', spinnerImage: '' });
                    $('.datetime-g').datetimepicker({ datetimeFormat: 'D/O/Y H:M', spinnerImage: '' });
                    $(a).closest('tr').find('input:visible:first').focus().select();
                    if (window.form_refresh)
                    {
                       form_refresh();
                    }
                    }";
            string sc = string.Format(@"<script type='text/javascript'>{0}</script>", fc);

            #endregion

            div.InnerHtml = sc;
            div.InnerHtml += table;

            //div.InnerHtml += @"<div class='sv-button-addline'><a href='javascript:void(0);' onclick='senvietgridaddrow(this)'> Thêm dòng</a></div>";

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString svGridVoucher(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield, string defaultkeyvalue)
        {
            if (source == null || source.Count() == 0)
            {
                return new MvcHtmlString(string.Empty);
            }

            TagBuilder table = new TagBuilder("table");

            string tablename = "";
            foreach (object item in source)
            {
                tablename = item.GetType().Name;
                break;
            }

            var columns = Services.GlobalMeta.GetMetaObject(tablename).GetMetaTable();

            table.MergeAttribute("tablename", tablename);
            table.MergeAttribute("keyfield", keyfield);
            table.MergeAttribute("defaultkeyvalue", defaultkeyvalue);
            table.AddCssClass("sv-grid-voucher");


            #region tạo header
            var theader = new TagBuilder("thead");
            var trheader = new TagBuilder("tr");
            foreach (var item in columns)
            {
                var th = new TagBuilder("th");
                th.InnerHtml = item.Value.Des;

                th.MergeAttribute("style", string.Format("width: {0}px;", item.Value.GridViewWidth));

                if (item.Value.GridViewShow != true)
                {
                    th.MergeAttribute("style", string.Format("width: {0}px;display: none;", item.Value.GridViewWidth), true);
                }

                th.MergeAttribute("fieldname", item.Value.ColumnName);

                trheader.InnerHtml += th;
            }
            var thdelete = new TagBuilder("th");
            thdelete.MergeAttribute("style", "width: 48px;");
            trheader.InnerHtml += thdelete.ToString();
            theader.InnerHtml = trheader.ToString();
            table.InnerHtml += theader;
            #endregion

            #region Tạo từng dòng của bảng
            var newrow = source.FirstOrDefault();

            table.InnerHtml += AddRow(html, newrow, columns, tablename, keyfield, -1);

            int i = 0;
            foreach (object item in source)
            {
                if (item != null)
                {
                    table.InnerHtml += AddRow(html, item, columns, tablename, keyfield, i);
                    i++;
                }
            }
            #endregion

            #region Tạo script xóa và thêm mới cho bảng
            //Phải tìm giải pháp đồng bộ bộ cultureinfo của script với cấu hình hệ thống

            var div = new TagBuilder("div");
            string fc = @"function senvietgridremoverow(delobject) {$(delobject).parent().find(':hidden').val('');
            $(delobject).parent().parent().hide();
            if (window.form_refresh)
            {
               form_refresh();
            }
            //$(delobject).parent().parent().find('input[type=text]').trigger('change');
            }";

            fc += @"    function addnew(a) {
        var tablename = $(a).parent().parent().find('table').attr('tablename');
        var keyfield = $(a).parent().parent().find('table').attr('keyfield');
        var keyvalue = $(a).parent().parent().find('table').attr('defaultkeyvalue');

        var tbbody = $(a).parent().parent().find('table >tbody');
        var rowcount = $(tbbody).children().size();
        var newrow = $(tbbody).find('tr:last').clone();
        $(tbbody).find('tr:last').after(newrow)
        $(tbbody).find('tr:last').show();

        $(tbbody).find('tr:last>td').each(function (index, value) {

            var col = $(value).find('input').first();
            var fieldname = $(col).attr('fieldname');
            $(col).attr({
                name: tablename + 's[' + rowcount + '].' + fieldname,
                id: tablename + 's_' + rowcount + '__' + fieldname
            });
            $(col).val('');

            if (fieldname == keyfield) {
                $(col).val(keyvalue);
            };
            if (fieldname == tablename) {

                $(col).val(keyvalue);
                $(col).attr({
                    name: tablename + 'z[' + rowcount + ']'
                });
            };
        });";

            fc += "$(tbbody).find('tr:last input[isautocomplete = \"isautocomplete\"]').each(function (i) {";

            UrlHelper u = new UrlHelper(html.ViewContext.RequestContext);
            string url = u.Action("AutoComplete", "Services", new { area = "Accounting" });
            fc += string.Format("$(this).lookup('{0}'){1}", url, ";}); ");

            fc += @"$('.numeric').each(function (i) {
                        var decimaldigits = $(this).attr('decimaldigits');
                        $(this).autoNumeric({ mDec: decimaldigits });
                    });
                    $('.datetime').removeClass('hasDatepicker');
                    $.datepicker.setDefaults($.datepicker.regional['vi']); 
                    $('.datetime').datepicker($.datepicker.regional['vi']); 
                    $(tbbody).find('tr:last').find('input:visible:first').focus();}";

            string sc = string.Format(@"<script type='text/javascript'>{0}</script>", fc);

            #endregion

            div.InnerHtml = sc;
            div.InnerHtml += table;


            div.InnerHtml += @"<div class='sv-button-addline'><a href='javascript:void(0);' onclick='addnew(this)'> Thêm dòng</a></div>";
            //            div.InnerHtml += @"<button type='button' class='sv-button-addline' onclick='addnew(this)'>Thêm dòng</button>";

            return new MvcHtmlString(div.ToString());
        }

        private static MvcHtmlString AddRowNew(this HtmlHelper html, object row, Dictionary<string, Models.SysTableDetailView> columns, string tablename, string keyfield, int rownumber)
        {
            //string tablename = columns.SingleOrDefault().Value.TableName;
            var tr = new TagBuilder("tr");
            var keyvalue = row.GetType().GetProperty(keyfield).GetValue(row, null) ?? String.Empty;
            tr.MergeAttribute("keyvalue", keyvalue.ToString());
            tr.MergeAttribute("rownumber", rownumber.ToString());

            foreach (var column in columns)
            {
                var value = row.GetType().GetProperty(column.Value.ColumnName).GetValue(row, null) ?? String.Empty;
                var td = new TagBuilder("td");

                var col = new TagBuilder("input");
                string name = string.Format("{0}s[{1}].{2}", column.Value.TableName, rownumber, column.Value.ColumnName);
                col.MergeAttribute("name", name);
                col.GenerateId(name);


                col.MergeAttribute("type", "text");
                col.MergeAttribute("value", value.ToString());
                col.MergeAttribute("fieldname", column.Value.ColumnName);

                //gán kiểu để kiểm tra sẽ bỏ khi test ok//
                col.MergeAttribute("field-type", row.GetType().GetProperty(column.Value.ColumnName).PropertyType.ToString());



                if (column.Value.ReadOnly == true)
                {
                    col.MergeAttribute("readonly", "readonly", true);
                    //builder.MergeAttribute("disabled", "disabled", true);
                }

                if (column.Value.IsValid == true)
                {
                    col.MergeAttribute("isautocomplete", "isautocomplete", true);
                }


                //Thiết lập độ rộng cột
                col.MergeAttribute("style", string.Format("width: {0}px", column.Value.GridViewWidth));
                //Kết thúc Thiết lập độ rộng cột

                string datatype = Services.ExConvert.Sqltype2Systemtype(column.Value.DATA_TYPE); //row.GetType().GetProperty(column.Value.ColumnName).PropertyType.ToString();
                if (datatype == "boolean")
                {
                    col.MergeAttribute("type", "checkbox", true);
                    col.AddCssClass("check-box");
                    col.MergeAttribute("value", "true", true);
                    if (value.ToString() != "")
                    {
                        if ((bool)value)
                        {
                            col.MergeAttribute("checked", "checked");
                        }
                    }

                    col.InnerHtml = string.Format("<input type='hidden' value='false' name='{0}'>", name);
                }
                else
                {
                    col.AddCssClass(Uti.GetClassNameByType(datatype, column.Value.FormatValue));
                    col.MergeAttribute("value", Services.ExConvert.Data2String(value, datatype, column.Value.FormatValue, column.Value.CultureInfo), true);
                }
                if (datatype == "numeric")
                {
                    //gán số lẻ nếu là trường số
                    col.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(column.Value.FormatValue, column.Value.CultureInfo).ToString());
                }


                // If there are any errors for a named field, we add the css attribute.
                //string attemptedValue = null;
                ModelState modelState = null;
                html.ViewData.ModelState.TryGetValue(name, out modelState);
                string errormessage = "";
                if (modelState != null)
                {
                    if (modelState.Errors.Count > 0)
                    {
                        col.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                        //errormessage = string.Format("<span class='field-validation-error'>{0}</span>", modelState.Errors[0].ErrorMessage);
                    }
                }

                // Create tag builder  
                col.InnerHtml += errormessage;

                if (datatype == "numeric")
                {
                    col.MergeAttribute("style", string.Format("width: {0}px;text-align:right;", column.Value.GridViewWidth), true);
                }

                td.InnerHtml = col.ToString();


                if (column.Value.GridViewShow != true)
                {
                    td.MergeAttribute("style", "display: none;");
                }




                tr.InnerHtml += td;
            } //kết thúc quét cột

            //var tdedit = new TagBuilder("td") { InnerHtml = string.Format(@"<a class='sv-button-addline' href='javascript:void(0);' onclick='senvietgridaddrow(this)'> Thêm dòng</a>") };
            //div.InnerHtml += @"<button type='button' class='sv-button-addline' onclick='addnew(this)'>Thêm dòng</button>";
            var tdedit = new TagBuilder("td") { InnerHtml = string.Format(@"<button type='button' class='btn btn-primary' onclick='senvietgridaddrow(this)'><span class='fa fa-plus-circle'></span></button>") };
            var deltag = new TagBuilder("input");

            string namedeltag = string.Format("{0}z[{1}]", tablename, rownumber);
            deltag.MergeAttribute("type", "hidden");
            deltag.MergeAttribute("name", namedeltag);
            deltag.MergeAttribute("fieldname", tablename);

            if (html.ViewData.ModelState[namedeltag] != null)
            {

                deltag.MergeAttribute("value", html.ViewData.ModelState[namedeltag].Value.AttemptedValue.ToString());
                if (string.IsNullOrEmpty(html.ViewData.ModelState[namedeltag].Value.AttemptedValue.ToString()))
                {
                    tr.MergeAttribute("style", "display: none;");

                }

            }
            else
            {
                deltag.MergeAttribute("value", keyvalue.ToString());

            }

            deltag.GenerateId(namedeltag);



            tdedit.InnerHtml += deltag.ToString();

            tr.InnerHtml += tdedit.ToString();

            return MvcHtmlString.Create(tr.ToString());
        }

        private static MvcHtmlString AddRow(this HtmlHelper html, object row, Dictionary<string, Models.SysTableDetailView> columns, string tablename, string keyfield, int rownumber)
        {
            //string tablename = columns.SingleOrDefault().Value.TableName;
            var tr = new TagBuilder("tr");
            var keyvalue = row.GetType().GetProperty(keyfield).GetValue(row, null) ?? String.Empty;
            tr.MergeAttribute("keyvalue", keyvalue.ToString());
            tr.MergeAttribute("rownumber", rownumber.ToString());

            foreach (var column in columns)
            {
                var value = row.GetType().GetProperty(column.Value.ColumnName).GetValue(row, null) ?? String.Empty;
                var td = new TagBuilder("td");

                var col = new TagBuilder("input");
                string name = string.Format("{0}s[{1}].{2}", column.Value.TableName, rownumber, column.Value.ColumnName);
                col.MergeAttribute("name", name);
                col.GenerateId(name);


                col.MergeAttribute("type", "text");
                col.MergeAttribute("value", value.ToString());
                col.MergeAttribute("fieldname", column.Value.ColumnName);

                //gán kiểu để kiểm tra sẽ bỏ khi test ok//
                col.MergeAttribute("field-type", row.GetType().GetProperty(column.Value.ColumnName).PropertyType.ToString());



                if (column.Value.ReadOnly == true)
                {
                    col.MergeAttribute("readonly", "readonly", true);
                    //builder.MergeAttribute("disabled", "disabled", true);
                }

                if (column.Value.IsValid == true)
                {
                    col.MergeAttribute("isautocomplete", "isautocomplete", true);
                }


                //Thiết lập độ rộng cột
                col.MergeAttribute("style", string.Format("width: {0}px", column.Value.GridViewWidth));
                //Kết thúc Thiết lập độ rộng cột

                string datatype = Services.ExConvert.Sqltype2Systemtype(column.Value.DATA_TYPE); //row.GetType().GetProperty(column.Value.ColumnName).PropertyType.ToString();
                if (datatype == "boolean")
                {
                    col.MergeAttribute("type", "checkbox", true);
                    col.AddCssClass("check-box");
                    col.MergeAttribute("value", "true", true);
                    if (value.ToString() != "")
                    {
                        if ((bool)value)
                        {
                            col.MergeAttribute("checked", "checked");
                        }
                    }

                    col.InnerHtml = string.Format("<input type='hidden' value='false' name='{0}'>", name);
                }
                else
                {
                    col.AddCssClass(Uti.GetClassNameByType(datatype, column.Value.FormatValue));
                    col.MergeAttribute("value", Services.ExConvert.Data2String(value, datatype, column.Value.FormatValue, column.Value.CultureInfo), true);
                }
                if (datatype == "numeric")
                {
                    //gán số lẻ nếu là trường số
                    col.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(column.Value.FormatValue, column.Value.CultureInfo).ToString());
                }


                // If there are any errors for a named field, we add the css attribute.
                //string attemptedValue = null;
                ModelState modelState = null;
                html.ViewData.ModelState.TryGetValue(name, out modelState);
                string errormessage = "";
                if (modelState != null)
                {
                    if (modelState.Errors.Count > 0)
                    {
                        col.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                        //errormessage = string.Format("<span class='field-validation-error'>{0}</span>", modelState.Errors[0].ErrorMessage);
                    }
                }

                // Create tag builder  
                col.InnerHtml += errormessage;

                if (datatype == "numeric")
                {
                    col.MergeAttribute("style", string.Format("width: {0}px;text-align:right;", column.Value.GridViewWidth), true);
                }

                td.InnerHtml = col.ToString();


                if (column.Value.GridViewShow != true)
                {
                    td.MergeAttribute("style", "display: none;");
                }




                tr.InnerHtml += td;
            } //kết thúc quét cột

            var tdedit = new TagBuilder("td") { InnerHtml = string.Format("<a class='btn btn-default' onclick='senvietgridremoverow(this)' href='javascript:void(0);'> <span class='fa fa-minus-circle'></span> </a>") };
            var deltag = new TagBuilder("input");

            string namedeltag = string.Format("{0}z[{1}]", tablename, rownumber);
            deltag.MergeAttribute("type", "hidden");
            deltag.MergeAttribute("name", namedeltag);
            deltag.MergeAttribute("fieldname", tablename);

            if (html.ViewData.ModelState[namedeltag] != null)
            {

                deltag.MergeAttribute("value", html.ViewData.ModelState[namedeltag].Value.AttemptedValue.ToString());
                string valuedel = html.ViewData.ModelState[namedeltag].Value.AttemptedValue.ToString();
                if (valuedel == "-1")
                {
                    tr.MergeAttribute("style", "display: none;");
                }



            }
            else
            {
                deltag.MergeAttribute("value", keyvalue.ToString());

            }

            deltag.GenerateId(namedeltag);



            tdedit.InnerHtml += deltag.ToString();

            tr.InnerHtml += tdedit.ToString();

            return MvcHtmlString.Create(tr.ToString());
        }

        public static svGridView<TModel> svGridViewFor<TModel>(this HtmlHelper helper) where TModel : class
        {
            return new svGridView<TModel>(helper);
        }
    }

    public class svGridView<TModel>
    {
        private HtmlHelper HtmlHelper { get; set; }
        private IEnumerable<TModel> Data { get; set; }

        private string AjaxUpdateID { get; set; }
        private string AjaxLoadingID { get; set; }
        private string PrimaryKey { get; set; }
        private Dictionary<string, string[]> Excolumns { get; set; }


        private svGridView() { }



        internal svGridView(HtmlHelper helper)
        {
            this.HtmlHelper = helper;

            this.Excolumns = new Dictionary<string, string[]>();
        }

        public svGridView<TModel> Excolumn(string action, string[] para)
        {
            this.Excolumns.Add(action, para);
            return this;
        }

        public svGridView<TModel> DataSource(IEnumerable<TModel> dataSource)
        {
            this.Data = dataSource;
            return this;
        }

        public svGridView<TModel> AjaxSetting(string ajaxupdateid, string ajaxloadingid = null)
        {
            this.AjaxUpdateID = ajaxupdateid;
            this.AjaxLoadingID = ajaxloadingid;
            return this;
        }


        public MvcHtmlString ToHtml(string tablename)
        {
            //IEnumerable<dynamic> source, 

            var metaobject = Services.GlobalMeta.GetMetaObject(tablename).GetMetaTable();



            TagBuilder tagtable = new TagBuilder("table");
            tagtable.AddCssClass("sv-gv-table");

            #region Tạo header và filter

            // tag header
            TagBuilder tagtableheader = new TagBuilder("tr");

            // tag filter
            TagBuilder tagtablefilter = new TagBuilder("tr");

            foreach (var item in metaobject)
            {
                tagtableheader.InnerHtml += svHeaderFor(tablename, item.Value.ColumnName);
                tagtablefilter.InnerHtml += svFilterFor(tablename, item.Value.ColumnName);
            }

            tagtable.InnerHtml += tagtableheader;
            tagtable.InnerHtml += tagtablefilter;

            #endregion

            int row = 0;
            foreach (TModel model in this.Data)
            {
                tagtable.InnerHtml += svDataRow(model, metaobject);
                row++;
            }



            //var actionlink = new UrlHelper(HtmlHelper.ViewContext.RequestContext);
            //RouteValueDictionary A = new RouteValueDictionary();
            //A.Add("id", "cuimia");


            //TagBuilder excol = new TagBuilder("a");
            //excol.MergeAttribute("href", actionlink.Action("Edit",A));
            //excol.MergeAttribute("data-ajax-update", "#systabledetail_container");
            //excol.MergeAttribute("data-ajax-mode", "replace");
            //excol.MergeAttribute("data-ajax-method", "GET");
            //excol.MergeAttribute("data-ajax-loading", "#ajaxloadingelementid");
            //excol.MergeAttribute("data-ajax", "true");
            //excol.SetInnerText("Sửa");

            //<a          
            //            href="/SysAdmin/SysColumn/Edit/AccountCreditID" 
            //            data-ajax-update="#syscolumn_container" 
            //            data-ajax-mode="replace" 
            //            data-ajax-method="GET" 
            //            data-ajax-loading="#ajaxloadingelementid" 
            //            data-ajax="true">
            //            Sửa
            //</a>

            //tagtable.InnerHtml += excol.ToString();



            return MvcHtmlString.Create(tagtable.ToString());

        }

        public MvcHtmlString svDataRow(TModel row, Dictionary<string, Models.SysTableDetailView> columns)
        {
            TagBuilder tagtr = new TagBuilder("tr");
            tagtr.AddCssClass("sv-gv-datarow");

            foreach (var column in columns)
            {
                var value = row.GetType().GetProperty(column.Value.ColumnName).GetValue(row, null) ?? String.Empty;
                var td = new TagBuilder("td");
                td.AddCssClass("sv-gv");




                string datatype = Services.ExConvert.Sqltype2Systemtype(column.Value.DATA_TYPE);
                if (datatype == "boolean")
                {
                    var col = new TagBuilder("input");
                    col.MergeAttribute("type", "checkbox", true);
                    col.MergeAttribute("disabled", "disabled", true);

                    col.AddCssClass("check-box");
                    if (value.ToString() != "")
                    {
                        if ((bool)value)
                        {
                            col.MergeAttribute("checked", "checked");
                        }
                    }
                    td.InnerHtml = col.ToString();
                }
                else
                {
                    td.SetInnerText(value.ToString());
                }



                if (column.Value.GridViewShow != true)
                {
                    td.MergeAttribute("style", "display: none;");
                }

                tagtr.InnerHtml += td;
            } //kết thúc quét cột


            return MvcHtmlString.Create(tagtr.ToString());
        }

        public MvcHtmlString svHeaderFor(string tablename, string columnname)
        {
            var MetaColumns = Services.GlobalMeta.GetMetaObject(tablename).GetMetaTable();

            TagBuilder tagheadercol = new TagBuilder("td");
            tagheadercol.AddCssClass("sv-gv-header");
            tagheadercol.MergeAttribute("id", string.Format("{0}.sort.{1}", tablename, columnname));


            TagBuilder table = new TagBuilder("table");
            table.MergeAttribute("cellspacing", "0");
            table.MergeAttribute("cellpadding", "0");
            table.MergeAttribute("style", "width:100%;border-collapse:collapse;");
            TagBuilder table_tr = new TagBuilder("tr");
            TagBuilder table_td = new TagBuilder("td");
            table_td.MergeAttribute("style", "width:100%;");
            table_td.SetInnerText(MetaColumns[columnname].Des);
            table_tr.InnerHtml = table_td.ToString();

            if (!string.IsNullOrWhiteSpace(MetaColumns[columnname].OrderExpression))
            {
                tagheadercol.MergeAttribute("OrderExpression", MetaColumns[columnname].OrderExpression);
                TagBuilder table_tdorder = new TagBuilder("td");
                table_tdorder.MergeAttribute("id", "padding-left: 5px;");
                table_tdorder.SetInnerText(MetaColumns[columnname].OrderExpression);

                TagBuilder imgsort = new TagBuilder("div");
                imgsort.AddCssClass(string.Format("sv-gv-header-{0}", MetaColumns[columnname].OrderExpression));

                table_tdorder.InnerHtml = imgsort.ToString();

                table_tr.InnerHtml += table_tdorder.ToString();
            }


            table.InnerHtml = table_tr.ToString();

            tagheadercol.InnerHtml = table.ToString();

            return MvcHtmlString.Create(tagheadercol.ToString());
        }

        public MvcHtmlString svFilterFor(string tablename, string columnname)
        {
            var MetaColumns = Services.GlobalMeta.GetMetaObject(tablename).GetMetaTable();


            TagBuilder filtercol = new TagBuilder("td");
            filtercol.AddCssClass("sv-gv-filter");


            var col = new TagBuilder("input");
            string name = string.Format("{0}.filter.{1}", tablename, columnname);
            col.MergeAttribute("name", name);
            //Thiết lập độ rộng cột
            col.MergeAttribute("style", string.Format("width: {0}px", MetaColumns[columnname].GridViewWidth));
            //Kết thúc Thiết lập độ rộng cột


            switch (Services.ExConvert.Sqltype2Systemtype(MetaColumns[columnname].DATA_TYPE))
            {
                //case "boolean":
                //    col.MergeAttribute("type", "checkbox", true);
                //    col.AddCssClass("check-box");
                //    col.MergeAttribute("value", "true", true);
                //    if (!string.IsNullOrWhiteSpace(MetaColumns[columnname].FilterExpression))
                //    {
                //        if (Boolean.Parse(MetaColumns[columnname].FilterExpression))
                //        {
                //            col.MergeAttribute("checked", "checked");
                //        }
                //    }

                //    col.InnerHtml = string.Format("<input type='hidden' value='false' name='{0}'>", name);
                //    break;
                default:
                    col.MergeAttribute("type", "text");
                    col.MergeAttribute("value", MetaColumns[columnname].FilterExpression);
                    break;
            }

            col.MergeAttribute("fieldname", name);

            filtercol.InnerHtml = col.ToString();

            return MvcHtmlString.Create(filtercol.ToString());
        }

    }

    public static class Uti
    {
        public static string GetClassNameByType(string datatype, string formatvalue)
        {
            switch (datatype)
            {
                case "System.Nullable`1[System.Boolean]":
                    return "boolean";
                case "System.Boolean":
                    return "boolean";
                case "System.Nullable`1[System.DateTime]":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.DateTime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.Nullable`1[System.Decimal]":
                    return "numeric";
                case "System.Nullable`1[System.Int32]":
                    return "numeric";
                case "numeric":
                    return "numeric";
                case "datetime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                default: // nếu là string
                    return "";
            }

        }

        public static class Date
        {
            public static void SetDateRange(HttpRequestBase request, out  DateTime DateFrom, out DateTime DateTo)
            {

                #region xử lý điều kiện ngày
                try
                {
                    //((System.Globalization.CultureInfo)SenViet.GlobeVariant.CultureInfo["CICC"]).DateTimeFormat

                    DateFrom = DateTime.Parse(request.Params["DateFrom"] ?? DateTime.Now.Date.ToShortDateString()); // Default min giờ.
                    DateTo = DateTime.Parse(request.Params["DateTo"] ?? DateTime.Now.Date.ToShortDateString());
                    DateTo = DateTo.Date.AddDays(1).AddTicks(-1); // Set Max giờ.


                    //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
                    //controller.ViewBag.DateTo = DateTo.ToShortDateString();
                    //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";

                }
                catch (Exception)
                {
                    DateFrom = DateTime.Now.Date;
                    DateTo = DateTime.Now.Date;
                    //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
                    //controller.ViewBag.DateTo = DateTo.ToShortDateString();
                    //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";
                }

                #endregion
            }

            public static int GetMonthRange(DateTime DateFrom, DateTime DateTo)
            {
                if (DateFrom > DateTo)
                {
                    return 0;
                }
                DateTime DateTemp = DateFrom;
                int MonthRange = 0;
                while ((DateTemp.Year < DateTo.Year) || (DateTemp.Year == DateTo.Year && DateTemp.Month < DateTo.Month))
                {
                    MonthRange++;
                    DateTemp = DateTemp.AddMonths(1);
                }
                return MonthRange;
            }

            public static int GetDayRange(DateTime DateFrom, DateTime DateTo)
            {
                if (DateFrom > DateTo)
                {
                    return 0;
                }
                TimeSpan ts = DateTo - DateFrom;
                // Difference in days.
                int DayRange = ts.Days;
                return DayRange;
            }
        }

        public static string GetId()
        {
            string _filename = string.Format("{0}", DateTime.UtcNow.ToString("yyyyMMddhhss"));
            return _filename;
        }
    }
}