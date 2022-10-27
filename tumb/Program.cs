using static TheUltimateMicroBeast.Logger;
    
namespace TheUltimateMicroBeast
{
    public class Shell
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"The Ultimate Micro-Beast version {Info.Version}");
            string flnm = args.Length < 1 ? "code.tumb" : args[0];

            string[] code = File.ReadAllLines(flnm);
            List<byte> memory = new();

            for (int i = 0; i < 65536; i++) memory.Add(0);

            byte[] bytes = Assembler.Assemble(code).ToArray();
            string s = "";
            foreach (byte b in bytes)
                s += Assembler.maxlen(Convert.ToString(b, 16), 2) + " ";

            Console.WriteLine("=== BYTECODE OF " + flnm + " ===\n" + s /* + "\n\n" + bytes.Length */ + "\n");

            string[] deassembled = Assembler.Deassemble(bytes).ToArray();
            /*foreach (string ln in deassembled)
                Console.WriteLine(ln);*/

            Log("Reading boot.bin...");
            byte[] bootldr = File.ReadAllBytes("boot.bin");
            Log("Writing boot.bin to $0000-$1FFF...");
            s = "";
            for (int i = 0; i < bootldr.Length; i++)
            {
                if (i >= 0x1fff)
                {
                    Log("Bootloader too large!", LogType.Error);
                    return 1;
                }
                memory[0x0000 + i] = bootldr[i];
                s += Assembler.maxlen(Convert.ToString(memory[0x0000 + i], 16), 2) + " ";
            }
            Console.WriteLine("=== BOOTLOADER BYTECODE ===\n" + s + "\n");

            Log("Writing " + flnm + " to $8000-$BFFF...");
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i >= 0xbfff - 0x8000)
                {
                    Log("Program too large!", LogType.Error);
                    return 1;
                }
                memory[0x8000 + i] = bytes[i];
            }
            Log("Running bootloader/boot.bin now!");
            Console.WriteLine(Runner.Exec(bootldr, ref memory, 164379154));
            Log("Running $8000-$BFFF now!");
            Console.WriteLine(Runner.Exec(bytes, ref memory, 760249));
            return 0;
        }
    }
}