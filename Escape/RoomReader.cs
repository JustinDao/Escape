using System;
using Microsoft.Xna.Framework;
using System.IO;

namespace Escape
{
    class RoomReader
    {
        public int NumCols;
        public int NumRows;
        public Room[,] Rooms;
        public Room StartRoom;

        public RoomReader(MainGame mg, Castle castle, string csvName)
        {
            NumCols = 0;
            NumRows = 0;

            var path = @"Content\\" + csvName;

            // Get Size of Castle
            using (var stream = TitleContainer.OpenStream(path))
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var cells = line.Split(',');
                    NumCols = cells.Length;
                    NumRows++;
                }
            }

            Rooms = new Room[NumCols, NumRows];

            // Initialize all Rooms
            using (var stream = TitleContainer.OpenStream(path))
            using (var reader = new StreamReader(path))
            {
                int row = 0;
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var roomList = line.Split(',');

                    int col = 0;

                    foreach (string s in roomList)
                    {
                        var cells = s.Split(':');

                        if (cells[0] != "0")
                        {

                            for (int i = 0; i < cells.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        Room r = new Room(mg, castle, cells[i] + ".csv");

                                        if (cells[i] == "StartRoom")
                                        {
                                            this.StartRoom = r;
                                        }

                                        Rooms[col, row] = r;
                                        break;
                                    case 1: // Entities
                                        if (Rooms[col, row] == null) break;
                                        Rooms[col, row].AddObjectsFromCsv(cells[i] + ".csv");
                                        break;

                                }
                            }
                        }

                        col++;

                    }

                    row++;
                }
            }


            for (int i = 0; i < NumCols; i++)
            {
                for (int j = 0; j < NumRows; j++)
                {
                    Room r = Rooms[i, j];

                    if (r == null) continue;

                    if (i + 1 < NumCols)
                    {
                        Room n = Rooms[i + 1, j];
                        if (n != null)
                        {
                            r.RightRoom = n;
                            n.LeftRoom = r;
                        }
                    }

                    if (j + 1 < NumRows)
                    {
                        Room n = Rooms[i, j + 1];
                        if (n != null)
                        {
                            r.DownRoom = n;
                            n.UpRoom = r;
                        }
                    }
                }
            }

        }
    }
}

