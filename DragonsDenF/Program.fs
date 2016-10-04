// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open LibGit2Sharp
open System

let rec getFileNamesFromTree (treeEntry:TreeEntry) =
    match treeEntry.Mode with
        | Mode.Directory ->
            treeEntry.Target :?> Tree
            |> Seq.collect getFileNamesFromTree
        | _ ->  seq { yield treeEntry.Path }

let getRepoFiles (log:Commit) =
    log.Tree
    |> Seq.collect getFileNamesFromTree

let getCommitChangedFiles (repo:Repository) (log:Commit) =
    let oldTree = log.Parents |> Seq.find(fun x -> true)
    repo.Diff.Compare<TreeChanges>(oldTree.Tree, log.Tree)
    |> Seq.map(fun x -> x.Path)

let getChangedFiles (repo:Repository) (log:Commit) =
    match not (Seq.isEmpty log.Parents) with
    | true -> getCommitChangedFiles repo log
    | false -> getRepoFiles log

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    use repo = new Repository(argv.[0])
    let filter = new CommitFilter()
    let getChangedFilesForRepo = getChangedFiles repo
    filter.SortBy <- CommitSortStrategies.Time
    repo.Commits.QueryBy filter
    |> Seq.collect getChangedFiles
    |> Seq.iter (fun x -> printf "%s " x)

    System.Console.ReadKey() |> ignore

    0