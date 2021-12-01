// See https://aka.ms/new-console-template for more information

string[] labyrinthIN = System.IO.File.ReadAllLines(@"C:\UNI\AI\IDAstar\labyrinth.txt");
string[] labyrinthOUT = System.IO.File.ReadAllLines(@"C:\UNI\AI\IDAstar\labyrinthOUT.txt");


System.Console.WriteLine("The input labyrinth: ");
foreach (string line in labyrinthIN)
{
    FormattedLabRow(line);
}

System.Console.WriteLine("The output labyrinth: ");
foreach (string line in labyrinthOUT)
{
    FormattedLabRow(line);
}


void FormattedLabRow(string line) {
    char[] characters = line.ToCharArray();
    foreach (char c in characters) {
        switch ( c ) {
            case '#': {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("  "); 
                break;
            }

            case ' ': {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("  "); 
                break;
            }

            case 'S': {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("  "); 
                break;
            }

            case 'F': {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("  "); 
                break;
            }

            case 'p': {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write("  "); 
                break;
            }

            default: break;
        }
    }
    Console.ResetColor();
    Console.WriteLine();
}