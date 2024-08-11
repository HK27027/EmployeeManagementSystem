using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApi.Model
{
    public class MethodResponse<T>
    {
        public int Status { get; set; } = 200;
        public List<string> StatusTexts {get; set;} = new List<string>();
        public T Item { get; set; }
        public long? Count { get; set; } = 0;    
      
    }
}
