using Textile_Management_System;

class Program
{



    static void Main(String[] args)

    {
        Console.WriteLine("\t\t--------------------------------------");
        Console.WriteLine("\t\t|       Textile Management System     |");
        Console.WriteLine("\t\t--------------------------------------");

        Console.WriteLine("\n\t\t\t 01. Store Administration");
        Console.WriteLine("\n\t\t\t 03. Exit");


        Store aStore = new Store();

        Console.Write("Enter Your choice :- ");
        if (int.TryParse(Console.ReadLine(), out int x))
        {

            switch (x)
            {
                case 1:
                    aStore = new Store();
                    aStore.MStore();
                    break;
                case 3:
                    Console.WriteLine("\n\n\n\n\t\t\t Thank you, Have a nice Day");
                    break;
                default:
                    Console.WriteLine("\n\n\n\t\t Please Enter the Correct Input");
                    break;
            }
        }

        else
        {
            Console.WriteLine("Invalid input");
        }
        Console.WriteLine("\n\n\n Thank you");
    }
}
