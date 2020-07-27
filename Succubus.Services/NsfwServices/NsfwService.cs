using Succubus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Services.NsfwServices
{
    public class NsfwService : IService
    {
        public ImageData GetRandomImage()
        {
            string folderPath = System.IO.Directory.GetCurrentDirectory() + "/bin/Debug/netcoreapp3.0/Images/";
            Random r = new Random();
            int imgNumber = r.Next(0, 2753);

            return new ImageData
            {
                Name = String.Format("{0:0000}", imgNumber),
                Url = folderPath + String.Format("{0:0000}", imgNumber) + ".jpg"
            };
        }
    }

    public class ImageData
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
