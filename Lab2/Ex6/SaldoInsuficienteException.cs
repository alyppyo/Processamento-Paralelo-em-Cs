using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex6
{
    internal class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException() : base()
        {
        }

        public SaldoInsuficienteException(string? message) : base(message)
        {
        }

        public SaldoInsuficienteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
