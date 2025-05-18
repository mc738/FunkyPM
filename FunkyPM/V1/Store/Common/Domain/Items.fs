namespace FunkyPM.V1.Store.Common.Domain

open System
open System.IO

module Items =

    type NewItem =
        { Id: string
          ItemGroupVersionId: string
          CreatedOn: DateTime }

    type NewItemVersion =
        { Id: IdType
          Version: EntityVersion
          ItemId: string
          Name: string
          StatusId: string
          RawData: MemoryStream
          FileType: string
          CreatedOn: DateTime }

    type NewItemVersionCategory =
        { ItemVersionId: string
          CategoryId: string }
