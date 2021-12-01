using IdaStar;

string[] labyrinthIN = System.IO.File.ReadAllLines(@"C:\UNI\AI\IDAstar\labyrinthOUT.txt");


System.Console.WriteLine("The input labyrinth: ");
foreach (string line in labyrinthIN)
{
    FormattedLabRow(line);
}

var algoBoard = new IdaStar.WorkingBoard(labyrinthIN.Select((row) => row.ToList()).ToList());
int step = 0;
bool done = false;
ConsoleColor border = ConsoleColor.Magenta;
algoBoard.AlgorithmStep += (_) => {
    Console.Clear();
    step++;
    if(done){
        System.Console.WriteLine("The solved labyrinth is:");
    }else if(step/2 == 0) {
        System.Console.WriteLine("Computing [• ]");
    }else {
        System.Console.WriteLine("Computing [ •]");
    }

    //top border
    Console.BackgroundColor = border;
    for (var i=0; i<algoBoard.Board[0].Count()+2; i++){
        Console.Write("==="); 
    }
    Console.ResetColor();

    foreach (var line in algoBoard.Board)
    {
        var charlist = line.Select((state) => CellStateUtil.ToInput(state));
        var str = string.Join("", charlist);

        //left border
        Console.BackgroundColor = border;
        Console.Write("|||"); 
        Console.ResetColor();

        //labyrinth line
        FormattedLabRow(str);

        //right border
        Console.BackgroundColor = border;
        Console.Write("|||"); 
        Console.ResetColor();
    }

    //bottom border
    Console.BackgroundColor = border;
    for (var i=0; i<algoBoard.Board[0].Count()+2; i++){
        Console.Write("==="); 
    }
    Console.ResetColor();
};

algoBoard.RunIdaStar();


void FormattedLabRow(string line) {
    char[] characters = line.ToCharArray();
    foreach (char c in characters) {
        switch ( c ) {
            case '#': {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("   "); 
                break;
            }

            case ' ': {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("   "); 
                break;
            }

            case 'S': {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("<•>"); 
                break;
            }

            case 'F': {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("[ ]"); 
                break;
            }

            case 'p': {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" • "); 
                break;
            }

            default: break;
        }
    }
    Console.ResetColor();
    Console.WriteLine();
}