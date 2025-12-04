namespace RazorPageCampaignsWebsite.Services.Interfaces
{
    public interface IDataService<T> where T : class
    {
        Task<List<T>> GetAllAsync(string? path = null);
        Task<T?> GetByIdAsync(int id, string? path = null);
    }
}
