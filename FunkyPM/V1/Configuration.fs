namespace FunkyPM.V1

module Configuration =

    [<RequireQualifiedAccess>]
    type ConfigurationValue =
        | Provided of string
        | EnvironmentVariable of string

        static member Deserialize(value: string) =
            match value with
            | _ when value.StartsWith("$") -> value.Substring(1) |> EnvironmentVariable
            | _ -> Provided value


    type BackingStore = SQLite of SQLiteBackingStoreConfiguration

    and SQLiteBackingStoreConfiguration = { StorePath: string }

    type FunkyPMConfiguration = { BackingStore: BackingStore }
