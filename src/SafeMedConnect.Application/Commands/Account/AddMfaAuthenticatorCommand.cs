using MediatR;
using QRCoder;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using SixLabors.ImageSharp.Formats.Png;

namespace SafeMedConnect.Application.Commands.Account;

public record AddMfaAuthenticatorCommand : IRequest<ResponseWrapper<QrCodeDto>>;

public class AddMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaService mfaService
) : IRequestHandler<AddMfaAuthenticatorCommand, ResponseWrapper<QrCodeDto>>
{
    private const string Issuer = "SafeMedApp";

    public async Task<ResponseWrapper<QrCodeDto>> Handle(AddMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userClaims = session.GetUserClaims();

        var email = userClaims.Email;
        var id = userClaims.UserId;

        var secretKey = mfaService.GenerateSecretKey();

        var secretKeySaved = await mfaService.AddMfaSecretToUserAsync(id, secretKey, cancellationToken);
        if (!secretKeySaved)
        {
            return new ResponseWrapper<QrCodeDto>(ResponseTypes.Error, "Error while saving secret key");
        }

        var provisionUri = $"otpauth://totp/{Issuer}:{email}?secret={secretKey}&issuer={Issuer}";
        var base64QrCode = GenerateQrCode(provisionUri);

        return new ResponseWrapper<QrCodeDto>(
            ResponseTypes.Success,
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