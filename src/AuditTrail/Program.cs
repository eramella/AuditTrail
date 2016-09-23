using AuditTrail.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditTrail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new AuditContext())
            {
                try
                {
                    if (!db.Products.Any())
                    {
                        Product p = new Product {
                            Id = new Guid(),
                            Color = "Blue",
                            Name = "iPhone 7"
                        };
                        db.Products.Add(p);
                        db.SaveChanges();
                    }

                    if (!db.Users.Any())
                    {
                        User u = new User {
                        Id = new Guid(),
                        FirstName = "John",
                        LastName = "Doe"
                        };
                        db.Users.Add(u);
                        db.SaveChanges();
                    }

                    while (true)
                    {
                        foreach (var log in db.AuditLogs)
                        {
                            Console.WriteLine("{0:s} - {1}.{2} OLD: {3} => NEW: {4}", log.DateChanged, log.EntityName, log.PropertyName, log.OldValue, log.NewValue);
                        }
                        Console.Write("Press ENTER to update log: ");
                        Console.ReadLine();
                        UpdateValues(db);
                    }


                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                    throw;
                }
            }
        }

        private static void UpdateValues(AuditContext db)
        {
            Random r = new Random();
            int i = r.Next(1, 3);
            UpdateUser(db, i);
            UpdateProduct(db, i);
            db.SaveChanges();

        }

        private static void UpdateUser(AuditContext db, int i)
        {
            var user = db.Users.FirstOrDefault();
            switch (i)
            {
                case 1:
                    user.FirstName = "Robert";
                    user.LastName = "Jones";
                    break;
                case 2:
                    user.LastName = "Forever";
                    break;
                case 3:
                    user.FirstName = "Albano";
                    break;
            }           
        }

        private static void UpdateProduct(AuditContext db, int i)
        {
            var product = db.Products.FirstOrDefault();
            switch (i)
            {
                case 1:
                    product.Color = "Gray";
                    product.Name = "Palm";
                    break;
                case 2:
                    product.Color = "Pink";
                    break;
                case 3:
                    product.Name = "Washer";
                    break;
            }
        }
    }
}
