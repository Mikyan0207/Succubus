using System.Collections.Generic;

namespace Succubus.Database.JsonModels
{
    public class SetData
    {
        public string Name { get; set; }
        public List<string> Aliases { get; set; }
        public int Size { get; set; }
        public string FolderName { get; set; }
        public string FilePrefix { get; set; }
        public int YabaiLevel { get; set; }
    }
}