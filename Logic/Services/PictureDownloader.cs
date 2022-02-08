using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace Logic.Services
{
    public class PictureDownloader
    {
        public async Task<string> InitiateDownload(string uri, string address)
        {
            HttpClient client = new();
            try
            {
                string path = Path.Combine("c://TegningHenter", address + DateTime.Now.ToString().Replace(":", "") + ".png");
                Directory.CreateDirectory("c://TegningHenter");

                Uri newuri = new Uri(uri);
                var result = await client.GetByteArrayAsync(new Uri(uri));

                await File.WriteAllBytesAsync(path, result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
    }
}
