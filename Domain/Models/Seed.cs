using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nordic.Taxes.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Taxes.Domain.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
            {
                if (context.Taxes.Any())
                {
                    return;   // DB has been seeded
                }


                context.Municipalities.AddRange(
					new Municipality
					{
						Name = "Copenhagen"
					}, 
					new Municipality
					{
						Name = "Aarhus"
					},
					new Municipality
					{
						Name = "Aalborg"
					}
				);

				context.Taxes.AddRange(
					new Tax
					{
						TaxType = TaxPeriod.Year,
						From = new DateTime(DateTime.Today.Year, 1, 1),
						To = (new DateTime(DateTime.Today.Year, 1, 1)).AddYears(1).AddTicks(-1),
						MunicipalityId = 1,
						TaxSize = 0.2
					},
					new Tax
					{
						TaxType = TaxPeriod.Month,
						From = new DateTime(2021, 4, 1),
						To = new DateTime(2021, 4, 30),
						MunicipalityId = 1,
						TaxSize = 0.4
					},
					new Tax
					{
						TaxType = TaxPeriod.Week,
						From = new DateTime(2021, 6, 21),
						To = new DateTime(2021, 6, 27),
						MunicipalityId = 1,
						TaxSize = 0.4
					},
					new Tax
					{
						TaxType = TaxPeriod.Day,
						From = new DateTime(2021, 1, 1),
						To = new DateTime(2021, 1, 1),
						MunicipalityId = 1,
						TaxSize = 0.1
					},
					new Tax
					{
						TaxType = TaxPeriod.Day,
						From = new DateTime(2021, 12, 24),
						To = new DateTime(2021, 12, 24),
						MunicipalityId = 1,
						TaxSize = 0.1
					}
				);

				context.SaveChanges();
            }
        }
    }
}
