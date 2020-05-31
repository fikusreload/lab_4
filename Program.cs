using System;
using System.IO;
using System.Linq;

namespace labOOP2
{
    class Runtime
    {
        static void Main(string[] args)
        {
            MenuHandler menuHandler = new MenuHandler();
            Console.WriteLine("Welcome to DB manager");
            menuHandler.OpenMainMenu();
        }
    }
    class DatabaseHandler
    {
        public void CreateDB(String dbFilename)
        {
            File.Create(dbFilename).Close();
        }
        public void DeleteDB(String dbFilename)
        {
            File.Delete(dbFilename);
        }
        public void AddDBEntry(String dbFilename, String surname, String dealingDate, String storageTerm, String inventoryNumber, String itemName)
        {
            String entryToWrite = surname + " " + dealingDate + " " + storageTerm + " " + inventoryNumber + " " + itemName + "\n";
            File.AppendAllText(dbFilename, entryToWrite);
        }
        public String GetDBEntry(String dbFilename, int entryNumber)
        {
            String[] dbEntriesArray = File.ReadAllLines(dbFilename);
            return dbEntriesArray[entryNumber];
        }
        public void RemoveDBEntry(String dbFilename, int entryNumber)
        {
            String[] dbEntriesArray = File.ReadAllLines(dbFilename);
            String entryToRemove = dbEntriesArray[entryNumber-1];
            dbEntriesArray = dbEntriesArray.Where(val => val != entryToRemove).ToArray();
            File.WriteAllLines(dbFilename, dbEntriesArray);
        }
        public String[] GetFullDBContent(String dbFilename)
        {
            return File.ReadAllLines(dbFilename);
        }
        public void GetDBList()
        {
            String[] databases = Directory.GetFiles(".", "*.txt", SearchOption.TopDirectoryOnly);
            int databaseNumber = 0;
            foreach(String database in databases)
            {
                databaseNumber++;
                Console.WriteLine(databaseNumber + "." + " " + database);
            }
        }
    }
    class MenuHandler
    {
        ConsoleKeyInfo keyInfo;
        public static DatabaseHandler databaseHandler = new DatabaseHandler();
        public static String dbFilename;
        public void OpenMainMenu()
        {
            Console.WriteLine();
            DatabaseHandler databaseHandler = new DatabaseHandler();
            ConsoleKeyInfo keyInfo;
            Console.WriteLine("Create a new database - n");
            Console.WriteLine("Edit database - e");
            Console.WriteLine("Delete database - d");
            Console.WriteLine("List databases - l");
            Console.WriteLine("Here is a list of databases available");
            databaseHandler.GetDBList();
            do
            {
                keyInfo = Console.ReadKey();
            }
            while (keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.E && keyInfo.Key != ConsoleKey.D && keyInfo.Key != ConsoleKey.L);
            if(keyInfo.Key == ConsoleKey.N)
            {
                CreateNewDatabase();
            }
            else if(keyInfo.Key == ConsoleKey.E)
            {
                EditDatabase();
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                DeleteDatabase();
            }
        }
        public void CreateNewDatabase()
        {
            Console.Write("Please enter database filename: ");
            MenuHandler.dbFilename = Console.ReadLine() + ".txt";
            databaseHandler.CreateDB(MenuHandler.dbFilename);
            OpenMainMenu();
        }
        public void EditDatabase()
        {
            Console.Write("Please specify database name(with .txt extension): ");
            MenuHandler.dbFilename = Console.ReadLine();
            if(File.Exists(MenuHandler.dbFilename))
            {
                Console.WriteLine("Add database entry - a");
                Console.WriteLine("Remove database entry - r");
                Console.WriteLine("Get current database content - v");
                Console.WriteLine("Sort database content - s");
                do
                {
                    keyInfo = Console.ReadKey();
                }
                while(keyInfo.Key != ConsoleKey.A && keyInfo.Key != ConsoleKey.R && keyInfo.Key != ConsoleKey.V);
                if(keyInfo.Key == ConsoleKey.A)
                {
                    AddEntryToDatabase();
                }
                else if(keyInfo.Key == ConsoleKey.R)
                {
                    RemoveEntryFromDatabase();
                }
                else if(keyInfo.Key == ConsoleKey.V)
               {
                   ViewFullDatabaseContent();
               }
               else if(keyInfo.Key == ConsoleKey.S)
               {
                   SortDBRecords();
               }
            }
            else
            {
                Console.WriteLine("Incorrect DB name");
            }
            static void AddEntryToDatabase()
            {
                String surname;
                String dealingDate;
                String storageTerm;
                String inventoryNumber;
                String itemName;
                Console.Write("Please enter Surname: ");
                surname = Console.ReadLine();
                Console.Write("Please enter dealing date: ");
                dealingDate = Console.ReadLine();
                Console.Write("Please define storing time: ");
                storageTerm = Console.ReadLine();
                Console.Write("Please specify inventory number: ");
                inventoryNumber = Console.ReadLine();
                Console.Write("Please specify item name: ");
                itemName = Console.ReadLine();
                databaseHandler.AddDBEntry(dbFilename, surname, dealingDate, storageTerm, inventoryNumber, itemName);
            }
            static void RemoveEntryFromDatabase()
            {
                String[] fullDBContent = databaseHandler.GetFullDBContent(MenuHandler.dbFilename);
                int entryNumber;
                int i = 0;
                foreach(String entry in fullDBContent)
                {
                    i++;
                    Console.WriteLine(i.ToString() + "." + " " + entry);
                }
                Console.Write("Please specify entry number you want to remove: ");
                entryNumber = Int32.Parse(Console.ReadLine());
                databaseHandler.RemoveDBEntry(MenuHandler.dbFilename, entryNumber);
                fullDBContent = databaseHandler.GetFullDBContent(dbFilename);
                foreach(String entry in fullDBContent)
                {
                    Console.WriteLine(entry);
                }    
            }
            static void ViewFullDatabaseContent()
            {
                String[] fullDBContent = databaseHandler.GetFullDBContent(dbFilename);
                foreach(String record in fullDBContent)
                {
                    Console.WriteLine(record);
                }
            }
            OpenMainMenu();
        }
        public void DeleteDatabase()
        {
            databaseHandler.GetDBList();
            Console.Write("Please specify database name, you want to delete: ");
            MenuHandler.dbFilename = Console.ReadLine();
            databaseHandler.DeleteDB(MenuHandler.dbFilename);
            OpenMainMenu();
        }
        public void ListDatabases()
        {
            databaseHandler.GetDBList();
            OpenMainMenu();
        }
        public void SortDBRecords()
        {
            String[] dbContent = File.ReadAllLines(dbFilename);
            dbContent.OrderBy(a => a).ToArray();
            File.WriteAllLines(dbFilename, dbContent);
            OpenMainMenu();
        }
    }
}