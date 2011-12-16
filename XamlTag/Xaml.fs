namespace XamlTag

open System
open System.Collections.Generic
open System.Dynamic

type Xaml<'a> internal (b : IXamlBuilder, c : SetterFactory)=
  inherit DynamicObject()
  
  let model = new ConstructModel<'a>(b,c)
  let thing = lazy(Activator.CreateInstance<'a>() |> model.Play)
  
  let (|Single|Multi|NestedFunc|NestedManyFunc|) (binder : InvokeMemberBinder, args : Object[]) =
    if binder.Name.Contains("And") then
      Multi
    elif args.[0] :? Func<IXamlBuilder,XamlCreator> then
      NestedFunc
    elif args.[0] :? Func<IXamlBuilder,XamlCreator[]> then
      NestedManyFunc
    else
      Single
     
  member x.Create()=thing.Force()

  interface XamlCreator with
    member x.GetXamlObject()=
      x.Create() :> Object

  override x.TryInvokeMember(binder, args, result)=
    match (binder,args) with
    | Single -> model.AddSingle binder.Name args.[0]
    | Multi ->  model.AddMulti binder.Name args
    | NestedFunc -> model.AddNested binder.Name args
    | NestedManyFunc -> model.AddNestedMany binder.Name args
    result <- x
    true
      
    