// Copyright © 2016 Dimasin. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace fermtools
{
    class TelegramBot
    {
        public string token;
        public int lastUpd;
        public string chatID;
        public bool bInit;
        public ReplyKeyboardMarkup kbmu;
        public KeyboardButton kb1;
        public KeyboardButton kb2;
        public KeyboardButton kb3;

        public TelegramBot(string tk = "", string id = "")
        {
            token = tk;
            lastUpd = 0;
            chatID = id;
            bInit = false;
            kb1 = new KeyboardButton(); kb1.Text = "one";
            kb2 = new KeyboardButton(); kb2.Text = "two";
            kb3 = new KeyboardButton(); kb3.Text = "tre";
            kbmu = new ReplyKeyboardMarkup();
            kbmu.Keyboard = new KeyboardButton[1,3];
            kbmu.Keyboard[0,0] = kb1;
            kbmu.Keyboard[0,1] = kb2;
            kbmu.Keyboard[0,2] = kb3;
        }
        public List<Update> GetUpdates(string offset = "", string limit = "", string timeout = "")
        {
            var coll = new NameValueCollection
            {
                {"offset", offset},
                {"limit", limit},
                {"timeout", timeout}
            };
            try
            {
                var update = JsonConvert.DeserializeObject<RootObject<List<Update>>>(RequestCore.Get("getUpdates", token, coll).Result).Result;
                return update;
            }
            catch { }
            return null;
        }
        public User GetMe()
        {
            try
            {
                var user = JsonConvert.DeserializeObject<RootObject<User>>(RequestCore.Get("getMe", token, new NameValueCollection()).Result).Result;
                return user;
            }
            catch {}
            return null;
        }
        public void SendMessage(string chatId, string text, string parseMode = "", string disableWebPagePreview = "", string replyToMessageId = "", string replyMarkup = "")
        {
            var coll = new NameValueCollection
            {
                {"chat_id", chatId},
                {"text", text},
                {"parse_mode", parseMode},
                {"disable_web_page_preview", disableWebPagePreview},
                {"reply_to_message_id", replyToMessageId},
                {"reply_markup", replyMarkup}
            };
            RequestCore.Get("sendMessage", token, coll).Wait();
        }
        public void MsgQueueClear(int id) //Очистка очереди бота
        {
            try { id++; }
            catch { id = Int32.MaxValue; }
            //Очищаем очередь на сервере
            GetUpdates(id.ToString());
        }
    }

    public class RequestCore
    {
        public static async Task<string> Get(string method, string token, NameValueCollection args)
        {
            string url = "https://api.telegram.org/bot" + token + "/" + method + ToQueryString(args);
            var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseContent;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }

        private static string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value)).ToArray();
            if (array.Any())
                return "?" + string.Join("&", array);
            return "";
        }
    }
    
    public class RootObject<T>
    {
        [JsonProperty(PropertyName = "ok")]
        public bool Ok { get; set; }

        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }
    public class Update
    {
        [JsonProperty(PropertyName = "update_id")]
        public int UpdateId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }
    }
    public class Message
    {
        [JsonProperty(PropertyName = "message_id")]
        public int MessageId { get; set; }

        [JsonProperty(PropertyName = "from")]
        public User From { get; set; }

        [JsonProperty(PropertyName = "date")]
        public int Date { get; set; }

        [JsonProperty(PropertyName = "chat")]
        public Chat Chat { get; set; }

        [JsonProperty(PropertyName = "forward_from")]
        public User ForwardFrom { get; set; }

        [JsonProperty(PropertyName = "forward_date")]
        public int ForwardDate { get; set; }

        [JsonProperty(PropertyName = "reply_to_message")]
        public Message ReplyToMessage { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "audio")]
        public Audio Audio { get; set; }

        [JsonProperty(PropertyName = "document")]
        public Document Document { get; set; }

        [JsonProperty(PropertyName = "photo")]
        public List<PhotoSize> Photo { get; set; }

        [JsonProperty(PropertyName = "sticker")]
        public Sticker Sticker { get; set; }

        [JsonProperty(PropertyName = "video")]
        public Video Video { get; set; }

        [JsonProperty(PropertyName = "voice")]
        public Voice Voice { get; set; }

        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }

        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "new_chat_participant")]
        public User NewChatParticipant { get; set; }

        [JsonProperty(PropertyName = "left_chat_participant")]
        public User LeftChatParticipant { get; set; }

        [JsonProperty(PropertyName = "new_chat_title")]
        public string NewChatTitle { get; set; }

        [JsonProperty(PropertyName = "new_chat_photo")]
        public List<PhotoSize> NewChatPhoto { get; set; }

        [JsonProperty(PropertyName = "delete_chat_photo")]
        public bool DeleteChatPhoto { get; set; }

        [JsonProperty(PropertyName = "group_chat_created")]
        public bool GroupChatCreated { get; set; }

        [JsonProperty(PropertyName = "supergroup_chat_created")]
        public bool SuperGroupChatCreated { get; set; }

        [JsonProperty(PropertyName = "channel_chat_created")]
        public bool ChangelChatCreated { get; set; }

        /* Since supergroups were added into Telegram, the migrate_to_chat_id values */
        /* from moving to supergroups no longer fit inside of an Int32 container. */
        [JsonProperty(PropertyName = "migrate_to_chat_id")]
        public long MigrateToChatId { get; set; }

        [JsonProperty(PropertyName = "migrate_from_chat_id")]
        public long MigrateFromChatId { get; set; }
    }
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
    public class Chat
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    }
    public class Audio
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "performer")]
        public string Performer { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class Document
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public PhotoSize Thumb { get; set; }

        [JsonProperty(PropertyName = "file_name")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class PhotoSize
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class Sticker
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public PhotoSize Thumb { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class Video
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public PhotoSize Thumb { get; set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class Voice
    {
        [JsonProperty(PropertyName = "file_id")]
        public string FileId { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public int FileSize { get; set; }
    }
    public class Contact
    {
        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
    }
    public class Location
    {
        [JsonProperty(PropertyName = "longitude")]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public float Latitude { get; set; }
    }
    public class KeyboardButton
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "request_contact")]
        public bool RequestContact { get; set; }

        [JsonProperty(PropertyName = "request_location")]
        public bool RequestLocation { get; set; }
    }
    public class ReplyKeyboardHide 
    {
        [JsonProperty(PropertyName = "hide_keyboard")]
        public bool HideKeyboard { get; set; }

        [JsonProperty(PropertyName = "selective")]
        public bool Selective { get; set; }
    }
    public class ReplyKeyboardMarkup
    {
        [JsonProperty(PropertyName = "keyboard")]
        public KeyboardButton[,] Keyboard { get; set; }

        [JsonProperty(PropertyName = "resize_keyboard")]
        public bool ResizeKeyboard { get; set; }

        [JsonProperty(PropertyName = "one_time_keyboard")]
        public bool OneTimeKeyboard { get; set; }

        [JsonProperty(PropertyName = "selective")]
        public bool Selective { get; set; }
    }
}
