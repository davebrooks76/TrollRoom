using System;
using System.Collections;
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
    public class CollisionLayoutScorerTests
    {
        private Map map;
        private ILayoutScorer scorer;
        private Layout layout;

        [SetUp]
        public void Setup()
        {
            map = new Map();
            scorer = new CollisionLayoutScorer();
            layout = new Layout(map);
        }
        [Test]
        public void No_Collisions()
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
            var byteCoordinates = new List<byte>
            {
                0, 0,   //A
                0, 1,   //B
                1, 2,   //C
                2, 2,   //D
                3, 1,   //E
                3, 0,   //F
                2, 3,  //G
                1, 3,  //H
                1, 1,   //I
                0, 3   //J  
            };

            layout.Bits = byteCoordinates.ToArray().ToBitArray();

            var score = scorer.Score(map, layout);
            Assert.AreEqual(1, score);
        }

        [Test]
        public void One_Collision()
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
            var byteCoordinates = new List<byte>
            {
                0, 0,   //A
                0, 1,   //B
                1, 2,   //C
                2, 2,   //D
                3, 1,   //E
                0, 0,   //F
                2, 3,  //G
                1, 3,  //H
                1, 1,   //I
                0, 4   //J  
            };

            layout.Bits = byteCoordinates.ToArray().ToBitArray();

            var score = scorer.Score(map, layout);
            Assert.AreEqual(0.9, score);
        }

        [Test]
        public void Three_Collisions()
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
            var byteCoordinates = new List<byte>
            {
                0, 0,   //A
                0, 1,   //B
                1, 2,   //C
                2, 2,   //D
                3, 1,   //E
                0, 0,   //F
                0, 1,  //G
                1, 2,  //H
                1, 1,   //I
                0, 3   //J  
            };

            layout.Bits = byteCoordinates.ToArray().ToBitArray();

            var score = scorer.Score(map, layout);
            Assert.AreEqual(0.7, score);
        }

        [Test]
        public void One_Collision_With_Four_Rooms()
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
            var byteCoordinates = new List<byte>
            {
                0, 0,   //A
                0, 1,   //B
                1, 2,   //C
                2, 2,   //D
                3, 1,   //E
                0, 0,   //F
                0, 0,  //G
                0, 0,  //H
                0, 0,   //I
                0, 4   //J  
            };

            layout.Bits = byteCoordinates.ToArray().ToBitArray();

            var score = scorer.Score(map, layout);
            Assert.AreEqual(0.6, score);
        }
    }
}
