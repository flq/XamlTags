namespace XamlTag

open System
open System.Dynamic

type XamBuilder()=
  let converter = new SetterFactory()
  member x.Start<'a>() : [<return: System.Runtime.CompilerServices.DynamicAttribute>] Object =
    new Xaml<'a>(x,converter) :> Object
  member x.Start<'a> dataContext : [<return: System.Runtime.CompilerServices.DynamicAttribute>] Object =
    new Xaml<'a>(x,converter,dataContext) :> Object
  interface IXamlBuilder with
    member x.Start<'a>()=
      x.Start<'a>()
    member x.Start<'a> dataContext=
      x.Start<'a> dataContext
