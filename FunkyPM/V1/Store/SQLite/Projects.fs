namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"

open FunkyPM.V1.Store.Common.Domain

[<RequireQualifiedAccess>]
module Projects =

    open Freql.Sqlite
    open FunkyPM.V1.Store.SQLite.Persistence

    open FunkyPM.V1.Store.Common.Domain.Projects

    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectProjectRecord ctx [ "WHERE id = @0" ] [ id ] |> Option.isSome

    let add (ctx: SqliteContext) (newProject: NewProject) =
        let id = newProject.Id.GetValue()

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ Id = id
               Name = newProject.Name
               CreatedOn = newProject.CreatedOn }
            : Parameters.NewProject)
            |> Operations.insertProject ctx
            
            AddResult.Success id

    let listOverviews (ctx: SqliteContext) =
        Operations.selectProjectRecords ctx [] []
        |> List.map (fun p ->
            ({ Id = p.Id
               Name = p.Name
               CreatedOn = p.CreatedOn
               Active = true }
            : ProjectOverview))
