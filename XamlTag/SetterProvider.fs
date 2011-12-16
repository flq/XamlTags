namespace XamlTag

open System
open System.Collections
open XamlModule

type SetterContext = { PropertyName : string; PropertyType : Type; Value : Object;  }

type internal ISetterProvider=
  abstract member Match : SetterContext->bool
  abstract member Setter<'a> : SetterContext->('a->unit)

type internal StandardSetterProvider()=
  interface ISetterProvider with
    member x.Match ctx=
      ctx.PropertyType.IsAssignableFrom(ctx.Value.GetType())
    member x.Setter<'a> ctx=
      fun (a : 'a) -> a?(ctx.PropertyName) <- ctx.Value

type internal ConverterBasedSetter()=
  let mutable converter = None
  interface ISetterProvider with
    member x.Match ctx=
      converter <- ctx.PropertyType.GetConverter()
      match converter with
      | Some(converter) -> true
      | None -> false
    member x.Setter<'a> ctx=
      let converted = (Option.get converter).ConvertFrom(null,System.Globalization.CultureInfo.InvariantCulture,ctx.Value)
      fun (a : 'a) -> a?(ctx.PropertyName) <- converted

type internal ArrayToListSetter()=
   interface ISetterProvider with
    member x.Match ctx=
      typeof<IList>.IsAssignableFrom(ctx.PropertyType) && typeof<Object[]>.IsAssignableFrom(ctx.Value.GetType())
    member x.Setter<'a> ctx=
     fun (a : 'a) ->
        let v = ctx.Value :?> Object[]
        let targetProp : IList = a?(ctx.PropertyName)
        let add = targetProp.Add >> ignore
        v |> Array.iter add

