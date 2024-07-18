namespace SafeMedConnect.Domain.Enums;

public enum ApiResponseTypes
{
    Unknown = 0,
    Success = 1,
    Error = 2,
    NotFound = 3,
    Conflict = 4,
    Forbidden = 5,
    InvalidRequest = 6,
    Unauthorized = 7
}