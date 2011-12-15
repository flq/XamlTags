namespace XamlTag

open System
open System.ComponentModel
open System.Collections.Generic
open XamlModule

type internal ConstructModel<'a>(b : IXamlBuilder, conv : ValueConverter)=
  let builder = b
  let converter = conv

  let dict = new Dictionary<string,Object>()
  let addVK (v,k) = dict.Add(k, v)
  
  let getXaml (obj  : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator>
    let xaml = func.Invoke builder
    xaml.GetXamlObject()

  let getManyXaml (obj : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator[]>
    let xamls = func.Invoke builder
    xamls |> Array.map (fun x -> x.GetXamlObject())
  
  member x.AddSingle name value = 
    let v = converter.GetValue<'a> name value
    dict.Add(name, v)
  
  member x.AddMulti name args =
    name |> splitOnAnd |> Seq.zip args |> Seq.iter addVK

  member x.AddNested name (args : Object[])= 
    getXaml(args.[0]) |> x.AddSingle name

  member x.AddNestedMany name (args : Object[])= 
    getManyXaml(args.[0]) |> x.AddSingle name

  member x.Play thing = 
    dict |> Seq.iter (fun kv -> thing?(kv.Key) <- kv.Value)
    thing



  

