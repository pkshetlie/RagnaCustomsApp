using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagnaCustoms
{
    public class Language
    {
        public string Text { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
