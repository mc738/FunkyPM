namespace FunkyPM.V1.Store.Common.Domain

open System

module ItemGroupVersionStatusTypes =

    type NewItemGroupVersionStatusType =
        { Id: IdType
          Name: string
          ItemGroupVersionId: string
          CreatedOn: DateTime }
