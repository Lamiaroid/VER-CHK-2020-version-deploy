using MailKit.Net.Smtp;
using MimeKit;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPosting.Models;

namespace WebPosting.Services
{
    /// <summary>
    /// Service to work with users
    /// </summary>
    public class UserService
    {
        // take your login
        const string SenderLogin = "test@gmai.com";

        // take your password
        const string SenderPassword = "admin";

        const string Host = "smtp.gmail.com";
        const int Port = 587;

        IMongoCollection<UserModel> Users;

        public UserService()
        {
            string connectionString = "mongodb://127.0.0.1:27017/WebPosting";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Users = database.GetCollection<UserModel>("Users");
        }

        /// <summary>
        /// Get all users with filter from data base
        /// </summary>
        /// <param name="searchingQuery">String for filtering</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserModel>> GetUsers(string searchingQuery)
        {
            var builder = new FilterDefinitionBuilder<UserModel>();
            var filter = builder.Empty;

            if (!String.IsNullOrEmpty(searchingQuery))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(searchingQuery));
            }

            return await Users.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Add a new user to data base
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns></returns>
        public async Task Create(UserModel user)
        {
            await Users.InsertOneAsync(user);
        }

        /// <summary>
        /// Get a single user from data base
        /// </summary>
        /// <param name="user">User to find</param>
        /// <param name="byEmail">If to search by email</param>
        /// <returns></returns>
        public async Task<UserModel> GetUser(UserModel user, bool byEmail)
        {
            if (byEmail)
            {
                return await Users.Find(x => x.Email == user.Email).FirstOrDefaultAsync();
            }
            else
            {
                return await Users.Find(x => x.Name == user.Name).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Send the forgotten password to user
        /// </summary>
        /// <param name="user">User to send password to</param>
        /// <returns></returns>
        public async Task SendPasswordByEmail(UserModel user)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Site administration", "article_site_admin@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(user.Name, user.Email));
            emailMessage.Subject = "Password";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "Your password: " + user.Password
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Host, Port, false);
                    await client.AuthenticateAsync(SenderLogin, SenderPassword);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch
            { }
        }

    }
}