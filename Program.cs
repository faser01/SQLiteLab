using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace SQLiteLab
{
    internal class Program

    {
        static SQLiteConnection connection;
        static SQLiteCommand command;
        private static string connectionString;

        static public bool Connect(string fileName)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName + ";Version=3; FailIfMissing = False");
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }
        static void Main(string[] args)
        {
            if (Connect("workplace.sqlite"))
            {
                Console.WriteLine("подключено к базе данных!\n");
            }

            command = new SQLiteCommand(connection)
            {
                CommandText = "CREATE TABLE IF NOT EXISTS [workplace]([id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, [имя] VARCHAR (1, 50), [монитор] VARCHAR (1, 50), [клавиатура] VARCHAR (1, 50), [мышь] VARCHAR (1, 50));"
            };
            command.ExecuteNonQuery();
            Console.WriteLine("Таблица создана\n");
            command.CommandText = "INSERT INTO  workplace (имя, монитор, клавиатура, мышь) VALUES "
                                  + "( \"WorkPC001\", \"Disp001\",  \"Keyboard001\", \"Mouse001\" ),"
                                  + "( \"WorkPC002\", \"Disp002\",  \"Keyboard002\", \"Mouse002\" ),"
                                  + "( \"WorkPC003\", \"Disp003\",  \"Keyboard003\", \"Mouse003\" ),"
                                  + "( \"WorkPC004\", \"Disp004\",  \"Keyboard004\", \"Mouse004\" )";
            command.ExecuteNonQuery();

            using (SQLiteCommand selectCMD = connection.CreateCommand())
            {

                selectCMD.CommandText = "SELECT * FROM  workplace";
                selectCMD.CommandType = CommandType.Text;
                SQLiteDataReader myReader = selectCMD.ExecuteReader();
                while (myReader.Read())
                {
                    Console.WriteLine("компьютер = "+ myReader["имя"] + " монитор = " + myReader["монитор"] + " клавиатура = " + myReader["клавиатура"] + " мышь " + myReader["мышь"]);
                }
            
            }
            Console.WriteLine("\nВведите новое имя компьютера:");
             string name = Console.ReadLine();
            string sqlExpression = $"UPDATE  workplace SET имя= '{name}' WHERE id >=1";
            using (var connection = new SQLiteConnection("Data Source=workplace.sqlite"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);

                int number = command.ExecuteNonQuery();

                Console.WriteLine($"Обновлено объектов: {number}");
                using (SQLiteCommand selectCMD = connection.CreateCommand())
                {

                    selectCMD.CommandText = "SELECT * FROM  workplace";
                    selectCMD.CommandType = CommandType.Text;
                    SQLiteDataReader myReader = selectCMD.ExecuteReader();
                    while (myReader.Read())
                    {
                        Console.WriteLine(myReader["имя"] + " " + myReader["монитор"] + " " + myReader["клавиатура"] + " " + myReader["мышь"]);
                    }
                    Console.ReadLine();
                }
                // не стал продолжать
                /* Console.WriteLine("Введите модель монитора:");
                 string display = (Console.ReadLine());

                 Console.WriteLine("Введите модель клавиатуры:");
                 string keyboard = (Console.ReadLine());

                 Console.WriteLine("Введите модель мыши:");
                 string mouse = (Console.ReadLine());*/



                connection.Close();


            }
        }
    }
}
