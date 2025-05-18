namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open Freql.Sqlite
open FunkyPM.V1.Store.Common.Domain
open FunkyPM.V1.Store.Common.Domain.ItemGroups
open FunkyPM.V1.Store.SQLite.Persistence

[<RequireQualifiedAccess>]
module ItemGroupVersionCategories =

    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =
        ()

    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemGroupVersionRecord ctx [ "WHERE id = @0" ] [ id ]
        |> Option.isSome

    let add (ctx: SqliteContext) (newItemGroupVersionCategory: NewItemGroupVersionCategory) =
        let id = newItemGroupVersionCategory.Id.GetValue()

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ Id = id
               ItemGroupVersionId = newItemGroupVersionCategory.ItemGroupVersionId
               Name = newItemGroupVersionCategory.Name
               Parent = newItemGroupVersionCategory.Parent
               CreatedOn = newItemGroupVersionCategory.CreatedOn
               Active = true }
            : Parameters.NewItemGroupVersionCategory)
            |> Operations.insertItemGroupVersionCategory ctx

            AddResult.Success id
