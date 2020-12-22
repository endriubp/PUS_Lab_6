using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{
    class People
    {
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public int year { get; set; }

        public override string ToString()
        {
            return this.id +". " + this.name + ", " + this.city + ", " + this.year.ToString();
        }
    }
}
