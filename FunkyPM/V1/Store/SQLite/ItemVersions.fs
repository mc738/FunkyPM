namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open System
open Freql.Core
open FunkyPM.V1.Store.Common.Domain

[<RequireQualifiedAccess>]
module ItemVersions =

    open Freql.Sqlite
    open FunkyPM.V1.Store.Common.Domain.Items
    open FunkyPM.V1.Store.SQLite.Persistence
    open FsToolbox.Extensions.Streams

    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =

        let getLatestVersionForItem (ctx: SqliteContext) (itemId: string) =
            Operations.selectItemGroupVersionRecord ctx [ "WHERE item_id = @0 ORDER BY version DESC" ] [ itemId ]

        let getSpecificVersionForItem (ctx: SqliteContext) (itemId: string) (version: int) =
            Operations.selectItemGroupVersionRecord ctx [ "WHERE item_id = @0 AND version = @1" ] [ itemId; version ]

    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemVersionRecord ctx [ "WHERE id = @0" ] [ id ]
        |> Option.isSome

    let add (ctx: SqliteContext) (newItemVersion: NewItemVersion) =
        let id = newItemVersion.Id.GetValue()

        //let hash = newItemVersion.RawData.GetSHA256Hash()
        let op (version: int) (hash: string) =
            ({ Id = id
               ItemId = newItemVersion.ItemId
               Name = newItemVersion.Name
               Version = version
               Status = newItemVersion.StatusId
               RawBlob = BlobField.FromStream newItemVersion.RawData
               Hash = hash
               FileType = newItemVersion.FileType
               CreatedOn = newItemVersion.CreatedOn
               Active = true }
            : Parameters.NewItemVersion)
            |> Operations.insertItemVersion ctx
            
            AddResult.Success id

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            let hash = newItemVersion.RawData.GetSHA256Hash()

            match newItemVersion.Version with
            | EntityVersion.Latest ->
                let version =
                    Internal.getLatestVersionForItem ctx newItemVersion.ItemId
                    |> Option.map (_.Version)
                    |> Option.defaultValue 1

                op version hash
            | EntityVersion.Specific version ->
                match
                    Internal.getSpecificVersionForItem ctx newItemVersion.ItemId version
                    |> Option.isSome
                with
                | true -> AddResult.AlreadyExists
                | false -> op version hash

    let getMetadata (ctx: SqliteContext) (itemVersionId: string) =
        Operations.selectItemVersionMetadataItemRecords ctx [ "WHERE item_version_id = @0" ] [ itemVersionId ]
        |> List.map (fun e -> e.ItemKey, e.ItemValue)
        |> Map.ofList

    let getMetadataValue (ctx: SqliteContext) (itemVersionId: string) (key: string) =
        Operations.selectItemVersionMetadataItemRecord
            ctx
            [ "WHERE item_version_id = @0 AND item_key = @1" ]
            [ itemVersionId; key ]
        |> Option.map (_.ItemValue)

    let metadataExists (ctx: SqliteContext) (itemVersionId: string) (key: string) =
        Operations.selectItemVersionMetadataItemRecord
            ctx
            [ "WHERE item_version_id = @0 AND item_key = @1" ]
            [ itemVersionId; key ]

    let addOrUpdateMetadata (ctx: SqliteContext) (itemVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx itemVersionId key with
        | None ->
            ({ ItemVersionId = ""
               ItemKey = failwith "todo"
               ItemValue = failwith "todo" }
            : Parameters.NewItemVersionMetadataItem)
            |> Operations.insertItemVersionMetadataItem ctx
        | Some existingValue when value.Equals(existingValue, StringComparison.OrdinalIgnoreCase) |> not ->
            ctx.ExecuteVerbatimNonQueryAnon(
                "UPDATE item_version_metadata SET item_value = @0 WHERE item_version_id = @1 AND item_key = @2",
                [ value; itemVersionId; key ]
            )
            |> ignore
        | Some _ -> ()
