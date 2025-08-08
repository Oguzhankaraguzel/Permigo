using SharedKernel.Concrete;

namespace Domain.Entities.User;
public static class UserError
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id '{userId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByUserName = Error.NotFound(
        "Users.NotFoundByUserName",
        "The user with the specified username was not found.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "This email address is already in use. Please try a different one.");

    public static readonly Error InvalidCredentials = Error.Failure(
        "Users.InvalidCredentials",
        "The username or password you entered is incorrect. Please try again.");
}
