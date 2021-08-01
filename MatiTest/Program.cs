using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace MatiTest
{
    public class Program
    {
        static readonly HttpClient client = new HttpClient();
        private static Stream stream;

        public static async Task Main(string[] args)
        {
            Upload("61070739b36538001be1d8fa", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnQiOnsiX2lkIjoiNWUzMzU4MGVlNTFmY2MwMDFiOTBhNTQ2IiwibWVyY2hhbnQiOnsiX2lkIjoiNWUzMzU4MGVlNTFmY2MwMDFiOTBhNTQ1Iiwib3duZXIiOiI1ZTMzNTgwZWU1MWZjYzAwMWI5MGE1NDMiLCJibG9ja2VkQXQiOm51bGx9fSwidXNlciI6eyJfaWQiOiI1ZTMzNTgwZWU1MWZjYzAwMWI5MGE1NDMiLCJmaXJzdE5hbWUiOiJDYXJsb3MiLCJsYXN0TmFtZSI6IlNlcHVsdmVkYSJ9LCJzY29wZSI6InZlcmlmaWNhdGlvbl9mbG93IGlkZW50aXR5OnJlYWQgdmVyaWZpY2F0aW9uOnJlYWQiLCJpYXQ");
        }

        private static void Upload(string identity, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer "+token);

                using (var content = new MultipartFormDataContent())
                {
                    var path = @"/Users/carlossepulveda/Downloads/INE_FRONT.jpeg";
                    var path2 = @"/Users/carlossepulveda/Downloads/INE_BACK.jpeg";

                    string assetName = Path.GetFileName(path);
                    string assetName2 = Path.GetFileName(path2);
                    string request = "[{\"inputType\": \"document-photo\",\"group\": 0,\"data\": {\"type\": \"national-id\",\"country\": \"MX\",\"page\": \"front\",\"filename\": \"INE_FRONT.jpeg\"}},{\"inputType\": \"document-photo\",\"group\": 0,\"data\": {\"type\": \"national-id\",\"country\": \"MX\",\"page\": \"back\",\"filename\": \"INE_BACK.jpeg\"}}]";

                    //Content-Disposition: form-data; name="inputs"
                    var stringContent = new StringContent(request);
                    stringContent.Headers.Add("Content-Disposition", "form-data; name=\"inputs\"");
                    content.Add(stringContent, "inputs");

                    //Opening Files
                    FileStream fs = File.OpenRead(path);
                    FileStream fs2 = File.OpenRead(path2);

                    // Setting stream and content type
                    var streamContent = new StreamContent(fs);
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"document\"; filename=\"" + Path.GetFileName(path) + "\"");
                    content.Add(streamContent, "document", Path.GetFileName(path));

                    var streamContent2 = new StreamContent(fs2);
                    streamContent2.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent2.Headers.Add("Content-Disposition", "form-data; name=\"document\"; filename=\"" + Path.GetFileName(path2) + "\"");
                    content.Add(streamContent2, "document", Path.GetFileName(path2));

                    Task<HttpResponseMessage> message = client.PostAsync("https://api.getmati.com/v2/identities/"+identity+"/send-input", content);

                    var input = message.Result.Content.ReadAsStringAsync();
                    Console.WriteLine(input.Result);
                    Console.Read();
                }
            }
        }

    }
}
