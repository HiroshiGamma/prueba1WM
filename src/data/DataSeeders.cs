using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Bogus;
using Bogus.DataSets;

namespace api.src.data
{
    public class DataSeeders
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<ApplicationDbContext>();

                var existingRut = new HashSet<string>();

                if (!context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Rut, f => GenerateUniqueRandomRut(existingRut))
                        .RuleFor(u => u.Nombre, f => f.Person.FullName)
                        .RuleFor(u => u.Correo, f => f.Person.Email)
                        .RuleFor(u => u.Genero, f => f.PickRandom(new[] { "masculino", "femenino", "otro", "prefiero no decirlo" }))
                        .RuleFor(u => u.FechaNacimiento, f => f.Date.Past(80, DateTime.Now).ToString("dd-MM-yyyy")); 

                    var users = userFaker.Generate(10);
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                context.SaveChanges();
                
                
            }
        }

        
        private static string GenerateUniqueRandomRut(HashSet<string> existingRuts)
        {
            string rut;
            do
            {
                rut = GenerateRandomRut();
            } while (existingRuts.Contains(rut));

            existingRuts.Add(rut);
            return rut;
        }

        private static string GenerateRandomRut()
        {
            Random random = new Random();
            int number = random.Next(10000000, 99999999); // Genera un número entre 10.000.000 y 99.999.999
            char checkDigit = GenerateCheckDigit(number);
            return $"{number}-{checkDigit}";
        }

        private static char GenerateCheckDigit(int number)
        {
            int sum = 0;
            int factor = 2;

            while (number > 0)
            {
                int digit = number % 10;
                sum += digit * factor;
                factor = factor == 7 ? 2 : factor + 1;
                number /= 10;
            }

            int mod = 11 - (sum % 11);
            if (mod == 11) return '0';
            if (mod == 10) return 'K';

            return mod.ToString()[0];
        }
    }
}
