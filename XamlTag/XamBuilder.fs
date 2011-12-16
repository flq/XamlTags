namespace XamlTag

open System
open System.Dynamic

type XamBuilder()=
  let converter = new SetterFactory()
  member x.Start<'a>() : [<return: System.Runtime.CompilerServices.DynamicAttribute>] Object =
    new Xaml<'a>(x,converter) :> Object
  interface IXamlBuilder with
    member x.Start<'a>()=
      x.Start<'a>()
