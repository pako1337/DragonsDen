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
        static void Main(string[] args)
        {
            using (var repo = new Repository("c:/users/jporwol/documents/projects/DragonsDen"))
            {
                foreach (var log in repo.Commits)
                {
                    Console.WriteLine(log.Id);
                }
            }

            Console.ReadKey();
        }
    }
}
