<meta name="funkypm:id" content="0ff6bd8629a44f1d8ba713e24b27fdcb">
<meta name="funkypm:" content="">

# Add plugin support

Add plugin support ot the core library

## Implementation

This will be based on `Faaz.FSharp.Compiled` to start with, 
allowing plugins to be written in `.fsx` files and compiled to a `dll`.

They can then be added to the operations pipeline.

## Comments

* This is an initial implementation, it is likely to change over time.
* The first plugin is likely to be `git` integration.