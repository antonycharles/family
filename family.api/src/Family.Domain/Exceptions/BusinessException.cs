using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Family.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message){
            
        }
    }
}