using RecognitionLibrary;
using System;
using System.Linq;


namespace ImageRecognition
{
    class Program

    {
        public static void my_output_function(string to_out)
        {
            System.Console.WriteLine(to_out);
        }
        static void Main(string[] args)
        {
            

            string imgPath = args.FirstOrDefault() ?? "./../../../../dataset/";
            string modelFilePath = "./../../../../resnet50-v2-7.onnx";

            Model model = new Model(my_output_function, modelFilePath, imgPath);
            
            Console.CancelKeyPress += (sender, eArgs) => {
                model.Stop();
                eArgs.Cancel = true;
            };

            model.Work();
        }
    }
}

