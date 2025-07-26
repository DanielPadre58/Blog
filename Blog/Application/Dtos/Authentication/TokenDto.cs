namespace Blog.Application.Dtos.Authentication;

public record TokenDto(string username ,string Token, string RefreshToken);