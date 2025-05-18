namespace FunkyPM.V1.Store.Common.Domain

open System

module Projects =
    
    type NewProject =
        { Id: IdType
          ProjectId: string
          Name: string
          CreatedOn: DateTime }

    type ProjectOverview =
        { Id: string
          Name: string
          CreatedOn: DateTime
          Active: bool }

