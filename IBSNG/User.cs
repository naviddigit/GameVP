using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace IBSNG
{

    public class Servers
    {

        // PANAHI
        public static string _ServerIP;
        public static string _AdminUser;
        public static string _AdminPass;


        // NAVID
        //public static string _ServerIP = "185.126.9.115:1500";
        //public static string _AdminUser = "system";
        //public static string _AdminPass = "Aaq123456";

        //public static string _ServerIP = "185.120.221.106:1500";
        //public static string _AdminUser = "system";
        //public static string _AdminPass = "P@nahi1362";

    }

    internal class Users
    {
        public static string? _UserID { get; set; }
        public static string _Username { get; set; }
        public static string _Password { get; set; }
        public static string _GroupName { get; set; }
        public static object? _Expairation { get; set; }
    }

    public class User
    {

        public User(string serverIP, string adminUser, string adminPass)
        {
            Servers._ServerIP = serverIP;
            Servers._AdminUser = adminUser;
            Servers._AdminPass = adminPass;
        }

        public static string _Location { get; set; }
        public static string _UserID { get; set; }
        public static CookieContainer? _Token { get; set; }

        public CookieContainer GetCookieAuthoraition()
        {
            var cookies = new CookieContainer();

            // Request 1 : Login
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/?");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;


            string postDataLogin = "username=" + Servers._AdminUser + "&password=" + Servers._AdminPass + "&x=28&y=15";
            byte[] postBytes = Encoding.Default.GetBytes(postDataLogin);
            request.ContentLength = postBytes.Length;
            using (Stream body = request.GetRequestStream())
            {
                body.Write(postBytes, 0, postBytes.Length);
            }

            WebResponse response = request.GetResponse();
            string referer = response.ResponseUri.AbsoluteUri;

            User._Token = request.CookieContainer;

            return request.CookieContainer;

        }

        private void NewUser(CookieContainer cookies)
        {
            // Request 2 : Create user
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/user/add_new_users.php?");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;


            //var postBytes = Encoding.Default.GetBytes("submit_form=submit_form=1&add=1&count=1&credit=1&owner_name=system&group_name=" + Users._GroupName + "&x=16&y=10&edit__normal_username=normal_username&  edit__voip_username=voip_username");
            var postBytes = Encoding.Default.GetBytes("submit_form=submit_form=1&add=1&count=1&credit=1000&owner_name=system&group_name=" + Users._GroupName + "&x=16&y=10&edit__normal_username=normal_username&  edit__voip_username=voip_username");
            request.ContentLength = postBytes.Length;
            using (Stream body = request.GetRequestStream())
            {
                body.Write(postBytes, 0, postBytes.Length);
            }

            var response = request.GetResponse();
            _Location = response.ResponseUri.ToString();

            var parsedQuery = HttpUtility.ParseQueryString(_Location);
            string userID = parsedQuery["user_id"].ToString().Trim();
            Users._UserID = userID;

        }

        public object? Create(string UserName, string UserPassword, string GroupName, string agent, string comment, string phone, object? Expairation)
        {

            Users._Username = UserName;
            Users._Password = UserPassword;
            Users._GroupName = GroupName;
            Users._Expairation = Expairation.ToString() == "-" ? null : Expairation;
            var cook = GetCookieAuthoraition();

            //if (AnyUser(UserName, cook)) return false;

            NewUser(cook);
            AddInternetAccount(cook);
            AddCommand(comment, agent, phone, cook);
            if (Users._Expairation != null)
            {
                AddExpairetiontAccount(Expairation, cook);
            }

            return Users._UserID;
        }

        public void AddInternetAccount(CookieContainer cookies)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php?");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            string postData = "target=user"
                + "&target_id=" + Users._UserID
                + "&update=1"
                + "&edit_tpl_cs=normal_username"
                + "&attr_update_method_0=normalAttrs"
                + "&has_normal_username=t"
                + "&current_normal_username="
                + "&normal_username=" + Users._Username
                + "&password_character=t"
                + "&password_digit=t"
                + "&password_len=6"
                + "&password=" + Users._Password
                + "&normal_save_user_add=t" // add to list / panahi goft tikesh zade beshe

                + "&x=30"
                + "&y=2";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void AddExpairetiontAccount(object expier, CookieContainer cookies)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php?");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            string postData =
                "target=user"
                + "&target_id=" + Users._UserID
                + "&update=1"
                + "&edit_tpl_cs=abs_exp_date"
                + "&tab1_selected=Exp_Dates"
                + "&attr_update_method_0=absExpDate"
                + "&has_abs_exp=t"
                // + "&abs_exp_date=2020-04-15 01:32"
                //("MM/dd/yyyy hh:mm tt")
                + "&abs_exp_date=" + Convert.ToDateTime(expier).ToString("yyyy/MM/dd hh:mm")
                + "&abs_exp_date_unit=gregorian"
                + "&x=30"
                + "&y=2";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void AddCommand(string comment, string agent, string phone, CookieContainer cookies)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;


            agent = agent == "" ? "" : ("&attr_update_method_1=name" + "&has_name=t" + "&name=" + agent);

            phone = phone == "" || phone == null ? "" : ("&attr_update_method_2=phone" + "&has_phone=t" + "&phone=" + phone);

            string postData =
                "target=user"
                + "&target_id=" + Users._UserID
                + "&update=1"

                + "&edit_tpl_cs=comment,name,phone"
                + "&tab1_selected=Comment"

                + "&attr_update_method_0=comment"
                + "&has_abs_exp=t"
                + "&comment=" + comment
                + agent
                + phone
                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void UpdateAgent(string UserName, string agent)
        {
            Users._Username = UserName;
            var cookies = GetCookieAuthoraition();

            _UserID = GetIdByUsername(UserName, cookies).ToString();
            Users._UserID = _UserID;

            UpdateAgent(agent, cookies);
        }

        protected void UpdateAgent(string agent, CookieContainer cookies = null)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies == null ? GetCookieAuthoraition() : cookies;

            string postData =
                "target=user"
                + "&target_id=" + Users._UserID
                + "&update=1"

                + "&edit_tpl_cs=name"
                + "&tab1_selected=Comment"
                + "&attr_update_method_0=name"
                + "&has_name=t"
                + "&name=" + agent

                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public bool AnyUser(string username, CookieContainer cookies = null)
        {
            if (cookies == null) cookies = GetCookieAuthoraition();

            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/user/user_info.php?normal_username_multi=" + username + "&x=37&y=1");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseText = reader.ReadToEnd();

            if (!responseText.ToString().Contains("does not exists"))
                return true;

            return false;
        }

        public object GetIdByUsername(string username, CookieContainer cookies = null)
        {
            if (cookies == null)
                cookies = GetCookieAuthoraition();

            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/user/user_info.php?normal_username_multi=" + username + "&x=37&y=1");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseText = reader.ReadToEnd();

            string html = responseText;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            if (responseText.ToString().Contains("does not exists"))
                return 0;

            var ok1Node = doc.DocumentNode.SelectNodes("//td[@class='Form_Content_Row_Right_light']").First().InnerHtml.Trim();
            int id = Convert.ToInt32(ok1Node);

            return id;
        }

        public object GetExpairationDate(string username, CookieContainer cookies = null)
        {
            if (cookies == null)
                cookies = GetCookieAuthoraition();

            //http://185.126.9.115:1500/IBSng/admin/user/user_info.php?tab1_selected=Exp_Dates&user_id_multi=62
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/user/user_info.php?normal_username_multi=" + username + "&x=31&y=10");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseText = reader.ReadToEnd();

            string html = responseText;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var ok1Node1 = doc.DocumentNode.SelectSingleNode("//table[@class='Form_Main']").SelectNodes("//td[@class='Form_Content_Row_Right_userinfo_light']");

            var ok1Node = doc.DocumentNode.SelectSingleNode("//table[@class='Form_Main']").SelectNodes("//td[@class='Form_Content_Row_Right_userinfo_light']").Skip(3).First().InnerHtml.Trim();
            DateTime? dateExp = null;
            try
            {
                dateExp = Convert.ToDateTime(ok1Node);
            }
            catch (Exception)
            {

            }

            return dateExp;
        }


        public DateTime? Renew(string Username, string GroupName, string comment, string agent, string phone)
        {
            Users._Username = Username;
            Users._GroupName = GroupName;
            var cookies = GetCookieAuthoraition();

            _UserID = GetIdByUsername(Username, cookies).ToString();

            var dateExp = GetExpairationDate(Username);
            DateTime? _dateExp = dateExp == null ? null : Convert.ToDateTime(dateExp);
            DateTime? _return = null;

            UpadateExpairetion(cookies, GroupName);
            if (_dateExp != null)
            {
                if (_dateExp < DateTime.Now)
                    ResetExpairetion(cookies);
            }
            else
            {
                object dateObj = GroupName.Replace("M", "");
                int months = Convert.ToInt32(dateObj);
                _return = _dateExp?.AddMonths(months);
                UpadateExpairetionToDatetim(cookies, GroupName, _return);
            }

            AddCommand(comment, agent, phone, cookies);

            return _return;
        }

        public void ResetExpairetion(CookieContainer cookies)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            string postData =
                 "target=user"
                + "&target_id=" + User._UserID
                + "&update=1"

                + "&edit_tpl_cs=abs_exp_date,first_login"
                + "&tab1_selected=Exp_Dates"
                + "&attr_update_method_0=absExpDate"
                + "&attr_update_method_1=firstLogin"
                + "&reset_first_login=t"

                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void UpadateExpairetionToDatetim(CookieContainer cookies, string GroupName, DateTime? dateExp)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            string postData =
                 "target=user"
                + "&target_id=" + User._UserID
                + "&update=1"

                + "&edit_tpl_cs: abs_exp_date"
                + "&tab1_selected: Exp_Dates"
                + "&attr_update_method_0: absExpDate"
                + "&has_abs_exp: t"
                + "&abs_exp_date: " + dateExp
                + "&abs_exp_date_unit: gregorian"

                + "&x=21"
                + "&y=3";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }


        public void UpadateExpairetion(CookieContainer cookies, string GroupName)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            string postData =
                 "target=user"
                + "&target_id=" + User._UserID
                + "&update=1"

                + "&edit_tpl_cs=group_name"
                + "&tab1_selected=Main"
                + "&attr_update_method_0=groupName"
                + "&group_name=" + GroupName

                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void UpadateUsernamePassword(CookieContainer cookies, string Username, string Password, string Mi_UserID = null)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;


            User._UserID = Mi_UserID == null ? User._UserID : Mi_UserID;

            string postData =
                 "target=user"
                + "&target_id=" + User._UserID
                + "&update=1"

                + "&edit_tpl_cs=normal_username"
                + "&attr_update_method_0=normalAttrs"
                + "&has_normal_username=t"
                + "&current_normal_username=" + Username
                + "&normal_username=" + Username
                + "&password_character=t"
                + "&password_digit=t"
                + "&password_len=6"
                + "&password=" + Password

                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }

        public void UpadateBanned(CookieContainer cookies, bool Banned, string BannedText, string Mi_UserID = null)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Servers._ServerIP + "/IBSng/admin/plugins/edit.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;

            var textBanned = "";

            if (Banned)
                textBanned =
                  "&has_lock=t"
                + "&lock=" + BannedText;

            User._UserID = Mi_UserID == null ? User._UserID : Mi_UserID;

            string postData =
                 "target=user"
                + "&target_id=" + User._UserID
                + "&update=1"
                + "&edit_tpl_cs=lock"
                + "&attr_update_method_0=lock"

                + textBanned

                + "&x=25"
                + "&y=4";


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string result = reader.ReadToEnd();
                        // استفاده از پاسخ دریافتی
                    }
                }
            }
        }


    }
}
