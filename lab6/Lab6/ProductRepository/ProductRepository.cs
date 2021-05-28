using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace lab6
{
    public class ProductRepository
    {
        private SqliteConnection connection;
        public ProductRepository(string filePath)
        {
            this.connection = new SqliteConnection($"Data Source = {filePath}");
        }

        // додавання сутності в БД і отримання ідентифікатора доданої сутності
        public long Insert(Product product)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO products (name, description, price, isExist, createdAt)
                VALUES ($name, $description, $price, $isExist, $createdAt);

                SELECT last_insert_rowid();";

            command.Parameters.AddWithValue("$name", product.name);
            command.Parameters.AddWithValue("$description", product.description);
            command.Parameters.AddWithValue("$price", product.price);
            if (product.isExist)
            {
                command.Parameters.AddWithValue("$isExist", "+");
            }
            else
            {
                command.Parameters.AddWithValue("isExist", "-");
            }
            // command.Parameters.AddWithValue("$isExist", product.isExist);
            command.Parameters.AddWithValue("$createdAt", product.createdAt.ToString("o"));

            long newID = (long)command.ExecuteScalar();
            connection.Close();

            return newID;
        }

        // видалення сутності з БД за ідентифікатором із результатом видалення (так-ні)
        public bool DeleteById(long id)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            
            int nChanged = command.ExecuteNonQuery();
            
            connection.Close();
            
            if (nChanged == 0)
            {
                return false;
            }

            return true;
        }

        // оновлення даних сутності в БД із результатом оновлення (так-ні)
        public bool Update(long id, Product product)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE products
                    SET name = $name, description = $description, price = $price, isExist = $isExist, createdAt = $createdAt
                    WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$name", product.name);
            command.Parameters.AddWithValue("$description", product.description);
            command.Parameters.AddWithValue("$price", product.price);
            if (product.isExist)
            {
                command.Parameters.AddWithValue("$isExist", "+");
            }
            else
            {
                command.Parameters.AddWithValue("isExist", "-");
            }
            // command.Parameters.AddWithValue("$isExist", product.isExist);
            command.Parameters.AddWithValue("$createdAt", product.createdAt);

            int nChanged = command.ExecuteNonQuery();
            
            connection.Close();

            if (nChanged == 0)
            {
                return false;
            }
            
            return true;
        }
        
        // отримання загальної кількості сторінок з сутностями (число)
        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        }

        private long GetCount()
        {
            connection.Open();
        
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM products";
            
            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        // отримання колекції сутностей за номером сторінки (нумерація сторінок з 1)
        public List<Product> GetPage(int pageNum, int pageSize)
        {
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNum));
            }

            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumber", pageNum);

            SqliteDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product product = new Product();
                product.id = int.Parse(reader.GetString(0));
                product.name = reader.GetString(1);
                product.description = reader.GetString(2);
                product.price = double.Parse(reader.GetString(3));
                if (reader.GetString(4) == "+")
                {
                    product.isExist = true;
                }
                else
                {
                    product.isExist = false;
                }
                product.createdAt = DateTime.Parse(reader.GetString(5));

                products.Add(product);
            }
            reader.Close();
            connection.Close();

            return products;
        }
    }
}
