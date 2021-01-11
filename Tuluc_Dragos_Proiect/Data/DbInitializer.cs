using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tuluc_Dragos_Proiect.Models;
using Microsoft.EntityFrameworkCore;

namespace Tuluc_Dragos_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();
            if (context.Hammocks.Any())
            {
                return; // BD a fost creata anterior
            }
            var hammocks = new Hammock[]
            {
                new Hammock{Nume="Balanganelul",Culoare="Verde",Producator="Yoummock",Pret=Decimal.Parse("22")},
                new Hammock{Nume="Usurelul",Culoare="Rosu",Producator="Balanganel",Pret=Decimal.Parse("18")},

            };
            foreach (Hammock s in hammocks)
            {
                context.Hammocks.Add(s);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

                new Customer{CustomerID=1050,Name="Popescu Marcela",BirthDate=DateTime.Parse("1979-09-01")},
                new Customer{CustomerID=1045,Name="Mihailescu Cornel",BirthDate=DateTime.Parse("1969-07-08")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
                new Order{HammockID=1,CustomerID=1050, OrderDate=DateTime.Parse("2020-07-08 12:12 AM")},
                new Order{HammockID=2,CustomerID=1045, OrderDate=DateTime.Parse("2020-06-08 12:13 AM")},

            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }


            context.SaveChanges();

            var distributors = new Distribuitor[]
 {

 new Distribuitor{DistributorName="GLE",Adress="Str. Aviatorilor, nr. 40, Bucuresti"},
 new Distribuitor{DistributorName="FNT",Adress="Str. Plopilor, nr. 35, Ploiesti"},

 };
            foreach (Distribuitor p in distributors)
            {
                context.Distribuitors.Add(p);
            }
            context.SaveChanges();
            var distributedhammocks = new DistributedHammock[]
            {
                 new DistributedHammock {
                 HammockID = hammocks.Single(c => c.Nume == "Balanganelul" ).ID, DistribuitorID = distributors.Single(i => i.DistributorName == "GLE").ID },
                 new DistributedHammock { HammockID = hammocks.Single(c => c.Nume == "Usurelul" ).ID, DistribuitorID = distributors.Single(i => i.DistributorName == "FNT").ID },
 
            };
            foreach (DistributedHammock pb in distributedhammocks)
            {
                context.DistributedHammocks.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
