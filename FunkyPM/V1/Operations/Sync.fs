namespace FunkyPM.V1.Operations

open System.IO

module Sync =
    
    let withFile (path: string) (fn: MemoryStream -> unit) =
        use fs = File.OpenRead(path)
        
        use ms = new MemoryStream()
        
        fs.CopyTo(ms)
        
        ms.Position <- 0L
        
        fn ms
    