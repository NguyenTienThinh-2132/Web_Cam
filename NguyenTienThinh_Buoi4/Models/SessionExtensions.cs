using System.Text.Json;

namespace CamZone.Models
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Set giá trị vào key nằm trong session
        /// </summary>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Lấy về giá trị của khóa nằm trong session
        /// </summary>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
