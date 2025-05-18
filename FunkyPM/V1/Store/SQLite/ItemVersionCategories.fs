namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open FunkyPM.V1.Store.Common.Domain
open FunkyPM.V1.Store.Common.Domain.Items

[<RequireQualifiedAccess>]
module ItemVersionCategories =

    open Freql.Sqlite
    open FunkyPM.V1.Store.SQLite.Persistence

    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =

        let getByItemGroupVersionAndName (ctx: SqliteContext) (itemGroupVersionId: string) (name: string) =
            Operations.selectItemGroupVersionCategoryRecord
                ctx
                [ "WHERE item_group_version_id = @0 AND name = @1" ]
                [ itemGroupVersionId; name ]


    let exists (ctx: SqliteContext) (itemVersionId: string) (categoryId: string) =
        Operations.selectItemVersionCategoryRecord
            ctx
            [ "WHERE item_version_id = @0 AND category = @1" ]
            [ itemVersionId; categoryId ]
        |> Option.isSome

    let add (ctx: SqliteContext) (newItemVersionCategory: NewItemVersionCategory) =
        match exists ctx newItemVersionCategory.ItemVersionId newItemVersionCategory.CategoryId with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ ItemVersionId = newItemVersionCategory.ItemVersionId
               CategoryId = newItemVersionCategory.CategoryId }
            : Parameters.NewItemVersionCategory)
            |> Operations.insertItemVersionCategory ctx

            AddResult.Success ""
