using DataService.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectLayer.Models.Enums;
using ProjectLayer.Models.Structure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectLayer.Models.Mod
{
    /// <summary>
    /// احراز هویت
    /// </summary>
    public class MAuth
    {
        private Repository.RAccount rAccounts = new Repository.RAccount();
        private Repository.RTransaction rTransaction = new Repository.RTransaction();

        public MResponceAuth login(string username, string password)
        {
            if (rAccounts.Any1(username))
            {
                var U = (Account?)rAccounts.Get(username);
                if (U.CountFailed < U.CountFailedLimit)
                {
                    if (rAccounts.Any2(username, password))
                    {
                        rAccounts.Update(username, generateJwtToken(username));
                        rAccounts.UpdateCountFailed(username);
                        var get = (Account?)rAccounts.Get(username);

                        return new MResponceAuth() { accessToken = get.Token, user = get };
                    }
                    else
                    {
                        rAccounts.UpdateCountFailed(username, (int)U.CountFailed + 1);
                        return new MResponceAuth() { message = "Wrong password." };
                    }
                }
                else
                {
                    rAccounts.UpdateCountFailed(username, (int)U.CountFailed + 1);
                    return new MResponceAuth() { message = "Limited To Login! Please Contact To Support." };
                }
            }
            return new MResponceAuth() { message = "Invalid User And Password." };
        }

        public MResponceAuth registerA(string display, string username, string password, string email, string mobile, int deposit, int creditorLimit, string ref_token)
        {

            if (!rAccounts.Any1(username))
            {
                var parent = (Account?)rAccounts.GetByToken(ref_token);

                if (parent.RoleId > 3)
                    return new MResponceAuth() { message = "Access Denied." };

                var token = generateJwtToken(username);
                var roleId = MAccount.GetAccountIdByRole(ref_token);

                var finance = new MAccount().finance(parent.Id);
                if (roleId != 0 && creditorLimit > ((decimal)finance.Balance + (decimal)finance.CreditorLimit))
                    return new MResponceAuth() { message = "Your Creditor Limit" };

                Account newAccount = new Account()
                {
                    DisplayName = display,
                    Username = username,
                    Password = password,
                    Mobile = mobile,
                    Email = email,
                    Token = token,
                    ParentId = parent?.Id,
                    RoleId = 3,
                    Active = true,//deposit < 1 || creditorLimit < 1 ? false : true,
                    CreditorLimit = creditorLimit,
                    AvatarUrl = "https://www.dropbox.com/s/iv3vsr5k6ib2pqx/avatar_default.jpg?dl=1",
                };
                rAccounts.Insert(newAccount);

                if (roleId == 0)
                    if (deposit > 0)
                    {
                        Transaction newTransaction = new Transaction()
                        {
                            AccountId = newAccount.Id,
                            AccountIdParent = parent?.Id,
                            Date = DateTime.Now,
                            TransactionTypeId = (int)ETransaction.Type.Deposit,
                            Description = "Charge Panel.",
                            CurrencyId = (int)ECurrency.TOMAN,
                            RawAmount = deposit,
                            Changerate = 50000,
                            Creditor = deposit,
                            Debtor = 0,
                        };
                        rTransaction.Insert(newTransaction);
                    }


                var productUserList = (List<ProductUser>?)new Repository.RProductUser().List(parent?.Id);
                var minimumPerMonth = 200000;
                foreach (var item in productUserList)
                {
                    var newItem = new ProductUser()
                    {
                        AccountId = newAccount.Id,
                        Active = true,
                        Pesent = 0,
                        ProductId = item.ProductId,
                        StaticPrice = ((item.StaticPrice == 0 ? (decimal)(minimumPerMonth * item.Product.X) : item.StaticPrice) * (decimal)0.2) + item.StaticPrice,
                    };
                    new Repository.RProductUser().Insert(newItem);
                }


                return new MResponceAuth() { accessToken = token, user = newAccount };
            }
            return new MResponceAuth() { message = "Exist Username." };
        }


        public MResponceAuth Update(string username, string password, bool active, string email, string mobile, int creditorLimit, string ref_token)
        {

            if (rAccounts.Any1(username))
            {
                var parent = (Account?)rAccounts.GetByToken(ref_token);

                var AccountEdit = (Account?)rAccounts.GetByUseranem(username);
                if (AccountEdit.ParentId != parent.Id)
                    return new MResponceAuth() { message = "Access Denied." };

                if (parent.RoleId > 3)
                    return new MResponceAuth() { message = "Access Denied." };

                var token = generateJwtToken(username);

                Account editAccount = new Account()
                {
                    Username = username,
                    Password = password,
                    Mobile = mobile,
                    Email = email,
                    Token = token,
                    Active = active,
                    CreditorLimit = creditorLimit,
                    AvatarUrl = "https://www.dropbox.com/s/iv3vsr5k6ib2pqx/avatar_default.jpg?dl=1",
                };
                rAccounts.Update(editAccount);

                return new MResponceAuth() { accessToken = token, user = editAccount };
            }
            return new MResponceAuth() { message = "Exist Username." };
        }




        private string generateJwtToken(string username)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("key to some_big_key_value_here_secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", username) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? Authorize(string token)
        {
            if (token != null)
                if (token.Contains("Bearer ") || token.Contains("bearer "))
                    token = token.ToString().Split(' ')[1];

            if (rAccounts.AnyByToken(token))
                return token;

            else
                return null;

        }

        public object? CallBack(string token)
        {
            if (token != null)
                if (token.Contains("Bearer ") || token.Contains("bearer "))
                    token = token.ToString().Split(' ')[1];

            if (rAccounts.AnyByToken(token))
                return rAccounts.GetByToken(token);

            else
                return null;

        }



    }
}
