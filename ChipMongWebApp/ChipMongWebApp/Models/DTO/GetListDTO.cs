using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO
{
    public class GetListDTO<T>
    {
       public MetaDataDTO metaData;
       public List<T> items;
    }
}