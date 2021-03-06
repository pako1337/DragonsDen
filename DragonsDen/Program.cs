﻿using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace DragonsDen
{
    class Program
    {
        private static Dictionary<string, int> filesFrequency = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            using (var repo = new Repository(args[0]))
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

            foreach (var f in filesFrequency.Select(f => new { Frequency = f.Value, File = f.Key })
                                            .OrderByDescending(f => f.Frequency))
            {
                Console.WriteLine($"{f.Frequency}\t{f.File}");
            }

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
