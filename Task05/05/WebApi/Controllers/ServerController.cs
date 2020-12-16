using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contracts;
using RecognitionLibrary;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        [HttpPost]
        public List<SinglePrediction> Post([FromBody] string selected_dir, CancellationToken ct)
        {
            List<SinglePrediction> all_pred = new List<SinglePrediction>();
            var mdl = new Model("C:/test_mdl/resnet50-v2-7.onnx", selected_dir, ct);

            mdl.Work();
            
            using var db = new MyResultContext();
            foreach (var single_img_path in Directory.GetFiles(selected_dir, "*.jpg"))
            {
                var single_pred = db.Results.Where(x => x.Path == single_img_path).Select(x => new
                SinglePrediction()
                {
                    Path = x.Path,
                    Confidence = x.Confidence,
                    Label = x.Label,
                    Image = Convert.ToBase64String(x.Detail.RawImg)
                }).ToList();
                if (single_pred.Count == 0)
                {
                    Console.WriteLine("continue");
                    continue;
                }
                //Console.WriteLine(single_img_path + " " + single_pred.First().Label);
                
                SinglePrediction current_pred = single_pred.First();
                all_pred.Add(single_pred.First());
            }
            return all_pred;
        }

        
        [HttpPost("single_img")]
        public SinglePrediction PostSingleImg([FromBody] string base64string)
        {
            Console.WriteLine("process single image");
            
            var mdl = new Model("C:/test_mdl/resnet50-v2-7.onnx", null);


            PredictionResult res = mdl.WorkSingleImg(base64string);
            SinglePrediction single_pred = new SinglePrediction()
            {
                Path = res.Path,
                Confidence = res.Confidence,
                Label = res.Label,
                Image = base64string
            };

            return single_pred;
        }


        [HttpGet("dbstats")]
        public string GetJson()
        {

            List<string> all_res = new List<string>();
            using (var db = new MyResultContext())
            {
                try
                {
                    foreach (var single_res in db.Results.ToList())
                    {
                        all_res.Add(single_res.Label + ":" + single_res.CountReffered);
                    }
                }
                catch (Exception exc)
                {
                    string a = Directory.GetCurrentDirectory();
                    Console.WriteLine(a);
                    Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                }
            };
            return JsonConvert.SerializeObject(all_res);
        }
        [HttpGet("display_all")]
        public string display_all()
        {
            Console.WriteLine("display all");
            var all_res = new List<SinglePrediction>();
            using var db = new MyResultContext();
            try
            {
                var pred = db.Results.Select(x => new
                SinglePrediction()
                {
                    Path = x.Path,
                    Confidence = x.Confidence,
                    Label = x.Label,
                    Image = Convert.ToBase64String(x.Detail.RawImg)
                }).ToList();
                all_res = pred;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            return JsonConvert.SerializeObject(all_res);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {

            List<string> all_res = new List<string>();
            using (var db = new MyResultContext())
            {
                try
                {
                    foreach (var single_res in db.Results.ToList())
                    {
                        all_res.Add(single_res.Label + ":" + single_res.CountReffered);
                    }
                }
                catch (Exception exc)
                {
                    string a = Directory.GetCurrentDirectory();
                    Console.WriteLine(a);
                    Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                }
            };
            return all_res;
        }


        [HttpDelete]
        public void Delete()
        {
            using (var db = new MyResultContext())
            {
                try
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
                catch (Exception)
                {
                    // dummy
                }
            }
        }
    }
}
