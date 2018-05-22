using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.Dapper
{
    public class TableMapping : Attribute
    {
        public string ConfigName { get; set; }
    }
}
