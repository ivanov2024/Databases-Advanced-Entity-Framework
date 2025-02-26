using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data;

namespace P02_FootballBetting
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting!");

            try
            {
                using var dbContext = new FootballBettingContext();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                Console.WriteLine("The database has been created!");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
