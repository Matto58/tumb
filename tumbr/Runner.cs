namespace TheUltimateMicroBeast
{
    public class Runner
    {
        public static string Exec(byte[] code, ref List<byte> mem, int passcode = 0)
        {
            string output = "";
            for (int i = 0; i < code.Length; i += 4)
            {

                string addrs, addr0s, addr1s, hxvals, reg0s, reg1s, contval, newlns;
                byte addr0, addr1, newln, hxval, reg0, reg1;
                short addr;
                switch (code[i])
                {
                    case 0x12:
                        addr0s = Convert.ToString(code[i + 1], 16);
                        addr1s = Convert.ToString(code[i + 2], 16);
                        newln = code[i + 3];

                        addrs = addr0s + addr1s;
                        addr = Convert.ToInt16(addrs, 16);
                        contval = Convert.ToString(mem[addr], 16);
                        output += Assembler.maxlen(contval, 2) + (newln != 0x0 ? "\n" : "");
                        break;
                    case 0x14:
                        reg0 = code[i + 1];
                        newln = code[i + 2];
                        contval = Convert.ToString(mem[0x5f00 + reg0], 16);
                        output += Assembler.maxlen(contval, 2) + (newln != 0x0 ? "\n" : "");
                        break;
                    case 0x10:
                        hxval = code[i + 1];
                        reg0 = code[i + 2];
                        mem[0x5f00 + reg0] = hxval;
                        break;
                    case 0x18:
                        reg0 = code[i + 1];
                        newln = code[i + 2];
                        output += Convert.ToChar(mem[0x5f00 + reg0]) + (newln != 0x0 ? "\n" : "");
                        break;
                }
            }
            return output;
        }
    }
}