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

		public RoomReader (MainGame mg, Castle castle, string csvName)
		{
			NumCols = 0;
			NumRows = 0;

			// Get Size of Castle
			using (var stream = TitleContainer.OpenStream(csvName))
			using (var reader = new StreamReader(csvName))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var cells = line.Split(',');
					NumRows = cells.Length;
					NumCols++;
				}
			}

			Rooms = new Room[NumCols, NumRows];

			// Initialize all Rooms
			using (var stream = TitleContainer.OpenStream(csvName))
			using (var reader = new StreamReader(csvName))
			{
				var line = reader.ReadLine();
				var roomList = line.Split(',');

				int row = 0;
				int col = 0;

				foreach(string s in roomList)
				{
					var cells = s.Split(':');

					if (cells[0] == "0") continue;

					for(int i = 0; i < cells.Length; i++)
					{
						switch(i) 
						{
							case 0:
								Room r = new Room(mg, castle, cells[i]);

								if(cells[i] == "StartRoom")
								{
									this.StartRoom = r;
								}

								Rooms[col, row] = r;
								break;
							case 1: // Entities
								if (Rooms[col, row] == null) break;
								Rooms[col, row].AddObjectsFromCsv(cells[i]);
								break;

						}
					}

					col++;

				}

				col = 0;
				row++;

			}


			for(int i = 0; i < NumCols; i++)
			{
				for(int j = 0; j < NumRows; j++)
				{
					if (Rooms[i, j] == null) continue;

					Room r = Rooms[i, j];

					if(i - 1 > 0)
					{
						r.LeftRoom = Rooms[i - 1, j];
					}

					if(i + 1 < NumCols)
					{
						r.RightRoom = Rooms[i + 1, j];
					}

					if(j - 1 > 0)
					{
						r.UpRoom = Rooms[i, j - 1];
					}

					if(j + 1 < NumRows)
					{
						r.DownRoom = Rooms[i, j + 1];
					}
				}
			}

		}
	}
}

