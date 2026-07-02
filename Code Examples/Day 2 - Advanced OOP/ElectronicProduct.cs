using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp1;

namespace ConsoleApp1
{
    public class ElectronicProduct : Product2, IReturnable
    {

        public int WarrrantyInMonths { get; set; }

        public ElectronicProduct(int id, string name, decimal price, int stock, int warrantyInMonths) : base(id, name, price, stock)
        {
            WarrrantyInMonths = warrantyInMonths;
        }

        public override decimal CalculateDiscount()
        {
            return Price * 0.01m;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Warrant: {WarrrantyInMonths} Months");
            Console.WriteLine("----------------------");

        }

        public void ReturnProduct()
        {
            Console.WriteLine("Product has been marked for return");
        }

    }
}
