using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eMeterApi.Data.Contracts.Models;

namespace eMeterApi.Data.Contracts
{
    public interface IUserService
    {
        
        public string? Authenticate(IUserCredentials userCredentials, out string? message );

        public IEnumerable<IUser>? GetUsers();

        public long? CreateUser( IUser user, IDictionary<string, object>? param, out string? message );

        public bool DisableUser( long userId, out string? message);

        public bool UpdateUser( long userId, IDictionary<string, object>? param, out string? message );

    }
}