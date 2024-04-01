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
        public async Task<HttpStatusCode> UploadPost(PostDto post,FileResult file = null)
        {
            try
            {
                var multipartFormContent = new MultipartFormDataContent();
                if(file != null)
                {
                    byte[] bytes;
                    using(MemoryStream ms = new()) 
                    {
                        var stream = await file.OpenReadAsync();
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    var content=new ByteArrayContent(bytes);
                    multipartFormContent.Add(content, "file",file.FileName);
                }
                var jsonContent = JsonSerializer.Serialize(post, _serializerOptions);
                var stringcontent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                multipartFormContent.Add(stringcontent, "post");

                var response = await _httpClient.PostAsync($"{URL}UploadImage",multipartFormContent);
                return response.StatusCode;  
            }
            catch (Exception) { return HttpStatusCode.BadRequest; }
        }
        #endregion
        
    }
}