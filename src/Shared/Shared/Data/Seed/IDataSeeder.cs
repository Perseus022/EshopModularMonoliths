namespace Shared.Data.Seed;

public interface IDataSeeder
{
    /// <summary>
    /// Seed all data asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SeedAllAsync();

}
