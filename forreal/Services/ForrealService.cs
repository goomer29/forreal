using System;
using forreal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace forreal.Services
{
    public class ForrealService
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        const string URL = @"https://82vvg3mq-7160.uks1.devtunnels.ms/ForrealApi/";
        public const string WwwRoot = @"https://82vvg3mq-7160.uks1.devtunnels.ms";
        public ForrealService()
        {
            _httpClient = new HttpClient();

            //הגדרות הסיריליזאציה
            //האם להעלם מאותיות גדולות/קטנות
            //האם לייצר json עם רווחים
            //כיצד לטפל במקרה של הפניות מעגליות
            _serializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

        }
        //"Get"
        #region GetHello
        /// <summary>
        /// פעולה קטנה לבדיקת תקשורת מול השרת.
        /// השרת מחזיר מחרוזת של שלום עולם
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetHello()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{URL}Hello");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return "Something is Wrong";
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return "ooops";
        }
        #endregion
        //"Post"
        #region LogInAsync
        /// <summary>
        /// פעולה המבצעת התחברות
        /// הפעולה מקבלת שם משתמש וסיסמה
        /// יוצרת אובייקט יוזר שנשלח לשרת לצורך הזדהות
        /// אם ההזדהות הצליחה הפעולה תחזיר את היוזר עם פרטיו המלאים
        /// אחרת יוחזר אובייקט שגיאה
        /// <param name="userName">שם משתמש</param>
        /// <param name="password">סיסמה</param>
        /// <returns>אובייקט מטיפוס משתמש</returns>
        /// </summary>
        public async Task<UserDto> LogInAsync(string userName, string password)
        {
            try
            {
                //האובייקט לשליחה
                var user = new LoginDto() { UserName = userName, UserPswd = password };
                //מבצעת סיריליזציה
                var jsonContent = JsonSerializer.Serialize(user, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}Login", content);

                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            User u = JsonSerializer.Deserialize<User>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            ((App)(Application.Current)).User = u;
                            return new UserDto() { Success = true, Message = string.Empty, User = u };

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new UserDto() { Success = false, User = null, Message = ErrorMessages.INVALID_LOGIN };

                        }

                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new UserDto() { Success = false, User = null, Message = ErrorMessages.INVALID_LOGIN };

        }
        #endregion
        //"Post"
        #region SignUpAsync
        public async Task<UserDto> SignUpAsync(string userName, string password, string email)
        {
            try
            {
                //האובייקט לשליחה
                var user = new SignUpDto() { UserName = userName, UserPswd = password, Email = email };
                //מבצעת סיריליזציה
                var jsonContent = JsonSerializer.Serialize(user, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}SignUp", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            User u = JsonSerializer.Deserialize<User>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            ((App)(Application.Current)).User = u;
                            return new UserDto() { Success = true, Message = string.Empty, User = u };
                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new UserDto() { Success = false, User = null, Message = ErrorMessages.INVALID_SIGNUP };
                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new UserDto() { Success = false, User = null, Message = ErrorMessages.INVALID_SIGNUP };
        }
        #endregion
        //"Get"
        #region GetChallenge
        public async Task<ChallangesDto> GetChallange()
        {
            try
            {
                var response = await _httpClient.GetAsync($@"{URL}GetChallenge");
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            var jsonContent = await response.Content.ReadAsStringAsync();
                            ObservableCollection<Challange> ch = JsonSerializer.Deserialize<ObservableCollection<Challange>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return new ChallangesDto() { Success = true, ChallangesList = ch, Message = string.Empty };

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new ChallangesDto() { Success = false, ChallangesList = null, Message = ErrorMessages.INVALID_CHALLANGE };

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new ChallangesDto() { Success = false, ChallangesList = null, Message = ErrorMessages.INVALID_CHALLANGE };
        }
        #endregion
        //"Post"
        #region PostChallenge
        public async Task<HttpStatusCode> UploadPost(PostDto post, FileResult file = null)
        {
            try
            {
                var multipartFormContent = new MultipartFormDataContent();
                if (file != null)
                {
                    byte[] bytes;
                    using (MemoryStream ms = new())
                    {
                        var stream = await file.OpenReadAsync();
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    var content = new ByteArrayContent(bytes);
                    multipartFormContent.Add(content, "file", file.FileName);
                }
                var jsonContent = JsonSerializer.Serialize(post, _serializerOptions);
                var stringcontent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                multipartFormContent.Add(stringcontent, "post");

                var response = await _httpClient.PostAsync($"{URL}UploadImage", multipartFormContent);
                return response.StatusCode;
            }
            catch (Exception) { return HttpStatusCode.BadRequest; }
        }
        #endregion
        //"Get"
        #region GetAllUsers
        public async Task<UsersDto> GetAllUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync($@"{URL}GetAllUsers");
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            var jsonContent = await response.Content.ReadAsStringAsync();
                            ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return new UsersDto() { Success = true, UsersList = users, Message = string.Empty };

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new UsersDto() { Success = false, UsersList = null, Message = "there is a problem" };

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new UsersDto() { Success = false, UsersList = null, Message = "was an exception" };
        }
        #endregion
        //"Get"
        #region GetUserNamesWithID
        public async Task<ObservableCollection<UserNameDto>> GetUserNameWithID()
        {
            try
            {
                var response = await _httpClient.GetAsync($@"{URL}GetAllUsersWithID");
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            var jsonContent = await response.Content.ReadAsStringAsync();
                            ObservableCollection<UserNameDto> user_names = JsonSerializer.Deserialize<ObservableCollection<UserNameDto>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return user_names;

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new ObservableCollection<UserNameDto>();

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new ObservableCollection<UserNameDto>();
        }
        #endregion
        //"Get"
        #region GetAllChallanges
        public async Task<ObservableCollection<ChallangeNameDto>> GetAllChallanges()
        {
            try
            {
                var response = await _httpClient.GetAsync($@"{URL}GetAllChallanges");
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            var jsonContent = await response.Content.ReadAsStringAsync();
                            ObservableCollection<ChallangeNameDto> ch_names = JsonSerializer.Deserialize<ObservableCollection<ChallangeNameDto>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return ch_names;

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new ObservableCollection<ChallangeNameDto>();

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new ObservableCollection<ChallangeNameDto>();
        }
        #endregion
        //"Post"
        #region GetUserID
        public async Task<IdDto> GetUserID(string username)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(username, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}GetUserID", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            var id = JsonSerializer.Deserialize<int>(jsonContent, _serializerOptions);
                            return new IdDto() { Success = true, Id=id, Message = string.Empty };
                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new IdDto() { Success = false, Id=-1, Message = "there is a problem" };
                        }
                    default:
                        {
                            // Handle other status codes
                            return new IdDto() { Success = false, Id = -1, Message = "there is a problem" };
                        }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return  new IdDto() { Success = false, Id = -1, Message = "was an exception" };
        }
        #endregion
        //"Post"
        #region GetChallangeID
        public async Task<IdDto> GetChallangeID(string challangename)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(challangename, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}GetChallangeID", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            var id = JsonSerializer.Deserialize<int>(jsonContent, _serializerOptions);
                            return new IdDto() { Success = true, Id = id, Message = string.Empty };
                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new IdDto() { Success = false, Id = -1, Message = "there is a problem" };
                        }
                    default:
                        {
                            // Handle other status codes
                            return new IdDto() { Success = false, Id = -1, Message = "there is a problem" };
                        }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new IdDto() { Success = false, Id = -1, Message = "was an exception" };
        }
        #endregion
        //"Post"
        #region GetChallangeName
        public async Task<string> GetChallangeName(int id)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(id, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}GetChallangeName", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            return jsonContent;
                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return null;
                        }
                    default:
                        {
                            // Handle other status codes
                            return null;
                        }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }
        #endregion
        //"Get"
        #region GetImages
        public async Task<ObservableCollection<string>> GetImages()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{URL}GetImages");
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            var jsonContent = await response.Content.ReadAsStringAsync();
                            ObservableCollection<string> users = JsonSerializer.Deserialize<ObservableCollection<string>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return users;

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new ObservableCollection<string>();
                        }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new ObservableCollection<string>();
        }
            #endregion
        //"Post"
        #region Get Wanted friends
        public async Task<FriendsNameDto> GetWantedFriends(string username) 
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(username, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}GetWantedFriends", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            var users = JsonSerializer.Deserialize<ObservableCollection<string>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return new FriendsNameDto() { Success = true, UsersNameList = users, Message = string.Empty };

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new FriendsNameDto() { Success = false, UsersNameList = new ObservableCollection<string>(), Message = "there is a problem" };

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new FriendsNameDto() { Success = false, UsersNameList = new ObservableCollection<string>(), Message = "was an exception" };
        }
        #endregion
        //"Post"
        #region Get Requested freinds
        public async Task<FriendsNameDto> GetWRequestFriends(string username)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(username, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}GetRequestFriends", content);
                switch (response.StatusCode)
                {
                    case (HttpStatusCode.OK):
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                           var users = JsonSerializer.Deserialize<ObservableCollection<string>>(jsonContent, _serializerOptions);
                            await Task.Delay(2000);
                            return new FriendsNameDto() { Success = true, UsersNameList = users, Message = string.Empty };

                        }
                    case (HttpStatusCode.Unauthorized):
                        {
                            return new FriendsNameDto() { Success = false, UsersNameList = new ObservableCollection<string>(), Message = "there is a problem" };

                        }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return new FriendsNameDto() { Success = false, UsersNameList = new ObservableCollection<string>(), Message = "was an exception" };
        }
        #endregion
        //"Post"
        #region FriendRequest user1-the asker, user 2-the reciver
        public async Task<HttpStatusCode> FriendRequest(string user1, string user2)
        {
            try
            {
                var friend = new FriendDto { username1 = user1, username2 = user2 };
                var jsonContent = JsonSerializer.Serialize(friend, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}FriendRequest", content);
                return response.StatusCode;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return HttpStatusCode.BadRequest;
        
        }
        #endregion
        //"Post"
        #region Delete friend request
        public async Task<HttpStatusCode> EnemyRequest(string user1, string user2)
        {
            try
            {
                var friend = new FriendDto { username1 = user1, username2 = user2 };
                var jsonContent = JsonSerializer.Serialize(friend, _serializerOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{URL}EnemyRequest", content);
                return response.StatusCode;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return HttpStatusCode.BadRequest;
        }
        #endregion
    }
}