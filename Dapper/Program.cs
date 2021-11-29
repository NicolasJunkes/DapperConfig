using Dapper;
using DapperConfig.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperConfig
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString =
                "Server=localhost,1433;Database=balta;user ID=sa;Password=@Nicolas14022000";

            using (var connection = new SqlConnection(connectionString))
            {
                //ListCategories(connection);
                //UpdateConnection(connection);
                //CreateManyConnection(connection);
                //ListCategories(connection);
                //CreateConnection(connection);
                //ExecuteProcedure(connection);
                ExecuteReadProcedure(connection);
            }
        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {

                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void CreateConnection(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO [Category] 
                            VALUES(
                                @Id, 
                                @Title, 
                                @Url,
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured)";

            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Description,
                category.Order,
                category.Summary,
                category.Featured
            });

            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void UpdateConnection(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2021"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }

        static void CreateManyConnection(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;

            var insertSql = @"INSERT INTO [Category] 
                            VALUES(
                                @Id, 
                                @Title, 
                                @Url,
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured)";

            var rows = connection.Execute(insertSql, new[] {
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Description,
                    category.Order,
                    category.Summary,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Description,
                    category2.Order,
                    category2.Summary,
                    category2.Featured
                }
            });

            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var preocedure = "[spDeleteStudent]";
            var pars = new { StudentId = "dce29266-1e6e-4563-b47e-21765ec7d660" };

            var affectRows = connection.Execute(preocedure, pars, commandType: CommandType.StoredProcedure);

            Console.WriteLine(affectRows);
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var preocedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

            var courses = connection.Query(preocedure, pars, commandType: CommandType.StoredProcedure);

            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }
    }
}
