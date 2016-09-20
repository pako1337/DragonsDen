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
                foreach (var log in repo.Commits.QueryBy(new CommitFilter() { SortBy = CommitSortStrategies.Time }))
                {
                    if (log.Parents.Any())
                    {
                        var oldTree = log.Parents.First().Tree;
                        var changes = repo.Diff.Compare<TreeChanges>(oldTree, log.Tree);
                        foreach (var change in changes)
                        {
                            UpdateFileFrequency(change.Path);
                        }
                    }
                    else
                    {
                        foreach (var file in log.Tree)
                        {
                            TreeEntryParsing(file);
                        }
                    }
                }
            }

            filesFrequency.Select(f => new { Frequency = f.Key, File = f.Value })
                          .OrderBy(f => f.Frequency)
                          .ToList()
                          .ForEach(f => Console.WriteLine($"{f.File}\t{f.Frequency}"));

            Console.ReadKey();
        }

        private static void TreeEntryParsing(TreeEntry file)
        {
            if (file.Mode == Mode.Directory)
            {
                foreach (var child in (file.Target as Tree))
                {
                    TreeEntryParsing(child);
                }
            }
            else
                UpdateFileFrequency(file.Path);
        }

        private static void UpdateFileFrequency(string file)
        {
            int frequency = 0;
            filesFrequency.TryGetValue(file, out frequency);
            frequency++;
            filesFrequency[file] = frequency;
        }
    }
}
