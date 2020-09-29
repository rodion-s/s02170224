using RecognitionLibrary;
using System.Linq;


namespace ImageRecognition
{
    class Program
    {
        static void Main(string[] args)
        {
            string imgPath = args.FirstOrDefault() ?? "./../../../../dataset/";
            string modelFilePath = "./../../../../resnet50-v2-7.onnx";

            Model model = new Model(modelFilePath, imgPath);
            model.Work();
        }
    }
}

