using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeter.Models;
using eMeterApi.Data.Contracts.Models;
using eMeterApi.Models;

namespace eMeterApi.Data.Contracts
{
    public interface IUserService
    {
        
        public string? Authenticate(IUserCredentials userCredentials, out string? message );

        public IEnumerable<User>? GetUsers();

        public long? CreateUser( UserRequest user, IDictionary<string, object>? param, out string? message );

        public bool DisableUser( long userId, out string? message);

        public bool UpdateUser( long userId, UserUpdateRequest userUpdateRequest, out string? message );

    }
}