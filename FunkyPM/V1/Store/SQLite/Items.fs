namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open FunkyPM.V1.Store.Common.Domain

[<RequireQualifiedAccess>]
module Items =

    open Freql.Sqlite
    open FunkyPM.V1.Store.SQLite.Persistence
    open FunkyPM.V1.Store.Common.Domain.Items

    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =

        let getAll (ctx: SqliteContext) =
            Operations.selectItemGroupVersionStatusTypeRecords ctx [] []


    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemRecord ctx [ "WHERE id = @0" ] [ id ] |> Option.isSome

    let add (ctx: SqliteContext) (newItem: NewItem) =
        let id = newItem.Id

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ Id = id
               ItemGroupVersionId = newItem.ItemGroupVersionId
               CreatedOn = newItem.CreatedOn
               Active = true }
            : Parameters.NewItem)
            |> Operations.insertItem ctx

            AddResult.Success id
