using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting!");

            try
            {
                using var dbContext = new StudentSystemContext();
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
