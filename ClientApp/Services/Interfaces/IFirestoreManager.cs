namespace ClientApp.Services.Interfaces
{
    internal interface IFirestoreManager
    {
        Task SearchUsers(string term);
    }
}