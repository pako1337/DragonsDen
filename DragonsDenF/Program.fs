// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open LibGit2Sharp
open System

let getRepoFiles (log:Commit) =
    log.Tree
    |> Seq.collect (fun treeEntry ->
                        match treeEntry.Mode with
                        | Mode.Directory -> seq { yield "asdf" }
                        | _ ->  seq { yield treeEntry.Path })

let getCommitChangedFiles (log:Commit) =
    seq { yield "Parent" }

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
    |> Seq.collect  getChangedFiles
    |> Seq.iter (fun x -> printf "%s " x)

    System.Console.ReadKey() |> ignore

    0