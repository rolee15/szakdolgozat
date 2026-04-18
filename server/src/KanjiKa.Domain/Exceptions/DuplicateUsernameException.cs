namespace KanjiKa.Domain.Exceptions;

public class DuplicateUsernameException : Exception
{
    public DuplicateUsernameException() : base("Username already exists.") { }
}
