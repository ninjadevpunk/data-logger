using Core.Enums;
using Core.Models;

namespace Data.Utilities
{
    internal class Extensions
    {
        internal static LOG.CATEGORY GetLogCategory(LogContext context)
        {
            return context switch
            {
                LogContext.Qt or LogContext.AndroidStudio => LOG.CATEGORY.CODING,
                LogContext.NOTES => LOG.CATEGORY.NOTES,
                _ => LOG.CATEGORY.CODING,
            };
        }
    }
}
