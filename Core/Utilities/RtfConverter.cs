using ReasonableRTF;
using ReasonableRTF.Enums;
using ReasonableRTF.Models;

namespace Core.Utilities
{
    public class RtfConverter
    {
        public static (string Text, Exception? Exception) ToPlainText(string rtfContent)
        {
            if (string.IsNullOrWhiteSpace(rtfContent))
                return (string.Empty, null);

            try
            {
                RtfResult result = ConvertRtf.ToText(rtfContent);

                if (result.Error == RtfError.OK)
                    return (result.Text, null);

                return (string.Empty, result.Exception);
            }
            catch (Exception ex)
            {
                return (string.Empty, ex);
            }
        }

        /// <summary>
        /// A lightweight structural helper to check if an RTF string has actual readable text.
        /// </summary>
        public static bool HasContent(string rtfContent)
        {
            var (text, _) = ToPlainText(rtfContent);
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
