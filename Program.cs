using Dapper;
using Microsoft.Data.SqlClient;
using SQL_Pract_2.Models;
using SQL_Pract_2.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Z.Dapper.Plus;

namespace SQL_Pract_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory(@"Server=DESKTOP-EAQLDKO\SQLEXPRESS;Database=DapperShop;TrustServerCertificate=true;Trusted_Connection=True;");

            using (var connection = factory.CreateConnection())
            {

                //var sql = @"SELECT P.Id, P.Name, P.Price, C.Name Category
                //        FROM Products P
                //        JOIN Categories C ON C.Id = P.CategoryId";

                //var select = connection.Query(sql);

                //foreach (var it in select)
                //{
                //    Console.WriteLine($"{it.Id}  {it.Name}  {it.Price}  {it.Category}");
                //}


                //var sql1 = @"INSERT INTO Products(Name,Price,CategoryId)
                //            VALUES(@Name, @Price, @CategoryId)";

                //var insert = connection.Execute(sql1, new { @Name = "Mouse", @Price = 25.50, @CategoryId = 1 });

                //if (insert > 0)
                //{

                //    Console.WriteLine("Product add to SQL");
                //}

                //var sql3 = @"UPDATE Products SET Price = @Price WHERE Name = @Name";


                //var update = connection.Execute(sql3, new { @Price = 27.99, @Name = "Mouse" });

                //if(update > 0)
                //{
                //    Console.WriteLine("Product update from SQL");
                //}
                //var sql2 = @"DELETE FROM Products WHERE Id = @Id";
                //var delete = connection.Execute(sql2, new { @Id = 6 });

                //if (delete > 0)
                //{
                //      Console.WriteLine("Product delete from SQL");
                //}


                ////////////Part 2

                //                1.Отримати всі товари з назвами їхніх категорій
                //•	SELECT з JOIN між Products та Categories.

                var sql4 = @"SELECT * FROM Products P JOIN Categories C ON P.CategoryId = C.Id";

                var select2 = connection.Query<Product, Category, Product>(sql4, (product, category) =>
                {
                    product.Category = category;
                    return product;
                }, splitOn: "Id"
                ).ToList();

                foreach (var it in select2)
                {
                    Console.WriteLine($"Id: {it.Id,-5}Name {it.Name,-20}Price {it.Price,-15}CategoryId: {it.CategoryId,-5} Category {it.Category.Name}");
                }

                //                2.Отримати всі категорії та кількість товарів у кожній
                //•	SELECT з GROUP BY.


                //var sql5 = @"SELECT C.Name, COUNT(P.Id) AS CountProduct FROM Products P JOIN Categories C ON P.CategoryId = C.Id GROUP BY C.Name";
                //Console.WriteLine();
                //var select3 = connection.Query(sql5);
                //foreach (var it in select3)
                //{
                //    Console.WriteLine($"{it.Name} {it.CountProduct}");
                //}
                //Console.WriteLine();


                //                3.Отримати всі замовлення з іменем покупця, датою та списком товарів у замовленні
                //•	SELECT з JOIN між Orders, Customers, OrderProducts, Products.


                //var sql6 = @"SELECT O.OrderDate, C.FullName, C.Email, OP.Quantity, P.Name, P.Price
                //            FROM Orders O 
                //            JOIN Customers C ON C.Id = O.CustomerId
                //            JOIN OrderProducts OP ON OP.OrderId = O.Id
                //            JOIN Products P ON P.Id = OP.ProductId";


                //var select4 = connection.Query(sql6);
                //foreach (var it in select4)
                //{
                //    Console.WriteLine($"Order: OrderDate {it.OrderDate,-25} Customer: FullName {it.FullName,-20}Email {it.Email,-15}Quantity: {it.Quantity,-15}Product:  Name{it.Name, -15} Price {it.Price}");
                //}
                //Console.WriteLine();


                //            4.Створити нове замовлення для наявного покупця
                //•	INSERT INTO Orders і INSERT INTO OrderProducts.

                //var sql7 = @"INSERT INTO Orders 
                //            VALUES (@CustomerId,@OrderDate)
                //            SELECT CAST(SCOPE_IDENTITY() as int)";

                //var insert1 = connection.QuerySingleOrDefault<int>(sql7, new { @CustomerId = 1 });

                //if(insert1 > 0) Console.WriteLine("Add Orders to SQL");


                //var sql8 = @"INSERT INTO OrderProducts
                //            VALUES (@OrderId, @ProductId, @Quantity)";

                //var insert2 = connection.Execute(sql8, new { @OrderId = insert1, @ProductId = 2, @Quantity = 4 });

                //if (insert2 > 0) Console.WriteLine("Add OrderProducts to SQL");


                //                5.Отримати всіх покупців і кількість їхніх замовлень
                //•	SELECT з JOIN і GROUP BY.

                //var sql9 = @"SELECT C.FullName , COUNT(O.Id) AS CountOrders
                //            FROM Customers C
                //            JOIN Orders O ON C.Id = O.CustomerId
                //            GROUP BY C.FullName";

                //var select5 = connection.Query(sql9).ToList();

                //Console.WriteLine();
                //foreach (var it in select5)
                //{
                //    Console.WriteLine($"Customer: FullName {it.FullName ,-20}CountOrders {it.CountOrders}");
                //}
                //Console.WriteLine();

                //                6.Отримати загальну суму кожного замовлення(сума = ціна * кількість)
                //•	SELECT з JOIN і агрегатною функцією SUM.

                var sql10 = @"SELECT O.Id AS OrderId, SUM(OP.Quantity * P.Price) AS SumOrder
                            FROM Orders O
                            JOIN OrderProducts OP ON O.Id = OP.OrderId
                            JOIN Products P ON P.Id = OP.ProductId
                            GROUP BY O.Id";

                var select6 = connection.Query(sql10);

                Console.WriteLine();
                foreach (var it in select6)
                {
                    Console.WriteLine($"OrderId {it.OrderId,-10}SumOrder {it.SumOrder}");
                }
                Console.WriteLine();


                //                7.Знайти найдорожчий товар і покупця, який його замовив
                //•	SELECT з JOIN, MAX.

                var sql11 = @"SELECT P.Name AS Product, P.Price, C.FullName AS Customer
                            FROM Products P
                            JOIN OrderProducts OP ON OP.ProductId = P.Id
                            JOIN Orders O ON O.Id = OP.OrderId
                            JOIN Customers C ON C.Id = O.CustomerId
                            WHERE P.Price = ( SELECT MAX(Price)
                                            FROM Products)";

                var select7 = connection.Query(sql11);

                Console.WriteLine();
                foreach (var it in select7)
                {
                    Console.WriteLine($"Product: {it.Product,-15}Price: {it.Price,-15}Customer: {it.Customer}");
                }
                Console.WriteLine();





                //    ///9.

                //var products = new List<Product>{
                //new Product { Name = "Tablet", Price = 299.99M, CategoryId = 1 },
                //new Product { Name = "E-Reader", Price = 129.99M, CategoryId = 2 },
                //new Product { Name = "Sneakers", Price = 89.99M, CategoryId = 3 }
                // };


                //object value = connection.BulkInsert(products);

                //    //10

                //    //    var productsToDelete = connection.Query<Product>(
                //    //"SELECT * FROM Products WHERE Price < 20").ToList();

                //    //    connection.BulkDelete(productsToDelete);

                //    //11  

                //    var produc = connection.Query<Product>("SELECT * FROM Products").ToList();
                //    produc.ForEach(p => p.Price *= 1.1M); // підвищення ціни на 10%
                //    connection.BulkUpdate(produc);





                //    ////1.ДЗ Підвищит ціну продукту який має найменшу кількість продажів
                //    var sql = @"UPDATE Products P SET P.Price = P.Price * 1.1 WHERE P.Id = (
                //                SELECT TOP 1 P.Id 
                //                FROM Products P 
                //                JOIN OrderProducts OP ON OP.ProductId = P.Id 
                //                GROUP BY P.Id 
                //                ORDER BY SUM(OP.Quantity) )";

                //    var updateprod = connection.Execute(sql);
                //    if (updateprod > 0) Console.WriteLine("Update Price from min value Quantity in Products");

                //    ///
                //    /// 2.  1ДЗ  в процедурі

                //string createProcedureSql = @"CREATE PROCEDURE SP_UpPriceProbuctMinQuantity
                //                            AS
                //                            BEGIN
                //                            UPDATE Products SET Price = Price * 1.1 WHERE Id = (
                //                            SELECT TOP 1 P.Id 
                //                            FROM Products P 
                //                            JOIN OrderProducts OP ON OP.ProductId = P.Id 
                //                            GROUP BY P.Id 
                //                            ORDER BY SUM(OP.Quantity) ) 
                //                            END";

                //connection.Execute(createProcedureSql);

                //connection.Execute("SP_UpPriceProbuctMinQuantity", commandType: System.Data.CommandType.StoredProcedure);

                //    /// 3. Тригери на видалення об'єктів таблиць: видалені об'єкти переносяться в таблицю видалених об'єктів
                //    /// 

                //string create_table = @"CREATE TABLE OrderProductsLogDelete
                //                    (
                //                        ProductId INT NOT NULL,
                //                        Name NVARCHAR(100) NOT NULL,
                //                        Price MONEY NOT NULL,
                //                        CategoryId INT NOT NULL,
                //                        CategoryName NVARCHAR(100) NOT NULL,
                //                        DateLog DATE DEFAULT GETDATE()
                //                    )";

                //connection.Execute(create_table);

                //string create_trg = @"CREATE TRIGGER TR_LogDeletedProductInfo
                //                    ON Products
                //                    AFTER DELETE
                //                    AS
                //                    BEGIN
                //                    INSERT INTO OrderProductsLogDelete
                //                    (ProductId, Name, Price, CategoryId, CategoryName)
                //                    SELECT P.Id, P.Name, P.Price, P.CategoryId, C.Name
                //                    FROM Deleted P
                //                    JOIN Categories C ON C.Id = P.CategoryId
                //                    END";

                //connection.Execute(create_trg);

                //connection.Execute("DELETE FROM Products WHERE LOWER(Name) = LOWER('Sneakers')");



            }
        }
    }
}
