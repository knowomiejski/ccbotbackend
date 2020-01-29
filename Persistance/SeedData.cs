using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Persistance
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (!context.Settings.Any())
            {
                var settings = new List<Settings>
                {
                    new Settings
                    {
                        Name = "StandardSettings",
                        Prefix = "!",
                        ReminderTimer = 120,
                        FolderId = "1byWobqcHkOQMBELGSXK1WL3Gjrp-_J5_"
                    }
                };

                context.Settings.AddRange(settings);
                context.SaveChanges();
            }
        }
    }
}
