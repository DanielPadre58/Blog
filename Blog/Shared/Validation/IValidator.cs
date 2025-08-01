namespace Blog.Shared.Validation;

public interface IValidator
{
    public void NotNull(object value, string name);
    public void NotNullOrEmpty(string value, string name);
    public void ValidId(int id, string name);
    public void ValidPassword(string password, string name);
    public void ValidEmail(string email, string name);
    public void BeforeToday(DateTime date, string name);
    public void OlderThan(DateTime date, int years, string name);
}