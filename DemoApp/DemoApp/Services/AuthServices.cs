using DemoApp.Classes;
using DemoApp.Core;
using DemoApp.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace DemoApp.Services
{
    public class AuthServices : IAuthServices
    {
        private Datamanager _dataManager;
        private DataSet _dataSet = new();
        private List<DataParam> _list = new();
        private Response _response;

        public AuthServices(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _dataManager = new Datamanager(configuration, contextAccessor);
        }

        public Response Authenticate(LoginReqDTO loginReq)
        {
            _list.Add(new DataParam() { Name = "", Value = loginReq.UserName });
            _dataSet = _dataManager.GetDataSet("LoginGet", _list, loginReq.CompID);
            if (_dataSet != null && _dataSet.Tables.Count > 0)
            {
                var encKey = _dataSet.Tables[0].Rows[0]["encKey"].ToString();
                var password = _dataSet.Tables[0].Rows[0]["password"].ToString();
                if (string.Equals(Encrypt(loginReq.Password, encKey), password) {
                    _response.ResponseCode = StatusCodes.Status200OK;
                    _response.ResponseStatus = true;

                }
                else {
                    _response.ResponseCode = StatusCodes.Status401Unauthorized;
                    _response.ResponseStatus = false;
                }

            }
            return _response;
        }


        #region Settings

        private const int Iterations = 2;
        private const int KeySize = 256;

        private const string Hash = "SHA1";
        private const string Salt = "aselrias38490a32";
        private const string Vector = "8947az34awl34kjq";

        #endregion
        #region Encryption
        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }
        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }
        private static string Encrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var valueBytes = Encoding.UTF8.GetBytes(value);

            using var cipher = new T();
            var passwordBytes =
                new PasswordDeriveBytes(password, saltBytes, Hash, Iterations);
            var keyBytes = passwordBytes.GetBytes(KeySize / 8);

            cipher.Mode = CipherMode.CBC;

            using var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes);
            using var to = new MemoryStream();
            using var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write);
            writer.Write(valueBytes, 0, valueBytes.Length);
            writer.FlushFinalBlock();
            var encrypted = to.ToArray();

            cipher.Clear();

            return Convert.ToBase64String(encrypted);
        }
        private static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            var valueBytes = Convert.FromBase64String(value);
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] decrypted;
            var decryptedByteCount = 0;

            using var cipher = new T();
            var passwordBytes = new PasswordDeriveBytes(password, saltBytes, Hash, Iterations);
            var keyBytes = passwordBytes.GetBytes(KeySize / 8);

            cipher.Mode = CipherMode.CBC;

            try
            {
                using var decrypt = cipher.CreateDecryptor(keyBytes, vectorBytes);
                using var from = new MemoryStream(valueBytes);
                using var reader = new CryptoStream(@from, decrypt, CryptoStreamMode.Read);
                decrypted = new byte[valueBytes.Length];
                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }

            cipher.Clear();
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
        public static string GetKey()
        {
            var guid = Guid.NewGuid();
            var encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded[..22];
        }
        #endregion
    }
}
