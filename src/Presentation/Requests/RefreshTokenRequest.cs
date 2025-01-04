namespace Presentation.Requests;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);
