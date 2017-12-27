using CommandLine;
using CommandLine.Text;

namespace ThreadsBattle
{
    public class Options
    {
        [Option("n", Required = true, HelpText = "Number of added threads.")]
        public int N { get; set; }

        [Option("m", Required = true, HelpText = "Number of taken threads.")]
        public int M { get; set; }

        [Option("a", Required = true, HelpText = "Minimum possible random number added in queue.")]
        public int A { get; set; }

        [Option("b", Required = true, HelpText = "Maximum possible random number added in queue.")]
        public int B { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText();
            help.AddPreOptionsLine("Threads battle");
            help.AddOptions(this);
            help.AddPostOptionsLine(@"Examples:");
            help.AddPostOptionsLine(@"-n 5 -m 15 -a 0 -b 100");
            return help;
        }
    }
}
