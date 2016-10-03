// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open LibGit2Sharp
open System

let getRepoFiles (log:Commit) =
    "No parent at all"

let getCommitChangedFiles (log:Commit) =
    "Parent!"

let getChangedFiles (log:Commit) =
    match not (Seq.isEmpty log.Parents) with
    | true -> getCommitChangedFiles log
    | false -> getRepoFiles log

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    use repo = new Repository(argv.[0])
    let filter = new CommitFilter()
    filter.SortBy <- CommitSortStrategies.Time
    let logs = repo.Commits.QueryBy filter
    logs
    |> Seq.map  getChangedFiles
    |> Seq.iter (fun x -> printf "%s " x)

//         match log with
//         | true -> Console.WriteLine "parents"
//         | false -> Console.WriteLine "asdfasdf")

//    System.Console.WriteLine(logs)
    System.Console.ReadKey() |> ignore

//  if (log.Parents.Any())
//  {
//      var oldTree = log.Parents.First().Tree;
//      var changes = repo.Diff.Compare<TreeChanges>(oldTree, log.Tree);
//      foreach (var change in changes)
//      {
//          UpdateFileFrequency(change.Path);
//      }
//  }
//  else
//  {
//      foreach (var file in log.Tree)
//      {
//          TreeEntryParsing(file);
//      }
//  }
    0 // return an integer exit code