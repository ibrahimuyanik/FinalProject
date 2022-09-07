using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}


/*
 * Bu ise void olmayan işlemleriçin yazılır mesela ürünleri getir dediğimizde bize liste döner
 * Biz hem data dönen hem işlem sonucu dönen hem de işlem sonucunda mesaj dönen bir yapı kuramız lazım
 * 
 * Zaten önceden işlem sonucu ve mesaj veren yapıyı kurmuştuk
 * Tekrar yazmamak için IResult'dan onları implemente ettik
 *
 */