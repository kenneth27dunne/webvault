namespace ClientApp.Services.Interfaces
{
    public interface IFirestoreService
    {
        Task<IEnumerable<T>> SearchDocumentsAsync<T>(string collection, string field, string searchTerm)
             where T : new();
        Task<T> GetDocument<T>(string collection, string document);
        Task<bool> WriteDocument<T>(string collection, string document, T data);
    }
}