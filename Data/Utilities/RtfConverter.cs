using ReasonableRTF;
using ReasonableRTF.Enums;
using ReasonableRTF.Models;

namespace Data.Utilities
{
    internal class RtfConverter
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
                return (
                    string.Empty, ex);
            }
        }
    }
}
