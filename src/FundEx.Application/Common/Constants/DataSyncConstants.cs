namespace FundEx.Application.Common.Constants;

using FundEx.Domain.Enums;

public static class DataSyncConstants
{
    public static readonly IReadOnlyList<string> FundTypeCodes = Enum.GetNames<FundTypeCode>();
}
