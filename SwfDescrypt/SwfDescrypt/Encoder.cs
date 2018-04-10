using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfDescrypt
{
    public class Encoder
    {
        protected byte[] __key;
        protected byte[] __keydata;
        public Encoder()
        {
            this.__key = UTF8Encoding.UTF8.GetBytes("u._78%,h@*~f&4G+<-JP");
            this.init();
        }

        public void init()
        {
            this.__keydata = new byte[256];
            int counter = 0;
            while ((int)counter < 256)
            {
                this.__keydata[counter] = (byte)counter;
                counter++;
            }
            byte tmp0 = 0;
            int tmp1 = 0;
            int tmp2 = 0;
            int tmpcounter = 0;
            while (tmpcounter < 256)
            {
                tmp0 = (byte)((this.__key[tmp2] & 255) + (this.__keydata[tmpcounter] & 255) + tmp0 & 255);
                tmp1 = this.__keydata[tmpcounter];
                this.__keydata[tmpcounter] = this.__keydata[tmp0];
                this.__keydata[tmp0] = (byte)tmp1;
                tmp2 = (tmp2 + 1) % this.__key.Length;
                tmpcounter++;
            }
        }


        public byte[] decrypt(byte[] data)
        {
            byte[] data_de = new byte[data.Length];
            int counter = 0;
            byte tmp0 = 0;
            int tmp1 = 0;
            byte tmp2 = 0;
            byte tmp3 = 0;
            while (counter < data.Length)
            {
                tmp2 = (byte)(tmp2 + 1 & 255);
                tmp3 = (byte)((this.__keydata[tmp2] & 255) + tmp3 & 255);
                tmp1 = this.__keydata[tmp2];
                this.__keydata[tmp2] = this.__keydata[tmp3];
                this.__keydata[tmp3] = (byte)tmp1;
                tmp0 = (byte)((this.__keydata[tmp2] & 255) + (this.__keydata[tmp3] & 255) & 255);
                data_de[counter] = (byte)(data[counter] ^ this.__keydata[tmp0]);
                counter++;
            }
            return data_de;
        }

        public byte[] encrypt(byte[] data)
        {
            byte[] data_de = new byte[data.Length];
            int counter = 0;
            byte tmp0 = 0;
            int tmp1 = 0;
            byte tmp2 = 0;
            byte tmp3 = 0;
            while (counter < data.Length)
            {
                tmp2 = (byte)(tmp2 + 1 & 255);
                tmp3 = (byte)((this.__keydata[tmp2] & 255) + tmp3 & 255);
                tmp1 = this.__keydata[tmp2];
                this.__keydata[tmp2] = this.__keydata[tmp3];
                this.__keydata[tmp3] = (byte)tmp1;
                tmp0 = (byte)((this.__keydata[tmp2] & 255) + (this.__keydata[tmp3] & 255) & 255);
                data_de[counter] = (byte)(data[counter] ^ this.__keydata[tmp0]);
                counter++;
            }
            return data_de;
        }
    }
}
