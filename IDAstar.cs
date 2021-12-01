using System.Collections.ObjectModel;
using System.Threading.Channels;

namespace IdaStar {

    public enum CellState {
        EMPTY,
        OBSTACLE,
        START,
        DESTINATION,
        PATH,
    }

    public static class CellStateUtil {
        public static CellState FromInput(this char c) => c switch
        {
            '#' => CellState.OBSTACLE,
            ' ' => CellState.EMPTY,
            'S' => CellState.START,
            'F' => CellState.DESTINATION,
            'P' => CellState.PATH,
            _ => throw new NotImplementedException(),
        };

		public static char ToInput(this CellState state) => state switch
		{
			CellState.EMPTY => ' ',
			CellState.OBSTACLE => '#',
			CellState.START => 'S',
			CellState.DESTINATION => 'F',
            CellState.PATH => 'P',
			_ => throw new NotImplementedException(),
		};

		public static List<List<CellState>> FromInput(this List<List<char>> board) {
			return board.Select((row) => row.Select((c) => FromInput(c)).ToList()).ToList();
		}

        public static List<List<char>> ToInput(this List<List<CellState>> board) {
			return board.Select((row) => row.Select((state) => ToInput(state)).ToList()).ToList();
		}
    }

    public class WorkingBoard {
		readonly List<List<CellState>> _board;

        public WorkingBoard(List<List<CellState>> board) {
			this._board = board;
		}

        public WorkingBoard(List<List<char>> board) {
			this._board = board.FromInput();
		}

        public string Display() {
			return string.Join(
                Environment.NewLine, 
                _board.ToInput().Select((row) => string.Join("", row))
            );
		}

        public ReadOnlyCollection<ReadOnlyCollection<CellState>> Board {
			get => _board.Select((row) => row.AsReadOnly()).ToList().AsReadOnly();
		}

		public event Action<WorkingBoard>? AlgorithmStep;

		public void RunIdaStar() {
			throw new NotImplementedException();
		}
    }

}

/*
    IDA*( state s, int g, threshold t ) {
        h = Eval( s );
        if( h == 0 ) return( true );

        f = g + h;
        if( f > threshold ) return( false );

        for( i = 1; i <= numchildren; i++ ) {
            done = IDA*( s.child[ i ], g + cost( child[ i ] ), t );
            if( done == true ) return( true );
        }
        return( false );
    } 
*/

