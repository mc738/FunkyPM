﻿module FunkyPM.V1.Operations

module Common =
    
    
    type Context =
        {
            ItemGroups: string
            Categories: string
        }
    
    let splitPathValue (path: string) = path.Split([|'/'|])
    
    let substituteValue (ctx) (value: string) =
        
        
        ()
    
