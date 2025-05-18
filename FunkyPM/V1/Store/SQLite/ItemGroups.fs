namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

open FunkyPM.V1.Store.Common.Domain

[<RequireQualifiedAccess>]
module ItemGroups =

    open Freql.Sqlite
    open FunkyPM.V1.Store.Common.Domain
    open FunkyPM.V1.Store.Common.Domain.ItemGroups
    open FunkyPM.V1.Store.SQLite.Persistence

    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =

        let getForProject (ctx: SqliteContext) (projectId: string) (activeStatus: ActiveStatus) =
            Operations.selectItemGroupRecords
                ctx
                [ "WHERE project_id = @0"; activeStatus.GetQueryText("AND ") ]
                [ projectId ]

        let getAll (ctx: SqliteContext) (activeStatus: ActiveStatus) =
            Operations.selectItemGroupRecords ctx [ activeStatus.GetQueryText("WHERE ") ] []

    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemGroupRecord ctx [ "WHERE id = @0" ] [ id ] |> Option.isSome

    let add (ctx: SqliteContext) (newItemGroup: NewItemGroup) =
        let id = newItemGroup.Id.GetValue()

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ Id = id
               ProjectId = newItemGroup.ProjectId
               Name = newItemGroup.Name
               CreatedOn = newItemGroup.CreatedOn
               Active = true }
            : Parameters.NewItemGroup)
            |> Operations.insertItemGroup ctx

            AddResult.Success id
