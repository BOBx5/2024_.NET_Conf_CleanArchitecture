namespace LibrarySolution.Infrastructure.Persistence;
internal sealed class DbTables
{
    public static readonly DbTable Book = new("Book");
    public static readonly DbTable User = new("User");
    public static readonly DbTable Rent = new("Rent");
}
internal sealed record DbTable(string Name, string Schema = "dbo");

