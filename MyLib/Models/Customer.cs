using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyLib.Models
{
    class IgnoreAttribute : Attribute
    {

    }


    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        [Ignore]
        public bool IsActive { get; set; }
    }
}
