namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("1) load {filename}\n2) print {pageNum}\n3) save {filename}");
            System.Console.WriteLine("4) export {N} {filename}\n5) subjects\n6) subject {subj}\n7) instructors\n8) image {filename}");

            ConsoleUserInterface.Run();
        }
    }
}
