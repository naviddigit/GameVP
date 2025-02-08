using DataService.Entity;
using ProjectLayer.Models.Enums;
using ProjectLayer.Models.Structure;
using SoftEther.VPNServerRpc;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using ProjectLayer.Models.Stucture;
using System.Diagnostics;


namespace ProjectLayer.Models.Mod
{
    public class MUser
    {
        private Repository.RUser rUser = new Repository.RUser();

        public MResponce CreateUser(string username, string password, string phoneNumber, int productUserId, string IssuedAccount_token)
        {
            // get Issued account
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            //
            //var Ammount = 100000;
            var productManager = new MProductUser().GetProductManager(IssuedAccount_token, productUserId);


            // check balance
            if (!new MAccount().financeAllow(GetIssuedAccount.Id, productManager.price))
                return new MResponce() { Message = "Low Balance.", Success = false };

            if (!rUser.Any1(username))
            {
                List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
                foreach (var server in _server)
                {
                    // check user server Limite
                    //if (rUser.Count(null) >= 100) continue;

                    // get server info
                    var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.Ip, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                    // count user:
                    //var getUser = ApiVPNSERVER.AnyUser(username, server.HubName);
                    var getUser = (bool)ApiVPNSERVER.AnyUser_V1(username, server.HubName); // New V

                    if (!getUser)
                    {
                        #region Create User To Server
                        var v_newUserItem = new VpnRpcSetUser()
                        {
                            Name_str = username,
                            Realname_utf = GetIssuedAccount?.Username,
                            Auth_Password_str = password,
                            HubName_str = server.HubName,
                            ExpireTime_dt = DateTime.Now.AddDays(30),
                            Note_utf = "1 Month",
                            GroupName_str = "1 User - (2M)",
                            Auth_NT_NTUsername = "",

                            AuthType_u32 = VpnRpcUserAuthType.Password,
                            Auth_UserCert_CertData = new byte[0] { },
                            Auth_RootCert_Serial = new byte[0] { },
                        };
                        ApiVPNSERVER.CreateUser_WithoutPolicy(v_newUserItem);
                        #endregion

                    }
                    //else return new MResponce() { Message = "Exist Username To SOFTWARE.", Success = false };
                }

                #region Crate User To Database
                User newUserItem = new User()
                {
                    Username = username,
                    Password = password,
                    PhoneNumber = phoneNumber,
                    ExpirationDate = DateTime.Now.AddDays(30),
                    ServerId = 1,//server.Id,
                    CreateDate = DateTime.Now,
                    AccountId = GetIssuedAccount?.Id,
                };
                rUser.Insert(newUserItem);
                #endregion

                // Finance:
                new MTransaction().FinanceAction(GetIssuedAccount, productManager);

                return new MResponce() { Message = "OK.", Success = true, Data = newUserItem };



                //return new MResponce() { Message = "Cannot Find Server.", Success = false };
            }
            else return new MResponce() { Message = "Exist Username.", Success = false };
        }

        public MResponce IBSNG_CreateUser(string username, string password, string phoneNumber, string description, int productUserId, string IssuedAccount_token)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            var productManager = new MProductUser().GetProductManager(IssuedAccount_token, productUserId);

            // check balance
            if (!new MAccount().financeAllow(GetIssuedAccount.Id, productManager.price))
                return new MResponce() { Message = "Low Balance.", Success = false };

            username = username.ToLower();
            // check has user
            if (new Repository.RUser().Any1(username))
                return new MResponce() { Message = "Exist Username.", Success = false };


            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);

            Server sv = _server.First(i => i.Active == true);

            if (!new IBSNG.User(sv.Ip + ":" + sv.VpnserverPort, sv.HubName, sv.AdminPassword).AnyUser(username))
            {

                // OLD ARVAN
                //CreateUserOnly(username, password, GetIssuedAccount, "185.231.183.1");

                foreach (var server in _server)
                {
                    new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Create(username, password, productManager.Group_IBSng, GetIssuedAccount.Username, phoneNumber, "New", "-");
                }


                #region Crate User To Database
                User newUserItem = new User()
                {
                    Username = username,
                    Password = password,
                    PhoneNumber = phoneNumber,
                    ExpirationDate = null,
                    ServerId = 1,//server.Id,
                    CreateDate = DateTime.Now,
                    AccountId = GetIssuedAccount?.Id,
                    ProductId = productManager.id,
                    Description = description,
                };
                rUser.Insert(newUserItem);
                #endregion

                // Finance:
                new MTransaction().FinanceAction(GetIssuedAccount, productManager);

                return new MResponce() { Message = "OK.", Success = true, Data = newUserItem };
            }
            else return new MResponce() { Message = "Exist Username.", Success = false };
        }

        public MResponce IBSNG_CreateUserTest(string username, string password, string IssuedAccount_token, string group, string comment)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            if (!new IBSNG.User("", "", "").AnyUser(username))  // باید درست شود آدرس ها
            {
                List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
                foreach (var server in _server)
                {
                    IBSNG.Servers._ServerIP = server.Ip + ":" + server.VpnserverPort;
                    if (server.VpnserverPort == 80)
                        IBSNG.Servers._ServerIP = server.Ip;

                    IBSNG.Servers._AdminUser = server.HubName;
                    IBSNG.Servers._AdminPass = server.AdminPassword;

                    object userId_IBSng = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Create(username, password, group, GetIssuedAccount.Username, comment, "", "-");
                }

                return new MResponce() { Message = "OK.", Success = true };
            }
            else return new MResponce() { Message = "Exist Username.", Success = false };

        }

        public object IBSNG_GetIdByUsername(string username)
        {
            return new IBSNG.User("", "", "").GetIdByUsername(username);
        }

        public MResponce IBSNG_IMPORT_CreateUser(string token)
        {
            Account? Account = (Account?)new Repository.RAccount().GetByToken(token);

            if (Account.Id > 1)
                return new MResponce { Success = false, Data = EMessage.Message.AssesDinaid };

            List<User?> userList = (List<User?>)rUser.List(0);


            foreach (var item in userList)
            {
                if (!new IBSNG.User("", "", "").AnyUser(item.Username))
                {
                    new IBSNG.User("", "", "").Create(item.Username, item.Password, "1M", item.Account.Username, item.RenewDate == null ? "New" : "Renew", "", item.ExpirationDate);
                    Console.WriteLine("Success: " + item.Username);
                }
                else Console.WriteLine("Exist: " + item.Username);
            }

            return new MResponce() { Message = "OK.", Success = true };
        }

        // temp :
        public MResponce CreateUserOnly(string username, string password, Account GetIssuedAccount, string ip)
        {
            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);

            foreach (var server in _server)
            {
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(ip == null ? server.Ip : ip, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                // count user:
                var getUser = (bool)ApiVPNSERVER.AnyUser_V1(username, server.HubName);

                if (!getUser)
                {

                    #region Create User To Server
                    var v_newUserItem = new VpnRpcSetUser()
                    {
                        Name_str = username,
                        Realname_utf = GetIssuedAccount?.Username,
                        Auth_Password_str = password,
                        HubName_str = server.HubName,
                        ExpireTime_dt = DateTime.Now.AddDays(30),
                        Note_utf = "1 Month",
                        GroupName_str = "1 User - (2M)",
                        Auth_NT_NTUsername = "",

                        AuthType_u32 = VpnRpcUserAuthType.Password,
                        Auth_UserCert_CertData = new byte[0] { },
                        Auth_RootCert_Serial = new byte[0] { },
                    };
                    ApiVPNSERVER.CreateUser_WithoutPolicy(v_newUserItem);
                    #endregion
                    //return new MResponce() { Message = "OK.", Success = true, Data = v_newUserItem };

                }
                else return new MResponce() { Message = "Exist Username.", Success = false };


            }

            //return new MResponce() { Message = "Cannot Find Server.", Success = false };
            return new MResponce() { Message = "OK.", Success = true };

        }

        public MResponce CreateUserTest(string username, string ip)
        {
            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);

            foreach (var server in _server)
            {
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(ip == null ? server.Ip : ip, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                // count user:
                var getUser = (bool)ApiVPNSERVER.AnyUser_V1(username, server.HubName);

                if (!getUser)
                {

                    #region Create User To Server
                    var v_newUserItem = new VpnRpcSetUser()
                    {
                        Name_str = username,
                        Realname_utf = "TEST",
                        Auth_Password_str = "111111",
                        HubName_str = server.HubName,
                        ExpireTime_dt = DateTime.Now.AddDays(30),
                        Note_utf = "TEST",
                        GroupName_str = "1 User - (2M)",
                        Auth_NT_NTUsername = "",

                        AuthType_u32 = VpnRpcUserAuthType.Password,
                        Auth_UserCert_CertData = new byte[0] { },
                        Auth_RootCert_Serial = new byte[0] { },
                    };
                    ApiVPNSERVER.CreateUser_WithoutPolicy(v_newUserItem);
                    #endregion
                    //return new MResponce() { Message = "OK.", Success = true, Data = v_newUserItem };

                }
                else return new MResponce() { Message = "Exist Username.", Success = false };


            }

            //return new MResponce() { Message = "Cannot Find Server.", Success = false };
            return new MResponce() { Message = "OK.", Success = true };

        }

        public object AnyUserA(string username, string ip)
        {
            try
            {
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(ip, 443, "Aaq123456", "VS");

                object? anyUser = null;
                try
                {
                    anyUser = ApiVPNSERVER.AnyUser_V1(username, "VS");
                    return (bool)anyUser;
                }
                catch (Exception)
                {
                    return anyUser;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }



        }

        public object? List(string token)
        {
            var user = (Account?)new Repository.RAccount().GetByToken(token);

            var list = (List<User>?)rUser.List(user.RoleId > 2 ? user?.Id : 0);

            var product = new Repository.RProductUser();

            return list.Select(i => new
            {
                i.Id,
                i.Username,
                avatarUrl = "https://api-prod-minimal-v4.vercel.app/assets/images/avatars/avatar_" + new Random().Next(1, 24) + ".jpg",
                agent = i.Account?.Username,
                server = i.Server.Ip,
                ExpirationDate = i.ExpirationDate == null ? "none" : i.ExpirationDate < DateTime.Now ? "Expir (" + (i.ExpirationDate - DateTime.Now)?.Days + ")" : Publics.DateFormatShort2(i.ExpirationDate).ToString() + " (+" + (i.ExpirationDate - DateTime.Now)?.Days + ")",
                CreateDate = Publics.DateFormatShortTiming(i.CreateDate),
                productId = i.ProductId == 4 ? 0 : product?.GetProductUserId(i.ProductId, user.Id), // +++ === >>> productUserId <<<
                productName = i.ProductId == 4 ? "No" : product?.GetProductNameUserId(i.ProductId, user.Id), // +++ === >>> productUserId <<<
                renew = i.RenewDate,
                renewActive = i.ExpirationDate < DateTime.Now.AddDays(5) ? true : false,
                status = i.ExpirationDate < DateTime.Now ? "Expair" : i.ExpirationDate < DateTime.Now.AddDays(5) ? ((bool)i.Banned == true ? "Banned" : "Active Renew") : ((bool)i.Banned == true ? "Banned" : "Renew"),
                statusRenew = i.RenewDate == null ? "New" : "Renewed",
                dateDesc = i.RenewDate == null ? i.CreateDate : i.RenewDate.Value,
                Banned = i.Banned
                //LastLogin =  SoftEther.Models.VPNRPCT
            }).OrderByDescending(i => i.dateDesc);
        }

        public object? Get(string token, string username)
        {
            var accountId = MAccount.GetAccountIdByRole(token);

            var getUser = (User?)rUser.Get(accountId, username);
            return new
            {
                accountId = getUser?.AccountId,
                username = getUser?.Username,
                password = getUser?.Password,
                phoneNumber = getUser?.PhoneNumber,
                expirationDate = getUser?.ExpirationDate,
                description = getUser?.Description,
                status = (bool)getUser.Banned == false ? "active" : "banned",
                statusText = getUser.BannedText == null ? "" : getUser.BannedText,
            };
        }

        public MResponce? Update(string token, string username, string password, string phoneNumber, string description, int accountChangeId = 0)
        {
            var accountId = MAccount.GetAccountIdByRole(token);

            if (!new Repository.RUser().Any2(accountId, username))
                return new MResponce { Success = false, Data = "Invalid Access" };


            // New Update PN
            if (accountChangeId != 0)
            {
                var accountChild = (Account?)new Repository.RAccount().GetById(accountChangeId);
                if (accountChild.ParentId != accountChangeId && accountId > 2)
                    return new MResponce { Success = false, Data = "Invalid Access Child Account" };

                accountId = accountChangeId;
            }

            var updateItem = new User
            {
                Username = username,
                Password = password,
                PhoneNumber = phoneNumber,
                AccountId = accountId,
                Description = description,
            };

            rUser.Update(updateItem);

            return UpdateServer(username);
        }

        public MResponce? Renew(string IssuedAccount_token, string username, int productUserId)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            ///////////////// low balance
            ///

            if (!new Repository.RUser().Any2(GetIssuedAccount.Id, username))
                return new MResponce { Success = false, Message = "Invalid Access" };

            var getUser = (User)rUser.Get(username);
            if (getUser.ExpirationDate < DateTime.Now.AddDays(-4))
                return new MResponce { Success = false, Message = "Renew Affter 5 Day " + Publics.DateFormatShort2(getUser.ExpirationDate) };


            var updateItem = new User
            {
                Username = username,
                ExpirationDate = DateTime.Now.AddMonths(1),
                RenewDate = DateTime.Now,
            };

            rUser.UpdateRenew(updateItem);

            //////////////////////////////////////// new IBSNG.User().AddExpairetiontAccount();

            var productManager = new MProductUser().GetProductManager(IssuedAccount_token, productUserId);

            // Finance:
            new MTransaction().FinanceAction(GetIssuedAccount, productManager, "Renew " + username + ".");

            return UpdateServer(username, " / Renew");
        }

        public MResponce? IBSNG_Renew(string IssuedAccount_token, string username, int productUserId, string? serverIp)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);
            var productManager = new MProductUser().GetProductManager(IssuedAccount_token, productUserId);

            if (!new MAccount().financeAllow(GetIssuedAccount.Id, productManager.price))
                return new MResponce() { Message = "Low Balance.", Success = false };

            if (!new Repository.RUser().Any2(GetIssuedAccount.Id, username))
                return new MResponce { Success = false, Message = "Invalid Access" };

            // Finance:
            new MTransaction().FinanceAction(GetIssuedAccount, productManager, "Renew " + username + ".");

            DateTime? ExpirationDate = null;


            //List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            //foreach (var server in _server)
            //{
            //    // IBSNG
            //    ExpirationDate = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Renew(username, productManager.Group_IBSng, "Renew", GetIssuedAccount.Username, "");
            //}
            string tracert = "1";
            try
            {

                Server? server = (Server?)new Repository.RServer()?.Get(serverIp);
                tracert += "2";
                ExpirationDate = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Renew(username, productManager.Group_IBSng, "Renew", GetIssuedAccount.Username, "");
                tracert += "3";
                tracert += ExpirationDate;
                var updateItem = new User
                {
                    Username = username,
                    ServerId = server.Id,
                    //ExpirationDate = DateTime.Now.AddMonths(1),
                    ProductId = productManager.id,
                    ExpirationDate = ExpirationDate == null ? null : ExpirationDate,
                    RenewDate = DateTime.Now,
                    Updated = DateTime.Now,
                };
                rUser.UpdateRenew(updateItem);
                tracert += "4";
            }
            catch (Exception ex)
            {
                return new MResponce() { Success = false, Message = "Er." + ex.Message + " -> " + tracert };
            }

            return new MResponce() { Success = true };
            // SOFTETHER TEST TEMP
            //return UpdateServer(username, " / Renew");

        }

        public MResponce? IBSNG_Renew_Only(string IssuedAccount_token, string username, int productUserId, string? serverIp)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);
            var productManager = new MProductUser().GetProductManager(IssuedAccount_token, productUserId);


            if (!new MAccount().financeAllow(GetIssuedAccount.Id, productManager.price))
                return new MResponce() { Message = "Low Balance.", Success = false };


            if (!new Repository.RUser().Any2(GetIssuedAccount.Id, username))
                return new MResponce { Success = false, Message = "Invalid Access" };


            if (GetIssuedAccount.Id > 2)
                return new MResponce { Success = false, Message = "Access Denied!" };

            DateTime? ExpirationDate = null;

            Server? server = (Server?)new Repository.RServer()?.Get(serverIp);
            ExpirationDate = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Renew(username, productManager.Group_IBSng, "Renew", GetIssuedAccount.Username, "");


            var updateItem = new User
            {
                Username = username,
                ServerId = server.Id,
                //ExpirationDate = DateTime.Now.AddMonths(1),
                ProductId = productManager.id,
                ExpirationDate = ExpirationDate == null ? null : ExpirationDate,
                RenewDate = DateTime.Now,
                Updated = DateTime.Now,
            };
            rUser.UpdateRenew(updateItem);




            //List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            //foreach (var server in _server)
            //{
            //    // IBSNG
            //    new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Renew(username, productManager.Group_IBSng, "Renew Only", GetIssuedAccount.Username, "");
            //}
            return new MResponce() { Success = true };
        }

        public MResponce? IBSNG_ChangeAccountId(string IssuedAccount_token, string username, int accountChangeId)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);


            // New Update PN
            var getUser = (User)rUser.Get(username);
            //var accountChild = (Account?)new Repository.RAccount().GetById(getUser.AccountId);
            if (getUser.AccountId != GetIssuedAccount.Id && GetIssuedAccount.Id > 2)
                return new MResponce { Success = false, Message = "Invalid Access Child Account" };



            var newAccountId = (Account?)new Repository.RAccount().GetById(accountChangeId);

            var updateItem = new User
            {
                Username = username,
                AccountId = accountChangeId,
                Updated = DateTime.Now,
            };

            rUser.UpdateAccountId(updateItem);

            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            foreach (var server in _server)
            {

                // IBSNG
                new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).UpdateAgent(getUser.Username, newAccountId.Username);
            }

            return new MResponce() { Success = true };
        }

        public MResponce? IBSNG_RenewTest(string IssuedAccount_token, string username, string Group_IBSng)
        {
            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            foreach (var server in _server)
            {
                // IBSNG
                new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).Renew(username, Group_IBSng, "Renew", "Admin-Renew-Sowger", "");
            }


            return new MResponce() { Success = true };

        }

        public MResponce? IBSNG_Get_And_Update_ExpairetionDate(string username, string serverIp)
        {
            object dateExp = "";

            try
            {
                Server? server = (Server?)new Repository.RServer()?.Get(serverIp);
                dateExp = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).GetExpairationDate(username);

                DateTime _dateExp = Convert.ToDateTime(dateExp);
                rUser.UpdateExpairetionDate(username, _dateExp);

                if (dateExp != "")
                {
                    dateExp = _dateExp == null ? "none" : _dateExp < DateTime.Now ? "Expir (" + (_dateExp - DateTime.Now).Days + ")" : Publics.DateFormatShort2(_dateExp).ToString() + " (+" + (_dateExp - DateTime.Now).Days + ")";
                    return new MResponce() { Success = true, Data = dateExp };
                }

            }
            catch (Exception ex)
            {
                return new MResponce() { Success = false, Message = "Er." + ex.Message };
            }


            return new MResponce() { Success = true, Data = dateExp };
        }

        public MResponce? IBSNG_Update(string IssuedAccount_token, string username, string password, string phoneNumber, string description, bool Banned, string BannedText)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            // New Update PN
            var getUser = (User)rUser.Get(username);
            //var accountChild = (Account?)new Repository.RAccount().GetById(getUser.AccountId);
            if (getUser.AccountId != GetIssuedAccount.Id && GetIssuedAccount.Id > 2)
                return new MResponce { Success = false, Message = "Invalid Access Child Account" };

            bool _Banned = true; // false = active
            BannedText = BannedText == null ? "" : BannedText;
            if (BannedText.ToString().Contains("***") && !Banned)
                _Banned = false;


            var updateItem = new User
            {
                Username = username,
                Password = password,
                Updated = DateTime.Now,
                Description = description,
                PhoneNumber = phoneNumber,
                //new Exception("");
                Banned = _Banned,
                BannedText = BannedText
            };

            rUser.Update(updateItem);

            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            foreach (var server in _server)
            {
                // IBSNG
                var token = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).GetCookieAuthoraition();
                var _UserId = new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).GetIdByUsername(username, token).ToString();
                new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).UpadateBanned(token, _Banned, BannedText, _UserId);
                new IBSNG.User(server.Ip + ":" + server.VpnserverPort, server.HubName, server.AdminPassword).UpadateUsernamePassword(token, username, password, _UserId);
            }

            return new MResponce() { Success = true };
        }

        public MResponce? UpdateServer(string username, string text = "")
        {

            var getItem = (User?)rUser.Get(username);


            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);

            foreach (var server in _server)
            {
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.Ip, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                // count user:
                var getUser = ApiVPNSERVER.AnyUser(username, server.HubName);

                if (getUser)
                {
                    #region Create User To Server
                    var v_newUserItem = new VpnRpcSetUser()
                    {
                        Name_str = username,
                        Realname_utf = getItem.Account.Username,
                        Auth_Password_str = getItem.Password,
                        HubName_str = server.HubName,
                        ExpireTime_dt = getItem.ExpirationDate,
                        Note_utf = "1 Month" + text,
                        GroupName_str = "1 User - (2M)",
                        Auth_NT_NTUsername = "",

                        AuthType_u32 = VpnRpcUserAuthType.Password,
                        Auth_UserCert_CertData = new byte[0] { },
                        Auth_RootCert_Serial = new byte[0] { },
                    };
                    ApiVPNSERVER.UpdateUser_WithoutPolicy(v_newUserItem);
                    #endregion
                }
            }

            return new MResponce { Success = true, Data = getItem };
        }

        public object? Report(string token)
        {
            var accountId = MAccount.GetAccountIdByRole(token);
            var account = new Repository.RAccount().GetByToken(token);


            var list = (List<User>?)rUser.List(accountId);

            list.AddRange((List<User>?)rUser.ListByParent(accountId));

            List<int> chartData = new List<int>();
            List<int> chartDataRenew = new List<int>();

            List<decimal> data = new List<decimal>();


            for (int i = 10; i >= 0; i--)
            {
                int start = -i;
                int end = -i + 1;
                var dateStart = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).AddDays(start);
                var dateEnd = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(end);


                chartDataRenew.Add(
                            list.Count(i =>
                            i.RenewDate != null &&
                            i.RenewDate.Value >= dateStart &&
                            i.RenewDate.Value < dateEnd)
                        );

                chartData.Add(
                        list.Where(i =>
                        i.CreateDate > dateStart &&
                        i.CreateDate < dateEnd).Count()
                    );


            }

            return new
            {
                countRenew = list.Count(i => i?.RenewDate != null),
                chartDataRenew = chartDataRenew.ToList(),
                todayRenew = chartDataRenew[10],
                percentRenew = chartDataRenew[9] != 0 ? (((chartDataRenew[10] - chartDataRenew[9]) / chartDataRenew[9]) * 100) : 0,

                count = list.Count,
                chartData = chartData.ToList(),
                today = chartData[10],
                Percent = chartData[9] != 0 ? (((chartData[10] - chartData[9]) / chartData[9]) * 100) : 0,
            };
        }

        // درحال نوشتن در زمان مناسب
        public async Task Rpt_UpdateUsersInfo()
        {
            List<Server>? _server = (List<Server>?)new Repository.RServer()?.List(null);
            foreach (var server in _server)
            {

                // get server info
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.VpnserverHost, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                foreach (var UserServer in ApiVPNSERVER.ListUser(server.HubName))
                {
                    //rUser.Update()
                }

            }

            await Task.Delay(5000);
        }


        // New 2025
        public MResponce? ChangeServerIp(string IssuedAccount_token, string username, string? serverIp)
        {
            Account? GetIssuedAccount = (Account?)new Repository.RAccount().GetByToken(IssuedAccount_token);

            if (!new Repository.RUser().Any2(GetIssuedAccount.Id, username))
                return new MResponce { Success = false, Message = "Invalid Access" };

            if (GetIssuedAccount.Id > 2)
                return new MResponce { Success = false, Message = "Access Denied!" };

            Server? server = (Server?)new Repository.RServer()?.Get(serverIp);

            var updateItem = new User
            {
                Username = username,
                ServerId = server.Id,
                Updated = DateTime.Now,
            };
            rUser.UpdateCahngeServerIp(updateItem);

            return new MResponce() { Success = true };
        }


    }
}