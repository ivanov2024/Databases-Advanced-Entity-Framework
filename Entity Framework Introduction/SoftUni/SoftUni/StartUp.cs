using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using SoftUniContext dbContext = new SoftUniContext();



            dbContext.Database.EnsureCreated();

            string result = RemoveTown(dbContext);
            Console.WriteLine(result);
        }

        //Task 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Task 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary,
                })
                .Where(d => d.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            const string newAddressText = "Vitoshka 15";

            const int newTownId = 4;

            Address newAddress = new Address()
            {
                AddressText = newAddressText,
                TownId = newTownId,
            };

            Employee employee = context.Employees
                .First(e => e.LastName.Equals("Nakov"));

            employee.Address = newAddress;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine(e);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
               .Select(e => new
               {
                   EmployeeFirstName = e.FirstName,
                   EmployeeLastName = e.LastName,
                   ManagerFirstName = e.Manager.FirstName ?? null,
                   ManagerLastName = e.Manager.LastName ?? null,
                   Projects = e.EmployeesProjects
                        .Select(ep => ep.Project)
                        .Where(p => p.StartDate.Year >= 2001
                               && p.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            ProjectName = p.Name,
                            p.StartDate,
                            p.EndDate,
                        })
                        .ToArray()
               })
               .Take(10)
               .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    string startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    string endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    sb.AppendLine($"--{p.ProjectName} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .GroupBy(e => new
                {
                    e.Address.AddressText,
                    e.Address.Town.Name
                })
                .Select(e => new
                {
                    AddressText = e.Key.AddressText ?? null,
                    TownName = e.Key.Name ?? null,
                    EmployeesCount = e.Count(),
                })
                .OrderByDescending(e => e.EmployeesCount)
                .ThenBy(e => e.TownName)
                .ThenBy(e => e.AddressText)
                .Take(10)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.AddressText}, {e.TownName} - {e.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,

                    e.LastName,
                    e.JobTitle,
                    EmployeeProjects = e.EmployeesProjects
                        .Select(ep => ep.Project)
                        .Select(p => new
                        {
                            ProjectName = p.Name
                        })
                        .OrderBy(p => p.ProjectName)
                        .ToArray()
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

                foreach (var p in e.EmployeeProjects)
                {
                    sb.AppendLine(p.ProjectName);
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
            .Where(d => d.Employees.Count > 5)
            .OrderBy(d => d.Employees.Count)
            .ThenBy(d => d.Name)
            .Select(d => new
            {
                DepartmentName = d.Name,
                ManagerFirstName = d.Manager.FirstName,
                ManagerLastName = d.Manager.LastName,
                Employees = d.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName,
                        JobTitle = e.JobTitle
                    })
                    .ToArray()
            })
            .ToArray();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(project => project.StartDate)
                .Take(10)
                .OrderBy(project => project.Name)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(d => d.Department.Name.Equals("Engineering") ||
                       d.Department.Name.Equals("Tool Design") ||
                       d.Department.Name.Equals("Marketing") ||
                       d.Department.Name.Equals("Information Services"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${((e.Salary) + (e.Salary * 0.12m)):F2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach( var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            const int deleteProjectId = 2;

            var employeesProjectsDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == deleteProjectId)
                .ToArray();
            context.EmployeesProjects.RemoveRange(employeesProjectsDelete);

            var deleteProject = context.Projects
                .Find(deleteProjectId);

            if (deleteProject != null)
            {
                context.Projects.Remove(deleteProject);
            }

            context.SaveChanges();

            var projectNames = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, projectNames);
        }

        //Task 15
        public static string RemoveTown(SoftUniContext context)
        {
            const string townNameToDelete = "Seattle";
            
            var town = context.Towns
                .FirstOrDefault(t => t.Name == townNameToDelete);          

            var addressesToDelete = context.Addresses
                .Where(a => a.TownId == town.TownId)
                .ToList();

            int count = addressesToDelete.Count();

            var employeesToUpdate = context.Employees
            .Where(e => context.Addresses
            .Where(a => a.Town.Name == "Seattle")
            .Select(a => (int?)a.AddressId)
            .Contains(e.AddressId))
            .ToList();

            foreach (var employee in employeesToUpdate)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}
