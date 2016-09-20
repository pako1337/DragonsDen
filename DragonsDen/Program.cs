using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace DragonsDen
{
    class Program
    {
        private static Dictionary<string, int> filesFrequency = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            using (var repo = new Repository("c:/users/jporwol/documents/projects/DragonsDen"))
            {
                foreach (var log in repo.Commits)
                {
                    Console.WriteLine(log.Id);
                    foreach (var file in log.Tree)
                    {
                        int frequency = 0;
                        filesFrequency.TryGetValue(file.Path, out frequency);
                        frequency++;
                        filesFrequency[file.Path] = frequency;
                    }
                }
            }

            filesFrequency.Select(f => new { Frequency = f.Key, File = f.Value })
                          .OrderBy(f => f.Frequency)
                          .ToList()
                          .ForEach(f => Console.WriteLine($"{f.File}\t{f.Frequency}"));

            Console.ReadKey();
        }
    }
}
