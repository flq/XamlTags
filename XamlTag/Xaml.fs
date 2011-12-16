namespace XamlTag

open System
open System.Collections.Generic
open System.Dynamic

type Xaml<'a> internal (b : IXamlBuilder, c : SetterFactory, ?dc : Object)=
  inherit DynamicObject()
  
  let model = new ConstructModel<'a>(b,c,dc)
  let thing = lazy(Activator.CreateInstance<'a>() |> model.Play)
  
  let (|Single|Multi|NestedFunc|NestedManyFunc|BindingOperation|) (binder : InvokeMemberBinder, args : Object[]) =
    if binder.Name.Contains("And") then
      Multi
    elif args.[0] :? Func<IXamlBuilder,XamlCreator> then
      NestedFunc
    elif args.[0] :? Func<IXamlBuilder,XamlCreator[]> then
      NestedManyFunc
    elif binder.Name.StartsWith("Bind") then
      BindingOperation
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
    | BindingOperation -> model.AddBinding binder args
    result <- x
    true
      
    