using System;
using System.Collections.Generic;
using System.Text;
using MyLib.Models;

namespace MyLib.Controllers
{
    class CustomerController
    {
        public static List<Customer> customers = new List<Customer>()
        {
            new Customer()
            {
                Id = 1,
                Name = "Petr",
                Age = 30,
                IsActive = true
            },
            new Customer()
            {
                Id = 2,
                Name = "Honza",
                Age = 18,
                IsActive = true
            },
            new Customer()
            {
                Id = 3,
                Name = "Jakub",
                Age = 22,
                IsActive = true
            }
        };

        public string List(int limit)
        {
            int i = 0;

            StringBuilder sb = new StringBuilder();

            foreach (Customer item in customers)
            {
                if (i < limit)
                {
                    sb.Append(item.Name)
                        .Append(" | vek: ").Append(item.Age)
                        .Append(" | je aktivni: ").Append(item.IsActive)
                        .Append(" | ID: ").Append(item.Id);
                    sb.AppendLine();
                    i++;
                }
            }

            return sb.ToString();
        }

        //public string Add(string Name, int Age, bool IsActive)
        //{
        //    Customer cus = new Customer()
        //    {
        //        Age = Age,
        //        Name = Name,
        //        IsActive = IsActive,
        //        Id = customers.Count + 1
        //    };

        //    customers.Add(cus);

        //    return cus.Id.ToString();
        //}

        public string Add(Customer customer)
        {
            customers.Add(customer);

            return customer.Id.ToString();
        }

    }
}
