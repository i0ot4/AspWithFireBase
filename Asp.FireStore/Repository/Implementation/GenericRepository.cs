using Asp.FireStore.Repository.IRepository;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using System.Linq.Expressions;

namespace Asp.FireStore.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T>
    {
        public FirestoreDb _db;
        private static string ApiKey = "AIzaSyBEyD0Y6E-guN-dFVc2EorSprGSlGMq0Ns";
        private static string Bucket = "malik-project-419023.web.app";

        //الاتصال بالفايرستور داتابيز
        public GenericRepository()
        {
            var filepath = @"C:\Users\Owner\source\repos\Maylzam_Projects\Asp.FireStore\Asp.FireStore\malik-project.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            _db = FirestoreDb.Create("malik-project-419023");

        }
        //الاتصال بقاعده بيانات Autimtacation
        FirebaseAuthClient authClient = new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = ApiKey,
            AuthDomain = Bucket,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        });

        public async Task<string> SignUp(string Email, string Password)
        {
            var result = await authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);
            return result.User.Uid;
        }


        public async Task<List<T>> GetAllAsync(string path)
        {
            Query query = _db.Collection(path);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            var list = new List<T>();

            foreach (var document in snapshot.Documents)
            {
                list.Add(document.ConvertTo<T>());
            }

            return list;
        }
        public async Task<List<T>> GetAllAsync(string path, Expression<Func<T, bool>> expression)
        {
            Query query = _db.Collection(path);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            var list = new List<T>();

            foreach (var document in snapshot.Documents)
            {
                list.Add(document.ConvertTo<T>());
            }

            // تطبيق التعبير على القائمة
            var queryResult = list.AsQueryable().Where(expression);

            return queryResult.ToList();
        }

        public async Task<int> Count(string path)
        {
            Query query = _db.Collection(path);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            return snapshot.Count;
        }

        public async Task<int> Count(string path, Expression<Func<T, bool>> expression)
        {
            Query query = _db.Collection(path);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            var list = new List<T>();

            foreach (var document in snapshot.Documents)
            {
                list.Add(document.ConvertTo<T>());
            }

            // تطبيق التعبير على القائمة
            var queryResult = list.AsQueryable().Where(expression);

            return queryResult.Count();
        }



        public async Task<T> GetById(string path, string id)
        {
            DocumentReference docRef = _db.Collection(path).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }
            else
            {
                return default(T);
            }
        }

        public async Task<T> GetById(string path, string id, Expression<Func<T, bool>> expression)
        {
            Query query = _db.Collection(path).WhereEqualTo("id", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    T obj = documentSnapshot.ConvertTo<T>();
                    // Apply the additional query
                    if (expression.Compile()(obj))
                    {
                        return obj;
                    }
                }
            }
            return default(T);
        }


        public async Task<string> UploadImageAsync(string path, IFormFile imageFile)
        {
            // تحويل الصورة إلى Stream
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // تهيئة العميل
            var storageClient = StorageClient.Create();

            // إنشاء اسم فريد للصورة
            string imageName = Guid.NewGuid().ToString() + ".png";

            // رفع الصورة
            await storageClient.UploadObjectAsync("malik-project-419023.appspot.com", path + "/" + imageName, null, memoryStream);

            // إرجاع رابط الصورة
            return "https://firebasestorage.googleapis.com/v0/b/malik-project-419023.appspot.com/o/" + Uri.EscapeDataString(path + "/" + imageName) + "?alt=media";
        }


        public async Task AddAsyncWithSignUp(string path, T item, string id)
        {
            CollectionReference colRef = _db.Collection(path);
            DocumentReference docRef = colRef.Document(id);
            await docRef.SetAsync(item);
        }


        public async Task AddAsync(string path, T item)
        {
            CollectionReference colRef = _db.Collection(path);
            DocumentReference docRef = await colRef.AddAsync(item);
        }


        public async Task Update(string path, string id, T item)
        {
            DocumentReference docRef = _db.Collection(path).Document(id);
            await docRef.SetAsync(item, SetOptions.Overwrite);
        }


    }
}
