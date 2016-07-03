using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    public enum NamePart : int
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        LastName    = 0,

        /// <summary>
        /// Имя
        /// </summary>
        FirstName   = 1,

        /// <summary>
        /// Отчество
        /// </summary>
        MiddleName  = 2
    }
}
