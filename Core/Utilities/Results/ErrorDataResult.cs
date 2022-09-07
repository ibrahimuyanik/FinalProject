using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // İşlem sonucu yanlışsa bu class
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
            // Data ve mesajı vermek için
        }
        public ErrorDataResult(T data) : base(data, false)
        {
            // Sadece datayı vermek için
        }
        public ErrorDataResult(string message) : base(default, false, message)
        {
            // Sadece mesaj vermek için datayı default hali ile döndürür
        }
        public ErrorDataResult() : base(default, false)
        {
            // Sadece datanın default halini döner mesaj vermez
        }
    }
}
