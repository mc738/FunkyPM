namespace FunkyPM.Store.V1.SQLite

open Freql.Sqlite
open FunkyPM.V1.Store.Common
open FunkyPM.V1.Store.Common.Domain
open FunkyPM.V1.Store.SQLite
open Microsoft.Extensions.Logging

type SQLiteFunkyPMStore(ctx: SqliteContext, log: ILogger) =

    interface IFunkyPMStore with
        member this.AddItemGroup(newItemGroup) =
            match Projects.exists ctx newItemGroup.ProjectId with
            | false ->
                log.LogWarning("Project {ProjectId} does not exist. Can not add item group.", newItemGroup.ProjectId)
                AddResult.ParentEntityNoteFound "project"
            | true -> ItemGroups.add ctx newItemGroup

        member this.AddItemGroupVersion(newItemGroupVersion) =
            match ItemGroups.exists ctx newItemGroupVersion.ItemGroupId with
            | false ->
                log.LogWarning(
                    "Item group {ItemGroupId} does not exist. Can not add item group version.",
                    newItemGroupVersion.ItemGroupId
                )

                AddResult.ParentEntityNoteFound "item-group"
            | true -> ItemGroupVersions.add ctx newItemGroupVersion

        member this.AddProject(newProject) = Projects.add ctx newProject

        member this.AddItem(newItem) =
            match ItemGroupVersions.exists ctx newItem.ItemGroupVersionId with
            | false ->
                log.LogWarning(
                    "Item group version {ItemGroupVersionId} does not exist. Can not add item.",
                    newItem.ItemGroupVersionId
                )

                AddResult.ParentEntityNoteFound "item-group-version"
            | true -> Items.add ctx newItem

        member this.AddItemGroupVersionStatusType(newItemGroupVersionStatusType) =
            match ItemGroupVersions.exists ctx newItemGroupVersionStatusType.ItemGroupVersionId with
            | false ->
                log.LogWarning(
                    "Item group version {ItemGroupVersionId} does not exist. Can not add item group version status type.",
                    newItemGroupVersionStatusType.ItemGroupVersionId
                )

                AddResult.ParentEntityNoteFound "item-group-version"
            | true -> ItemGroupVersionStatusTypes.add ctx newItemGroupVersionStatusType

        member this.AddItemVersion(newItemVersion) =
            match Items.exists ctx newItemVersion.ItemId with
            | false ->
                log.LogWarning("Item {ItemId} does not exist. Can not add item version.", newItemVersion.ItemId)

                AddResult.ParentEntityNoteFound "item"
            | true -> ItemVersions.add ctx newItemVersion

        member this.AddItemVersionCategory(newItemVersionCategory) =
            match ItemVersions.exists ctx newItemVersionCategory.ItemVersionId with
            | false ->
                log.LogWarning(
                    "Item version {ItemVersionId} does not exist. Can not add item version category.",
                    newItemVersionCategory.ItemVersionId
                )

                AddResult.ParentEntityNoteFound "item-version"
            | true -> ItemVersionCategories.add ctx newItemVersionCategory
