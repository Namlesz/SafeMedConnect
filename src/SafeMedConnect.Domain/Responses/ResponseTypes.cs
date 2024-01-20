namespace SafeMedConnect.Domain.Responses;

public enum ResponseTypes
{
    Unknown = 0,
    Success = 1,
    Error = 2,
    NotFound = 3,
    Conflict = 4,
    Forbidden = 5,
    InvalidRequest = 6
}