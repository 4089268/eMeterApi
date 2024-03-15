using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eMeterApi.Data.Exceptions
{
    public class SimpleValidationException : ValidationException
    {
        private ICollection<KeyValuePair<string, string>> _validationErrors = Array.Empty<KeyValuePair<string, string>>();
        public ICollection<KeyValuePair<string, string>> ValidationErrors {get => _validationErrors;}
        
        public SimpleValidationException(string message, ICollection<KeyValuePair<string, string>> erros)
            : base(message, null)
        {
            this._validationErrors = erros;
        }

        public SimpleValidationException(string message, ICollection<KeyValuePair<string, string>> erros, Exception? inner)
            : base(message, inner)
        {
            this._validationErrors = erros;
        }

    }
}