using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Context
{
    public class SetData
    {
        public string Name { get; set; }
        public string Aliases { get; set; }
        public int Size { get; set; }
        public string FolderName { get; set; }
        public int YabaiLevel { get; set; }
    }

    public class CosplayerData
    {
        public string Name { get; set; }
        public string Aliases { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Booth { get; set; }
        public string ProfilePicture { get; set; }
        public List<SetData> Sets { get; set; }
    }
}
