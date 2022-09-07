using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorResult: Result
    {
        // İşlem sonucu başarısız ise bu class'dan alacak
        public ErrorResult(string message): base(false, message)
        {
            // Mesajlı hali
        }
        public ErrorResult(): base(false)
        {
            // Mesajsız hali
        }
    }
}
