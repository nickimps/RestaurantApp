using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject
{
    public class FoodCategory
    {
        public string categoryName { get; set; }
        public List<FoodItem> items { get; set; }
        public FoodCategory()
        {
            items = new List<FoodItem>();
            categoryName = "Blank";
        }

        public FoodCategory(string catName, List<FoodItem> items)
        {
            categoryName = catName;
            this.items = items;
        }

        public FoodCategory(string catName)
        {
            categoryName = catName;
        }
    }
}
