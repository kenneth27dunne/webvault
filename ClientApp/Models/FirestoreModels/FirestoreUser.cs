using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models.FirestoreModels
{
    [FirestoreData]
    public class FirestoreUser
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string FirstName { get; set; }
        [FirestoreProperty]
        public string LastName { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public string DisplayName { get; set; }
        [FirestoreProperty]
        public bool IsEmailVerified { get; set; }
        [FirestoreProperty]
        public string PhoneNumber { get; set; }
        [FirestoreProperty]
        public string PhotoUrl { get; set; }
        [FirestoreProperty]
        public bool CanContact { get; set; }
        [FirestoreProperty]
        public string Username { get; set; }
    }
}
