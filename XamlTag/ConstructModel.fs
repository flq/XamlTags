namespace XamlTag

open System
open System.ComponentModel
open System.Collections.Generic
open System.Dynamic
open System.Windows
open XamlModule

type internal ConstructModel<'a>(b : IXamlBuilder, conv : SetterFactory, dc : Object option)=
  let builder = b
  let setterFactory = conv
  let dataContext = dc

  let actions = new List<'a->unit>()
  let add = actions.Add

  let setDataContext datactx=
    fun (a : 'a) ->
      if box a :? FrameworkElement then
        let fw = box a :?> FrameworkElement
        fw.DataContext <- datactx
  
  do 
    match dataContext with
    | Some(dataContext) -> dataContext |> setDataContext |> add
    | None -> ()

  let failIfNoFrameworkElement=
    if not (typeof<FrameworkElement>.IsAssignableFrom(typeof<'a>)) then
      invalidOp(sprintf "Type %s is not assignable to FrameworkElement, Binding is therefore not supported" typeof<'a>.Name)
  
  let getXaml (obj  : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator>
    let xaml = func.Invoke builder
    xaml.GetXamlObject()

  let getManyXaml (obj : Object)=
    let func = obj :?> Func<IXamlBuilder,XamlCreator[]>
    let xamls = func.Invoke builder
    xamls |> Array.map (fun x -> x.GetXamlObject())
  
  member x.AddSingle name (value : Object) = 
    setterFactory.GetAction<'a> name value |> add
  
  member x.AddMulti name args =
    name |> splitOnAnd |> Seq.zip args |> Seq.iter (fun (value,name) -> x.AddSingle name value)

  member x.AddNested name (args : Object[])= 
    getXaml(args.[0]) |> x.AddSingle name

  member x.AddNestedMany name (args : Object[])= 
    getManyXaml(args.[0]) |> x.AddSingle name

  member x.AddBinding (binder : InvokeMemberBinder) (args : Object[])=
    failIfNoFrameworkElement
    let propertyName = binder.Name.Replace("Bind","")
    let depProp = typeof<'a>.FindDependencyProperty(binder.Name.Replace("Bind",""))
    match depProp with
    | Some(depProp) -> setterFactory.GetBindAction<'a> depProp args binder.CallInfo.ArgumentNames |> add
    | None -> raise(new ArgumentException(sprintf "No DependencyProperty '%s' found on type '%s'" propertyName typeof<'a>.Name))
    

  member x.Play thing = 
    actions |> Seq.iter (fun applyTo -> applyTo thing)
    thing



  

