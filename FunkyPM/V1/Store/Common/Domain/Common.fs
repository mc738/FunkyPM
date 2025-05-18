namespace FunkyPM.V1.Store.Common.Domain

open System

[<AutoOpen>]
module Common =

    [<RequireQualifiedAccess>]
    type EntityVersion =
        | Latest
        | Specific of int

    [<RequireQualifiedAccess>]
    type ActiveStatus =
        | Active
        | Inactive
        | All

    type IdType =
        | Provided of string
        | Generated

        member id.Generate() =
            match id with
            | Provided _ -> id
            | Generated -> Guid.NewGuid().ToString("n") |> IdType.Provided

        [<CompilerMessage("Calling GetValue() in IdType.Generated types will generate 2 different values. For deterministic values call Generate() first and use the return. To surpress this warning add #nowarn \"250002\"",
                          250002)>]
        member id.GetValue() =
            match id with
            | Provided s -> s
            | Generated -> Guid.NewGuid().ToString("n")

    type AddResult =
        | Success of NewId: string
        | AlreadyExists
        | ParentEntityNoteFound of EntityType: string