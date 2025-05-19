namespace FunkyPM.V1.Store.Common

open FunkyPM.V1.Store.Common.Domain
open FunkyPM.V1.Store.Common.Domain.ItemGroupVersionStatusTypes
open FunkyPM.V1.Store.Common.Domain.ItemGroups
open FunkyPM.V1.Store.Common.Domain.Items
open FunkyPM.V1.Store.Common.Domain.Projects

type IFunkyPMStore =

    abstract member AddProject: NewProject: NewProject -> AddResult

    abstract member AddItemGroup: NewItemGroup: NewItemGroup -> AddResult

    abstract member AddItemGroupVersion: NewItemGroupVersion: NewItemGroupVersion -> AddResult

    abstract member AddItemGroupVersionStatusType:
        NewItemGroupVersionStatusType: NewItemGroupVersionStatusType -> AddResult

    abstract member AddItemVersionCategory: NewItemVersionCategory: NewItemVersionCategory -> AddResult

    abstract member AddItem: NewItem: NewItem -> AddResult

    abstract member AddItemVersion: NewItemVersion: NewItemVersion -> AddResult
