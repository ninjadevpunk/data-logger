using Core.Enums;
using Core.Utilities;

namespace Core.DTOs.Data
{
    public sealed record QtDataDTO : CodingDataDTO
    {
        public override LogContext Context => LogContext.Qt;
        public override int AppId  => 1;
        public override string ApplicationName => Constants.Qt;
    }
}
