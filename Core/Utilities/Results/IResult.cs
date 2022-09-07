using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    // temel void'ler için başlangıç
    // Api üzerinden gelen isteklere göre başarılı ve başarısız sonucu verecek
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
