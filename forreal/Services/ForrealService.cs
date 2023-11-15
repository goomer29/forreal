using System;
using forreal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace forreal.Services
{
    public class ForrealService
    { 
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        const string URL = @"";
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
                User user = new User() { Email = userName, Password = password };
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
    }
}
