using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eMeterApi.Helpers
{
    public class DataHasher
    {
        public static string HashDataWithKey( string data, string key)
        {
            using( var hmac = new HMACSHA256( Encoding.UTF8.GetBytes(key))){
                byte[] hashBytes = hmac.ComputeHash( Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashBytes);
            }
        }

        public static bool VerifyDataWithKey( string data, string key, string hashedData){
            string newHashedData = HashDataWithKey( data, key);
            return hashedData == newHashedData;
        }
    }
}