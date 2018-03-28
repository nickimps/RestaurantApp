using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject
{
    class TableLogic
    {
        public List<Bill> bills;
        public int numOfGuests { get; set; }
        
        public TableLogic()
        {
            bills = new List<Bill>();
        }

        public TableLogic(int guests)
        {
            numOfGuests = guests;
            for (int i = 0; i<guests; i++)
            {
                bills.Add(new Bill((i+1).ToString()));
            }
        }

    }
}
