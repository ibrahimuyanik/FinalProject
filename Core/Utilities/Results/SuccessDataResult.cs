using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T>: DataResult<T> 
    {
        // İşlem sonucu doğruysa bu class
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
            // Data ve mesajı vermek için
        }
        public SuccessDataResult(T data):base(data, true)
        {
            // Sadece datayı vermek için
        }
        public SuccessDataResult(string message): base(default, true, message)
        {
            // Sadece mesaj vermek için datayı default hali ile döndürür
        }
        public SuccessDataResult(): base(default, true)
        {
            // Sadece datanın default halini döner mesaj vermez
        }
    }
}
