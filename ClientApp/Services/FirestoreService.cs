using ClientApp.Models;
using ClientApp.Services.Interfaces;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace ClientApp.Services
{
    public class FirestoreService : IFirestoreService
    {
        private FirestoreDb _firestoreDb;

        public FirestoreService(string projectId)
        {
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<IEnumerable<T>> SearchDocumentsAsync<T>(string collection, string field, string searchTerm)
             where T : new()
        {
            var usersRef = _firestoreDb.Collection(collection);
            var query = usersRef.WhereGreaterThanOrEqualTo(field, searchTerm)
                                .WhereLessThan(field, searchTerm + "\uf8ff");
            var querySnapshot = await query.GetSnapshotAsync();

            return GetData<T>(querySnapshot);
        }

        private List<T> GetData<T>(QuerySnapshot snapshot) 
            where T : new()
        {
            List<T> dataList = new List<T>();
            foreach (var doc in snapshot.Documents)
            {
                T data = new T();
                Dictionary<string, object> docData = doc.ToDictionary();
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (docData.ContainsKey(property.Name))
                    {
                        property.SetValue(data, docData[property.Name]);
                    }
                }
                dataList.Add(data);
            }
            return dataList;
        }        

        public async Task<T> GetDocument<T>(string collection, string document)
        {
            var doc = await _firestoreDb
                .Collection(collection)
                .Document(document)
                .GetSnapshotAsync();

            return doc.ConvertTo<T>();
        }

        public async Task<bool> WriteDocument<T>(string collection, string document, T data)
        {
            var doc = _firestoreDb
                .Collection(collection)
                .Document(document);

            await doc.CreateAsync(data);
            return true;
        }
    }

    public static class FirestoreServiceExtensions
    {
        public static IServiceCollection AddFirestore(this IServiceCollection services, string projectId)
        {
            services.AddScoped<IFirestoreService>(_ => new FirestoreService(projectId));
            return services;
        }
    }
}
