using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Song.Runtime.Module
{
    public class InitModule : MonoBehaviour
    {
        public void Start()
        {
            // using (var wc = new WebClient())
            // {
            //     var data = wc.DownloadString("http://60.205.145.251/");
            // }
        }

        #region zip

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="source">未压缩的文件路径</param>
        /// <param name="target">压缩后数据存放的文件路径</param>
        private static void Compress(string source, string target)
        {
            //1.读取源文件（读取到源文件是未压缩的）
            using (FileStream fsRead = new FileStream(source, FileMode.OpenOrCreate, FileAccess.Read))
            {
                //要将读取到的内容压缩，就是要写入到一个新的文件；所以创建一个新的写入文件的文件流
                using (FileStream fsWrite = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    //因为在写入的时候要压缩后写入，所以需要创建压缩流来写入(因此在压缩写入前需要先创建写入流)
                    //压缩的时候就是要将压缩好的数据写入到指定流中，通过fsWrite写入到新的路径下
                    using (GZipStream zip = new GZipStream(fsWrite, CompressionMode.Compress))
                    {
                        //循环读取，每次从fsRead读取一部分，压缩就写入一部分
                        byte[] buffer = new byte[1024 * 1024 * 3];
                        //读取流每次读取buffer数组的大小
                        int r = fsRead.Read(buffer, 0, buffer.Length);
                        while (r > 0)
                        {
                            //写入，用压缩流来写入，这样写入的才是压缩后的数据
                            //压缩流要从 读取流中读取到的文件数据的数组 中调用Write方法将压缩字节写入基础流(fsWrite)
                            zip.Write(buffer, 0, r); //从读取到内容的数组中读取0到实际读取到的字节数进行压缩
                            //继续从未压缩的文件中读取数据
                            r = fsRead.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="source">被解压的文件</param>
        /// <param name="target">解压后数据存放的路径</param>
        public static void DeCompress(string source, string target)
        {
            //将要被解压的文件读取进来
            using (FileStream fsRead = new FileStream(source, FileMode.OpenOrCreate, FileAccess.Read))
            {
                //创建压缩流，根据fsRead读取到压缩内容进行解压（通过deZip读取，读取到的就是解压后的内容）
                using (GZipStream deZip = new GZipStream(fsRead, CompressionMode.Decompress))
                {
                    //要解压到一个新的文件中，所以需要一个写入文件的文件流
                    using (FileStream fsWrite = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024 * 1024 * 3];
                        //注：读取的时候是通过压缩流来读取，这样读取到的才是解压后的内容
                        int r = deZip.Read(buffer, 0, buffer.Length); //deZip从fsRead流中读取数据解压到buffer数组中
                        while (r > 0)
                        {
                            //通过fsWrite流将存放解压后数据的buffer写入到指定路径下
                            fsWrite.Write(buffer, 0, r);
                            //循环写入
                            r = deZip.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
