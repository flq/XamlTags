namespace XamlTag

open System
open System.ComponentModel
open System.Reflection
open System.Windows
open System.Windows.Controls
open XamlModule

type internal SetterFactory()=    
    
    let knownSetterProvider = new System.Collections.Generic.List<ISetterProvider>()
    do
      knownSetterProvider.Add(new StandardSetterProvider())
      knownSetterProvider.Add(new ConverterBasedSetter())

    let convert (converter : TypeConverter option) value =
        match converter with
        | Some(converter) -> converter.ConvertFrom(null,System.Globalization.CultureInfo.InvariantCulture,value)
        | None -> failwith "dead"

    member x.GetAction<'a> propName (value : Object)=
        let ctx = { PropertyName = propName; Value = value; PropertyType = (typeof<'a>.GetPropertyType propName) }
        try
          let setter = knownSetterProvider |> Seq.tryFind (fun provider -> provider.Match ctx)
          match setter with
          | Some(setter) -> setter.Setter<'a> ctx
          | None -> failwith "End of road"
        with
            | :? Exception -> raise(new NotSupportedException(sprintf "Could not resolve a way to set %s to target (%s:%s)" (value.GetType().Name) ctx.PropertyName ctx.PropertyType.Name))

