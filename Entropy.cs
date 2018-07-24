using System.IO;
using System.Collections.Generic;

namespace Tumultu_
{
    class Entropy
    {
        public readonly int SampleCount = 256;  //default number of samples
        public readonly long FileSizeBytes;

        private FileInfo file;
        private string fileName;
        private string filePath;
        private long sampleSizeBytes;

        public List<double> EntropyList { get; private set; }

        /// <summary>
        ///     Basic constructor for "Entropy" class.
        /// </summary>
        /// 
        /// <param name="f">
        ///     Handle to a file
        /// </param>
        /// 
        /// <param name="sampleCount">
        ///     We want to divde file to this amount of chunks
        /// </param>
        public Entropy(FileInfo f, int sampleCount = 256)
        {
            file = f;

            FileSizeBytes = file.Length;
            fileName = file.Name;
            filePath = file.FullName;
            SampleCount = sampleCount;
            sampleSizeBytes = FileSizeBytes / SampleCount;
            GetEntropy();
        }

        /// <summary>
        ///     Count entropy of single sequence of bytes in a file
        /// </summary>
        /// <param name="fs">
        ///     Handle to a file
        /// </param>
        /// <returns>
        ///     Entropy of a single chunk of a file
        /// </returns>
        private double countSingleSampleEntropy(FileStream fs)
        {
            int[] arr = new int[256];
            
            for (long i = 0; i < sampleSizeBytes; i++) arr[fs.ReadByte()]++;
           
                

            double ent = 0.0;
            foreach(int x in arr)
            {
                ent += ((double)x * x) / sampleSizeBytes;
            }
            ent = 1 - (ent / sampleSizeBytes);

            return ent * ent;
        }   

        /// <summary>
        ///     Counts whole file entropy and saves result to 'entropyList'.
        /// </summary>
        private void GetEntropy()
        {
            EntropyList = new List<double>();
            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            for(int i = 0; i < SampleCount; i++)  
                EntropyList.Add(countSingleSampleEntropy(fs));
            
            fs.Dispose();
            fs.Close();
        }
    }
}