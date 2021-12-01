using System.Runtime.Versioning;

using System.Collections.ObjectModel;
using System.Collections;

[assembly: RequiresPreviewFeatures()]

namespace IdaStar
{

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

	public record struct Point(int Row, int Column) {
        public int ManhattanDistance(Point otherPoint) {
			return Math.Abs(Row - otherPoint.Row) + Math.Abs(Column - otherPoint.Column);
		}

        [Flags]
        public enum NDirections : byte {
            N  = 0b0000_0001,
            E  = 0b0000_0010,
            S  = 0b0000_0100,
            W  = 0b0000_1000,
            NE = 0b0001_0000,
            SE = 0b0010_0000,
            SW = 0b0100_0000,
            NW = 0b1000_0000,
        }

        public List<Point> GetNeighbours(byte directions = 0b1111) {
			List<Point> result = new();

            if ((directions & ((byte)NDirections.N)) > 0) {
				result.Add(new(Row - 1, Column));
			}
            if ((directions & ((byte)NDirections.E)) > 0) {
				result.Add(new(Row, Column + 1));
			}
            if ((directions & ((byte)NDirections.S)) > 0) {
				result.Add(new(Row + 1, Column));
			}
            if ((directions & ((byte)NDirections.W)) > 0) {
				result.Add(new(Row, Column - 1));
			}
            if ((directions & ((byte)NDirections.NE)) > 0) {
				result.Add(new(Row - 1, Column + 1));
			}
            if ((directions & ((byte)NDirections.SE)) > 0) {
				result.Add(new(Row + 1, Column + 1));
			}
            if ((directions & ((byte)NDirections.SW)) > 0) {
				result.Add(new(Row + 1, Column - 1));
			}
            if ((directions & ((byte)NDirections.NW)) > 0) {
				result.Add(new(Row - 1, Column - 1));
			}

			return result;
		}

		public bool IsInsideBox(int Height, int Width) => IsInsideBox(new(Height - 1, Width - 1));
		public bool IsInsideBox(Point bottomRight) => IsInsideBox(new(0, 0), bottomRight);
		public bool IsInsideBox(Point topLeft, Point bottomRight) {
			return topLeft.Row <= Row && topLeft.Column <= Column &&
				Row <= bottomRight.Row && Column <= bottomRight.Column;
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

        /// <summary>
		/// Clean board, making it ready for a new run.
        /// 
		/// This is achieved by setting all PATH states to EMPTY states.
		/// </summary>
        public void Reset() {
            foreach (var row in _board) {
				for (var i = 0; i < row.Count; i++) {
                    if (row[i] == CellState.PATH) {
						row[i] = CellState.EMPTY;
					}
				}
			}
        }

		public void RunIdaStar() => RunIdaStar(0, 1, (p1, p2) => p1.ManhattanDistance(p2));
		public void RunIdaStar<Num>(Num zero, Num increment, Func<Point, Point, Num> heuristic) where Num: INumber<Num> {
			// Don't run algorithm on a "dirty" board
			// "dirty" = the algorithm was already ran before
			if (_board.Select((row) => row.Where((state) => state == CellState.PATH).Count()).Any((cnt) => cnt > 0)) {
				throw new DirtyBoardException();
			}

            Point findPoint(CellState neededState) {
                for (var i = 0; i < _board.Count; i++) {
                    for (var j = 0; j < _board[i].Count; j++) {
                        if (_board[i][j] == CellState.START) {
                            return new(i, j);
                        }
                    }
                }
                throw new NoPoint(neededState);
            }

			Point startPoint = findPoint(CellState.START);

			Point destinationPoint = findPoint(CellState.DESTINATION);

            Num search(Point current, Num cost, Num threshold) {
				var h = heuristic(current, destinationPoint);
                if (h == zero) {
					return h;
				}
				var f = cost + h;
                if (f > threshold) {
					return f;
				}
				var min = f;

                foreach(var neighbour in current.GetNeighbours()) {
					if (!neighbour.IsInsideBox(_board.Count, _board[0].Count)) {
						continue;
					}
					var neighbourF = search(neighbour, cost + increment, threshold);
                    if (neighbourF < min) {
						min = neighbourF;
					}
                    if (min == zero) {
						break;
					}
				}

				return min;
			}

			var threshold = heuristic(startPoint, destinationPoint);
            while (threshold == zero) {
				threshold = search(startPoint, zero, threshold);
			}
		}
    }

    [System.Serializable]
    public class DirtyBoardException : System.Exception
    {
        public DirtyBoardException() : base("The board is dirty (contains path cells)") { }
        public DirtyBoardException(System.Exception inner) : base("The board is dirty (contains path cells)", inner) { }
        protected DirtyBoardException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class NoPoint : System.Exception
    {
        public NoPoint(CellState neededState) : base($"The board doesn't contain any {neededState} point") { }
        public NoPoint(CellState neededState, System.Exception inner) : base($"The board doesn't contain any {neededState} point", inner) { }
        protected NoPoint(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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

