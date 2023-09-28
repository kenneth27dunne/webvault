using ClientApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    class FirestoreManager : IFirestoreManager
    {
        private IFirestoreService _firestore;
        public FirestoreManager(IFirestoreService firestore)
        {
            _firestore = firestore;
        }

        public async Task SearchUsers(string term)
        {
            //await _firestore.SearchUsersAsync(term);
        }
    }
}
