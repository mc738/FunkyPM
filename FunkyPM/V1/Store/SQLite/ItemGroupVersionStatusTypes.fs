namespace FunkyPM.V1.Store.SQLite

#nowarn "250002"
#nowarn "250011"

[<RequireQualifiedAccess>]
module ItemGroupVersionStatusTypes =

    open Freql.Sqlite
    open FunkyPM.V1.Store.Common.Domain
    open FunkyPM.V1.Store.Common.Domain.ItemGroupVersionStatusTypes
    open FunkyPM.V1.Store.SQLite.Persistence
    
    [<CompilerMessage("This module is for internal use only and results should be mapped to domain types when used outside of FunkyPM.V1.Store.SQLite. Validation checks might also be skipped. To suppress this warning add #nowarn \"250011\"",
                      250011)>]
    module internal Internal =



        ()

    let exists (ctx: SqliteContext) (id: string) =
        Operations.selectItemGroupVersionStatusTypeRecord ctx [ "WHERE = @0" ] [ id ]
        |> Option.isSome

    let add (ctx: SqliteContext) (newStatus: NewItemGroupVersionStatusType) =
        let id = newStatus.Id.GetValue()

        match exists ctx id with
        | true -> AddResult.AlreadyExists
        | false ->
            ({ Id = id
               Name = newStatus.Name
               ItemGroupVersionId = newStatus.ItemGroupVersionId
               CreatedOn = newStatus.CreatedOn
               Active = true }
            : Parameters.NewItemGroupVersionStatusType)
            |> Operations.insertItemGroupVersionStatusType ctx
            
            AddResult.Success id