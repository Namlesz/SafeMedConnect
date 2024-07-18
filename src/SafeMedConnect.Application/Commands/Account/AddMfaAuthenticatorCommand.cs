using MediatR;
using QRCoder;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;
using SixLabors.ImageSharp.Formats.Png;

namespace SafeMedConnect.Application.Commands.Account;

public record AddMfaAuthenticatorCommand : IRequest<ApiResponse<QrCodeDto>>;

public class AddMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaService mfaService
) : IRequestHandler<AddMfaAuthenticatorCommand, ApiResponse<QrCodeDto>>
{
    private const string Issuer = "SafeMedApp";

    public async Task<ApiResponse<QrCodeDto>> Handle(AddMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userClaims = session.GetUserClaims();

        var email = userClaims.Email;
        var id = userClaims.UserId;

        var secretKey = mfaService.GenerateSecretKey();

        var secretKeySaved = await mfaService.AddMfaSecretToUserAsync(id, secretKey, cancellationToken);
        if (!secretKeySaved)
        {
            return new ApiResponse<QrCodeDto>(ApiResponseTypes.Error, "Error while saving secret key");
        }

        var provisionUri = $"otpauth://totp/{Issuer}:{email}?secret={secretKey}&issuer={Issuer}";
        var base64QrCode = GenerateQrCode(provisionUri);

        return new ApiResponse<QrCodeDto>(
            ApiResponseTypes.Success,
            new QrCodeDto(base64QrCode)
        );
    }

    private static string GenerateQrCode(string provisionUri)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(provisionUri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);

        using var qrCodeImage = qrCode.GetGraphic(8);
        using var memoryStream = new MemoryStream();
        qrCodeImage.Save(memoryStream, new PngEncoder());

        return Convert.ToBase64String(memoryStream.ToArray());
    }
}