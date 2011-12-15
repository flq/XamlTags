namespace XamlTag

open System
open System.ComponentModel
open System.Collections.Generic
open XamlModule

type internal ConstructModel<'a>(b : IXamlBuilder, conv : ValueConverter)=
  let builder = b
  let converter = conv

  let actions = new List<'a->unit>()
  let add = actions.Add
  
  let getXaml (obj  : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator>
    let xaml = func.Invoke builder
    xaml.GetXamlObject()

  let getManyXaml (obj : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator[]>
    let xamls = func.Invoke builder
    xamls |> Array.map (fun x -> x.GetXamlObject())
  
  member x.AddSingle name (value : Object) = 
    converter.GetAction<'a> name value |> add
  
  member x.AddMulti name args =
    name |> splitOnAnd |> Seq.zip args |> Seq.iter (fun (value,name) -> x.AddSingle name value)

  member x.AddNested name (args : Object[])= 
    getXaml(args.[0]) |> x.AddSingle name

  member x.AddNestedMany name (args : Object[])= 
    getManyXaml(args.[0]) |> x.AddSingle name

  member x.Play thing = 
    actions |> Seq.iter (fun applyTo -> applyTo thing)
    thing



  

