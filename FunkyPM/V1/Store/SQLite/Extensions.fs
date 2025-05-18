namespace FunkyPM.V1.Store.SQLite

open FunkyPM.V1.Store.Common.Domain

[<AutoOpen>]
module Extensions =
    
    type ActiveStatus with
        
        member status.GetQueryText(prefix: string) =
            match status with
            | ActiveStatus.Active -> $"{prefix}active = TRUE"
            | ActiveStatus.Inactive -> $"{prefix}active = FALSE"
            | ActiveStatus.All -> $"{prefix}1 = 1"

