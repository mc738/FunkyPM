namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open FunkyPM.V1.Store.Common.Domain

[<RequireQualifiedAccess>]
module ItemGroupVersions =

    open Freql.Sqlite
    open FunkyPM.V1.Store.Common.Domain
    open FunkyPM.V1.Store.Common.Domain.ItemGroups
    open FunkyPM.V1.Store.SQLite.Persistence
    
    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =

        let getVersionsForItemGroup (ctx: SqliteContext) (itemGroupId: string) =
            Operations.selectItemGroupVersionRecords ctx [ "WHERE item_group_id = @0" ] [ itemGroupId ]


        let getLatestVersionForItemGroup (ctx: SqliteContext) (itemGroupId: string) =
            Operations.selectItemGroupVersionRecord
                ctx
                [ "WHERE item_group_id = @0 ORDER BY version DESC" ]
                [ itemGroupId ]

        let getSpecificVersionForItemGroup (ctx: SqliteContext) (itemGroupId: string) (version: int) =
            Operations.selectItemGroupVersionRecord
                ctx
                [ "WHERE item_group_id = @0 AND version = @1" ]
                [ itemGroupId; version ]


    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemGroupVersionRecord ctx [ "WHERE id = @0" ] [ id ]
        |> Option.isSome

    let add (ctx: SqliteContext) (newItemGroupVersion: NewItemGroupVersion) =
        let id = newItemGroupVersion.Id.GetValue()

        let op (version: int) =
            ({ Id = id
               ItemGroupId = newItemGroupVersion.ItemGroupId
               Version = version
               CreatedOn = newItemGroupVersion.CreatedOn }
            : Parameters.NewItemGroupVersion)
            |> Operations.insertItemGroupVersion ctx
            
            AddResult.Success id

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            match newItemGroupVersion.Version with
            | EntityVersion.Latest ->
                let version =
                    Internal.getLatestVersionForItemGroup ctx newItemGroupVersion.ItemGroupId
                    |> Option.map (_.Version)
                    |> Option.defaultValue 1

                op version
            | EntityVersion.Specific version ->
                match
                    Internal.getSpecificVersionForItemGroup ctx newItemGroupVersion.ItemGroupId version
                    |> Option.isSome
                with
                | true -> AddResult.AlreadyExists
                | false -> op version
