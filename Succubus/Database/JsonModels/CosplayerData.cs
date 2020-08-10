using System.Collections.Generic;

namespace Succubus.Database.JsonModels
{
    public class CosplayerData
    {
        public string Name { get; set; }
        public List<string> Aliases { get; set; }
        public string ProfilePicture { get; set; }

        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Booth { get; set; }
        public List<SetData> Sets { get; set; }
    }
}