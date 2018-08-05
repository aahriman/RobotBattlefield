using System.IO;
using ViewerLibrary.serializer;

namespace ViewerLibrary.model {
    /// <summary>
    /// This model work for file stored history of match.
    /// </summary>
    public class FileTurnDataModel : IReversibleTurnDataModel {
        private const int DEFAULT_CASH_LENGTH = 10;

        private StreamReader stream;

        private readonly string FILE;

        private readonly Turn[] turns;

        private int turnsIndex = -1;

        private int turn;

        private int fileTurn;

        private readonly object LOCK = new object();

        /// <inheritdoc />
        public FileTurnDataModel(string file) : this(file, DEFAULT_CASH_LENGTH) { 
        }

        /// <summary>
        /// Create instance with can read data from file.
        /// </summary>
        /// <param name="file">File path to it will be possible to open it and then draw.</param>
        /// <param name="cacheLength">For better working it is need to have some cache. Higher number means more needed space but faster running. Recommended value is 10 or more.</param>
        public FileTurnDataModel(string file, int cacheLength) {
            this.FILE = file;
            this.stream = File.OpenText(FILE);
            turns = new Turn[cacheLength];
            readTurns(0);
        }

        /// <inheritdoc />
        public bool HasNext() {
            if (turnsIndex + 1 < turns.Length) {
                return turns[turnsIndex + 1] != null;
            } else {
                readTurns(turn + 1);
                turnsIndex = 0;
                return turns[turnsIndex] != null;
            }
        }

        /// <inheritdoc />
        public bool HasPrevious() {
            return turn > 0;
        }

        /// <inheritdoc />
        public Turn Next() {
            turn++;
            if (turnsIndex + 1 < turns.Length) {
                turnsIndex++;
            } else {
                readTurns(turn);
                turnsIndex = 0;
            }
            return turns[turnsIndex];
        }

        /// <inheritdoc />
        public Turn Previous() {
            turn--;
            if (turn == 0) {
                turnsIndex = -1;
                return null;
            }
            if (turnsIndex > 0) {
                turnsIndex--;
            } else {
                int startAtTurn = turn - (turns.Length - 1) / 2;
                if (startAtTurn > 0) {
                    readTurns(startAtTurn);
                    turnsIndex = (turns.Length - 1) / 2;
                } else {
                    readTurns(0);
                    turnsIndex = (startAtTurn + (turns.Length - 1) / 2) - 1;
                }
            }
            return turns[turnsIndex];
        }

        /// <summary>
        /// Reset data. So data will be returned from beginning.
        /// </summary>
        public void Reset() {
            this.stream = File.OpenText(FILE);
            fileTurn = 0;
            turn = 0;
            turnsIndex = -1;
            turns[0] = null;
            readTurns(0);
        }

        /// <summary>
        /// Close file.
        /// </summary>
        public void Close() {
            stream?.Close();
            stream = null;
        }

        private void readTurns(int startAtTurn) {
            lock (LOCK) {
                if (startAtTurn < fileTurn) {
                    // reset Stream
                    this.stream = File.OpenText(FILE);
                    fileTurn = 0;
                }
                while (fileTurn + 1 < startAtTurn) {
                    // skip lines until read line with turns
                    stream.ReadLine();
                    fileTurn++;
                }
                JSONSerializer serializer = new JSONSerializer();

                int i = 0;
                for (i = 0; i < turns.Length; i++) {
                    string line = stream.ReadLine();
                    if (line != null) {
                        Turn turn = serializer.Deserialize(line);
                        turns[i] = turn;
                        fileTurn++;
                    } else {
                        break;
                    }
                }
                for (; i < turns.Length; i++) {
                    turns[i] = null;
                }
            }
        }
    }
}
