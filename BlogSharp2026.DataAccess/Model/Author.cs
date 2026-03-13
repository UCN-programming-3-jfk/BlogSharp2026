namespace BlogSharp2026.DataAccess.Model;

public class Author
{
    public int Id { get; set; }
    public string BlogTitle { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public override string ToString()
    {
        return $"Author {{ Id = {Id}, BlogTitle = '{BlogTitle}', Email = '{Email}', PasswordHash = '{PasswordHash}' }}";
    }
}