using Blog.Shared.Exceptions;
using Blog.Shared.Validation;

namespace Blog.Application.Dtos.Users;

public record UserUpdateDto(string? FirstName, string? LastName, DateTime? Birthday);