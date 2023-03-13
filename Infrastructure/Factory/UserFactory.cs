using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Factory
{
    public static class UserFactory
    {
        public static User GenerateUser()
        {
            Random rnd = new Random();
            int min = 0, max = 100000;

            return new User
            {
                Number = rnd.Next(min,max),
            };
        } 
    }
}
