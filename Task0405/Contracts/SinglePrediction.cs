using System;

namespace Contracts
{
    public class SinglePrediction
    {
        public string Path { get; set; }
        public string Label { get; set; }
        public double Confidence { get; set; }
        public string Image { get; set; }
    }
}
