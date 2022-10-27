namespace TheUltimateMicroBeast
{
    public class Assembler
    {
        public static string maxlen(string a, int l)
        {
            for (int i = a.Length; i < l; i++)
            {
                a = "0" + a;
            }
            return a;
        }
        public static List<byte> Assemble(string[] code)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < code.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(code[i]))
                {
                    if (code[i][0] != "#"[0])
                    {
                        string[] ln = code[i].Split(" ");
                        string[] args = ln[1].Split(",");

                        string addr, addr0s, addr1s, hxvals, reg0s, reg1s;
                        byte addr0, addr1, newln, hxval, reg0, reg1;
                        if (ln[0] == "cont")
                        {
                            bytes.Add(0x12);
                            addr = maxlen(args[0], 4);

                            addr0s = addr[0..1];
                            addr1s = addr[2..3];

                            addr0 = Convert.ToByte(addr0s, 16);
                            addr1 = Convert.ToByte(addr1s, 16);

                            bytes.Add(addr0);
                            bytes.Add(addr1);

                            if (args.Length > 1)
                                if (string.IsNullOrWhiteSpace(args[1]))
                                    newln = Convert.ToByte(args[1]);
                                else newln = 0;
                            else newln = 0;

                            bytes.Add(newln);
                            break;
                        }
                        else if (ln[0] == "contr")
                        {
                            bytes.Add(0x14);
                            addr = maxlen(args[0], 2);
                            addr0 = Convert.ToByte(addr, 16);

                            bytes.Add(addr0);

                            if (args.Length > 1) newln = Convert.ToByte(args[1]);
                            else newln = 0;

                            bytes.Add(newln);
                            bytes.Add(0);
                        }
                        else if (ln[0] == "rpoke")
                        {
                            bytes.Add(0x10);
                            hxvals = maxlen(args[0], 2);
                            hxval = Convert.ToByte(hxvals, 16);

                            bytes.Add(hxval);

                            reg0s = maxlen(args[1], 2);
                            reg0 = Convert.ToByte(reg0s, 16);

                            bytes.Add(reg0);
                            bytes.Add(0);
                        }
                        else if (ln[0] == "contrc")
                        {
                            bytes.Add(0x18);
                            addr = maxlen(args[0], 2);
                            addr0 = Convert.ToByte(addr, 16);

                            bytes.Add(addr0);

                            if (args.Length > 1) newln = Convert.ToByte(args[1]);
                            else newln = 0;

                            bytes.Add(newln);
                            bytes.Add(0);
                        }
                    }
                }
            }
            return bytes;
        }
        public static string[] Deassemble(byte[] bytes)
        {
            List<string> lns = new();
            for (int i = 0; i < bytes.Length; i += 4)
            {
                string addr, addr0s, addr1s, hxvals, reg0s, reg1s;
                byte addr0, addr1, newln, hxval, reg0, reg1;
                switch (bytes[i])
                {
                    case 0x12:
                        addr0s = Convert.ToString(bytes[i + 1], 16);
                        addr1s = Convert.ToString(bytes[i + 2], 16);
                        newln = bytes[i + 3];
                        lns.Add("cont " +
                            (addr0s == "0" ? "" : addr0s) + addr1s +
                            (newln != 0x00 ? "," + newln : "")
                        );
                        break;
                    case 0x14:
                        addr0s = Convert.ToString(bytes[i + 1], 16);
                        addr1s = Convert.ToString(bytes[i + 2], 16);
                        lns.Add("contr " +
                            addr0s +
                            (addr1s != "1" ? "," + addr1s : "")
                        );
                        break;
                    case 0x10:
                        hxval = bytes[i + 1];
                        hxvals = hxval.ToString();
                        reg0s = Convert.ToString(bytes[i + 2], 16);
                        lns.Add("rpoke " +
                            hxvals + "," +
                            reg0s
                        );
                        break;
                }
            }
            //Console.WriteLine(lns.Count);
            return lns.ToArray();
        }
    }
}