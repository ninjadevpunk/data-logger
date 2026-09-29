using Core.Enums;
using Core.Utilities;

namespace Core.DTOs.Cache
{
    public record QtCacheDTO : CodingCacheDTO
    {
        public override LogContext Context => LogContext.Qt;

        public override string ApplicationName => Constants.Qt;
    }
}
