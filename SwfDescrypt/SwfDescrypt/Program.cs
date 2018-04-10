using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SevenZip;

namespace SwfDescrypt
{
    class Program
    {
        static PacketReader2 packet;
        static byte[] key;

        public static int BUFFER_SIZE = 4096;

        static void Main(string[] args)
        {
            Console.Title = "Descrypt";
            string ndir = AppDomain.CurrentDomain.BaseDirectory;

            if (args.Length > 0)
            {

                Console.WriteLine("l {0}", args[0]);

                string filename = Path.GetFileNameWithoutExtension(args[0]);
                string type = Path.GetExtension(args[0]);
                byte[] data = File.getBytes(args[0]);     

                Console.WriteLine("type: {0}", type);
                if (type.Equals(".png"))
                {
                    string namef = "img-PNG32";
                    string filex = filename.Replace(".swf", "");
                    filex = filex + ".swf";
                    PacketWriter2 writer2 = new PacketWriter2();
                    writer2.WriteI((short)namef.Length);
                    writer2.WriteASCIIFixed(namef, namef.Length);
                    writer2.Write(data, 0, data.Length);
                    Encoder encoder = new Encoder();
                    byte[] enc = encoder.encrypt(writer2.ToArray());
                    ByteArrayToFile(ndir + filex, enc);
                }
                return;
            }


            string[] array2 = Directory.GetFiles(ndir, "*.swf");
            foreach (var dir in array2)
            {
                string filename = Path.GetFileName(dir);
                byte[] data = File.getBytes(dir);

                Encoder encoder = new Encoder();
                byte[] data_de = encoder.decrypt(data);

                if (filename.Equals("90011.swf"))
                {
                    Stream stream = new MemoryStream(data_de);
                    LzmaDecode lzmaDecode = new LzmaDecode(stream);
                    byte[] data_f = ReadFully(lzmaDecode);
                    //packet = new PacketReader2(data_f, data_f.Length);
                    Console.WriteLine("l: {0}", data_f.Length);
                    ByteArrayToFile(ndir + filename + ".bin", data_f);
                    Console.WriteLine("Compelte");
                    Console.ReadKey();
                    return;
                }

                Encoding enc = Encoding.ASCII;
                packet = new PacketReader2(data_de, data_de.Length);
                packet.ReadByte();
                int nlen = packet.ReadByte();
                string nname = enc.GetString(packet.ReadBytes(nlen));
                string nf = nname.Split('-')[0];

                /*Utils.HexDump(data_de, "");
                Console.ReadKey();*/

                if (nf == "img")
                {
                    if (!Directory.Exists(ndir + "/res/img/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/img/");
                    }
                    ByteArrayToFile(ndir + "/res/img/" + Path.GetFileName(dir) + ".png", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "modle")
                {
                    if (!Directory.Exists(ndir + "/res/modle/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/modle/");
                    }
                    ByteArrayToFile(ndir + "/res/modle/" + Path.GetFileName(dir) + ".bin", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "script") {
                    if (!Directory.Exists(ndir + "/res/script/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/script/");
                    }
                    ByteArrayToFile(ndir + "/res/script/" + Path.GetFileName(dir) + ".script", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "ani")
                {
                    if (!Directory.Exists(ndir + "/res/ani/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/ani/");
                    }
                    ByteArrayToFile(ndir + "/res/ani/" + Path.GetFileName(dir) + ".ani", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "texture")
                {
                    if (!Directory.Exists(ndir + "/res/texture/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/texture/");
                    }
                    ByteArrayToFile(ndir + "/res/texture/" + Path.GetFileName(dir) + ".texture", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "map")
                {
                    if (!Directory.Exists(ndir + "/res/map/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/map/");
                    }
                    ByteArrayToFile(ndir + "/res/map/" + Path.GetFileName(dir) + ".map", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "sound")
                {
                    if (!Directory.Exists(ndir + "/res/sound/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/sound/");
                    }
                    ByteArrayToFile(ndir + "/res/sound/" + Path.GetFileName(dir) + ".sound", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nlen < 20)
                {
                    Console.WriteLine("dir: {0}", dir);
                    Console.WriteLine("nlen: {0}", nlen);
                    Console.WriteLine("nname: {0}", nname);
                    Console.WriteLine("nf: {0}", nf);
                }
            }

            //string nname = enc.GetString(packet.ReadBytes(nlen));
            //Console.WriteLine("nname: {0}", nname);

            

            //Utils.HexDump(Decompress(_loc3_), "");


            /*try
            {
                Utils.HexDump(Decompress(_loc3_), "");
            } catch(Exception e)
            {
                Console.WriteLine("unpack");
            }*/


            //ByteArrayToFile("game222.swf", _loc3_);*/


            /*String file = "game222.swf";
            byte[] data = File.getBytes(file);
            packet = new PacketReader2(data, data.Length);
            int cfiles = (int)packet.ReadInt16();
            int counter = 0;

            Console.WriteLine("files: {0}", cfiles);

            while(counter < cfiles)
            {
                int namestr = (int)packet.ReadByte();
                Console.WriteLine("namestr: {0}", namestr);
                string name = packet.ReadString(namestr);
                Console.WriteLine("file: {0}", name);
                byte[] b = packet.ReadBytes(4);
                Array.Reverse(b);
                int size = BitConverter.ToInt32(b, 0);
                Console.WriteLine("size: {0}", size);
                byte[] tmp_data = packet.ReadBytes(size);
                ByteArrayToFile(name, tmp_data);
                packet.ReadByte();
                counter++;
            }*/



            //Console.ReadKey();
        }


        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        static int ReverseBytes(int val)
        {
            byte[] intAsBytes = BitConverter.GetBytes(val);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }
    }
}
