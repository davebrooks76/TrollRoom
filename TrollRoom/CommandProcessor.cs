using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrollRoom
{
    interface ICommandProcessor
    {
        public string Command;
        public void ProcessCommand(string commandSection);
    }
}
