using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TrollRoom;

namespace TrollRoomTests
{
    [TestFixture]
    public class GeneticLayoutGeneratorTests
    {
        private Map map;
        private GeneticLayoutGenerator generator;

        [SetUp]
        public void Setup()
        {
            map = new Map();
        }

        [Test]
        public void Test_All_Directions()
        {
            var roomA = new Room { Id = 0, Name = "A", Exits = new List<Exit>() };
            var roomB = new Room { Id = 1, Name = "B", Exits = new List<Exit>() };
            var roomC = new Room { Id = 2, Name = "C", Exits = new List<Exit>() };
            var roomD = new Room { Id = 3, Name = "D", Exits = new List<Exit>() };
            var roomE = new Room { Id = 4, Name = "E", Exits = new List<Exit>() };
            var roomF = new Room { Id = 5, Name = "F", Exits = new List<Exit>() };
            var roomG = new Room { Id = 6, Name = "G", Exits = new List<Exit>() };
            var roomH = new Room { Id = 7, Name = "H", Exits = new List<Exit>() };
            var roomI = new Room { Id = 8, Name = "I", Exits = new List<Exit>() };
            var roomJ = new Room { Id = 9, Name = "J", Exits = new List<Exit>() };

            map.Rooms.Add(roomA.Id, roomA);
            map.Rooms.Add(roomB.Id, roomB);
            map.Rooms.Add(roomC.Id, roomC);
            map.Rooms.Add(roomD.Id, roomD);
            map.Rooms.Add(roomE.Id, roomE);
            map.Rooms.Add(roomF.Id, roomF);
            map.Rooms.Add(roomG.Id, roomG);
            map.Rooms.Add(roomH.Id, roomH);
            map.Rooms.Add(roomI.Id, roomI);
            map.Rooms.Add(roomJ.Id, roomJ);

            roomA.Exits.Add(new Exit { Destination = roomB, Direction = Direction.North });
            roomB.Exits.Add(new Exit { Destination = roomC, Direction = Direction.Northeast });
            roomC.Exits.Add(new Exit { Destination = roomD, Direction = Direction.East });
            roomD.Exits.Add(new Exit { Destination = roomE, Direction = Direction.Southeast });
            roomE.Exits.Add(new Exit { Destination = roomF, Direction = Direction.South });
            roomF.Exits.Add(new Exit { Destination = roomG, Direction = Direction.Southwest });
            roomG.Exits.Add(new Exit { Destination = roomH, Direction = Direction.West });
            roomH.Exits.Add(new Exit { Destination = roomA, Direction = Direction.Northwest });
            roomA.Exits.Add(new Exit { Destination = roomI, Direction = Direction.Up });
            roomH.Exits.Add(new Exit { Destination = roomJ, Direction = Direction.Down });

            //  . C D .
            //  B I . E
            //  A . . F
            //  . H G .
            //  J . . .
            generator = new GeneticLayoutGenerator(map);
            var bestLayout = generator.GenerateLayout();
        }
    }
}
