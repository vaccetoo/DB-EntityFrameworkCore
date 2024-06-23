
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

        Console.WriteLine(GetEmployee147(context));
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
            .OrderBy (e => e.Salary)
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

        foreach( var addressText in addressTexts)
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

        foreach( var employee in employees)
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

        foreach ( var a in addresses)
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
}
