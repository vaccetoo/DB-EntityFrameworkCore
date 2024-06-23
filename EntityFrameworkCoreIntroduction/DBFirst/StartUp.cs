
using Microsoft.EntityFrameworkCore;
using SoftUni.Models;
using System.Diagnostics;
using System.Runtime.Loader;
using System.Text;

public class StartUp
{
    public static void Main()
    {
        // Problem 01. ---> Scaffold DB
        var context = new SoftUniContext();

        //Console.WriteLine(GetEmployeesFullInformation(context));
    }

    // Problem 02.
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        foreach (var e in employees)
        {
            output.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 03.
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Salary > 50_000)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .OrderBy(e => e.FirstName)
            .ToList();

        foreach (var e in employees)
        {
            output.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 04.
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                Departmen = e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToList();

        foreach (var e in employees)
        {
            output
                .AppendLine($"{e.FirstName} {e.LastName} from {e.Departmen} - ${e.Salary:f2}");
        }
        return output.ToString().TrimEnd();
    }

    // Problem 05.
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        Address newAddress = new Address
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };
        context.Addresses.Add(newAddress);

        Employee employee = context.Employees
            .First(e => e.LastName == "Nakov");

        employee.Address = newAddress;
        context.SaveChanges();

        List<string> addressTexts = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address.AddressText)
            .ToList();

        foreach (var addressText in addressTexts)
        {
            output
                .AppendLine(addressText);
        }

        return output.ToString().TrimEnd();
    }

    // Problem 06.
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var output = new StringBuilder();

        var employees = context.Employees
            .Take(10)
            .Select(e => new
            {
                EmployeeFullName = $"{e.FirstName} {e.LastName}",
                ManagerFullName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                Projects = e.EmployeesProjects
                    .Where(ep =>
                        ep.Project.StartDate.Year >= 2001
                     && ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ep.Project.StartDate,
                        EndDate = ep.Project.EndDate.HasValue ?
                        ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                    })
            }).ToList();

        foreach (var employee in employees)
        {
            output.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

            if (employee.Projects.Any())
            {
                foreach (var project in employee.Projects)
                {
                    output.AppendLine($"--{project.ProjectName} - {project.StartDate:M/d/yyyy h:mm:ss tt} - {project.EndDate}");
                }
            }
        }

        return output.ToString().TrimEnd();
    }

    // Problem 07.
    public static string GetAddressesByTown(SoftUniContext context)
    {
        var output = new StringBuilder();

        var addresses = context.Addresses
            .OrderByDescending(a => a.Employees.Count())
            .ThenBy(a => a.Town.Name)
            .ThenBy(a => a.AddressText)
            .Take(10)
            .Select(a => new
            {
                a.AddressText,
                TownName = a.Town.Name,
                EmployeesCount = a.Employees.Count()

            }).ToList();

        foreach (var a in addresses)
        {
            output.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 08.
    public static string GetEmployee147(SoftUniContext context)
    {
        var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(ep => ep.Project.Name)
                        .OrderBy(pn => pn)
                        .ToList()
                })
                .FirstOrDefault();

        if (employee == null)
        {
            return "Employee not found.";
        }

        var result = new StringBuilder();

        result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

        foreach (var projectName in employee.Projects)
        {
            result.AppendLine($"{projectName}");
        }

        return result.ToString().TrimEnd();
    }

    // Problem 09.
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var output = new StringBuilder();

        var departments = context.Departments
                .Where(employeesCount => employeesCount.Employees.Count() > 5)
                .Select(department => new
                {
                    DepartmentName = department.Name,
                    EmployeesCount = department.Employees.Count(),
                    ManagerFullName = $"{department.Manager.FirstName} {department.Manager.LastName}",
                    Employees = department.Employees.ToList()
                })
                .ToList();

        foreach (var department in departments.OrderBy(d => d.EmployeesCount))
        {
            output.AppendLine($"{department.DepartmentName} - {department.ManagerFullName}");

            foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                output.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            }
        }

        return output.ToString().TrimEnd();
    }

    // Problem 10.
    public static string GetLatestProjects(SoftUniContext context)
    {
        var output = new StringBuilder();

        var projects = context.Projects
                .OrderByDescending(d => d.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList();

        foreach (var project in projects.OrderBy(p => p.Name))
        {
            output.AppendLine(project.Name);
            output.AppendLine(project.Description);
            output.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
        }

        return output.ToString().TrimEnd();
    }

    // Problem 11.
    public static string IncreaseSalaries(SoftUniContext context)
    {
        var output = new StringBuilder();

        var employees = context.Employees
               .Where(e => e.Department.Name == "Engineering" ||
                           e.Department.Name == "Tool Design" ||
                           e.Department.Name == "Marketing" ||
                           e.Department.Name == "Information Services")
               .ToList();

        foreach (var employee in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
        {
            employee.Salary += employee.Salary * 0.12m;

            output.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
        }

        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    // Problem 12.
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var output = new StringBuilder();

        var employees = context.Employees
                .Where(fn => fn.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

        foreach (var employee in employees)
        {
            output.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 13.
    public static string DeleteProjectById(SoftUniContext context)
    {
        var projectToDelete = context.Projects.Find(2);

        if (projectToDelete != null)
        {
            var employeeProjects = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();

            context.EmployeesProjects.RemoveRange(employeeProjects);

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();
        }

        var projectNames = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToList();

        var output = new StringBuilder();
        foreach (var name in projectNames)
        {
            output.AppendLine(name);
        }

        return output.ToString().TrimEnd();
    }
}
