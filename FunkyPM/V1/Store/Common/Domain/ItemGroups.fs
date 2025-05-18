namespace FunkyPM.V1.Store.Common.Domain

module ItemGroups =

    open System

    type NewItemGroup =
        { Id: IdType
          ProjectId: string
          Name: string
          CreatedOn: DateTime }

    type NewItemGroupVersion =
        { Id: IdType
          Version: EntityVersion
          ItemGroupId: string
          CreatedOn: DateTime }

    type NewItemGroupVersionCategory =
        { Id: IdType
          ItemGroupVersionId: string
          Name: string
          Parent: string option
          CreatedOn: DateTime }

    type ItemGroupOverview =
        { Id: string
          Name: string
          CreatedOn: DateTime
          Versions: ItemGroupVersionOverview list }

    and ItemGroupVersionOverview = { Id: string }
